﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using UlstuScheduleParser.Lib.Exceptions;

namespace UlstuScheduleParser.Lib.Models
{
    /// <summary>
    /// Расписание студентов УлГТУ.
    /// </summary>
    public sealed class Schedule
    {
        /// <summary>
        /// Массив элементов расписания (пар).
        /// </summary>
        public ScheduleItem[] ScheduleItems { get; set; }

        /// <summary>
        /// Массив учебных групп.
        /// </summary>
        public StudentGroup[] StudentGroups { get; set; }

        /// <summary>
        /// Загружает и разбирает расписание с веб-сайта УлГТУ.
        /// </summary>
        /// <returns>Экземпляр расписания.</returns>
        public static async Task<Schedule> LoadFromWebSiteAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var result = new Schedule();

            // get groups
            var siteArticleUrl = "http://www.ulstu.ru/main/view/article/100";
            var scheduleHrefRegex = new Regex(@"href=""(?'url'[^\""]*raspisan[^\""]*)");
            var groups = new List<StudentGroup>();
            using (var httpClient = new HttpClient())
            {
                var siteArticleContent = await httpClient.GetStringAsync(siteArticleUrl);
                var scheduleHrefMatches = scheduleHrefRegex.Matches(siteArticleContent);
                foreach (Match scheduleHrefMatch in scheduleHrefMatches)
                {
                    var scheduleHref = "http://www.ulstu.ru" + scheduleHrefMatch.Groups["url"].Value;
                    var scheduleData = await httpClient.GetByteArrayAsync(scheduleHref);
                    groups.AddRange(StudentGroup.ParseFromScheduleData(result, scheduleHref, scheduleData));
                }
            }
            if (!groups.Any())
                throw new NoStudentGroupException();

            // get items from groups
            var weekContentCleanupRegex1 = new Regex(@"(?'base'<[\w]+)(?'rmv'\s[^\>]+)");
            var weekContentCleanupRegex2 = new Regex(@"<[\/]?(FONT|P|I|B)>");
            var weekContentCleanupRegex3 = new Regex(@"<!--([^!])*!>");
            var scheduleItems = new List<ScheduleItem>();
            foreach (var group in groups)
            {
                var weeksContent = new Dictionary<ScheduleWeekType, string>();
                using (var httpClient = new HttpClient())
                {
                    var groupData = await httpClient.GetByteArrayAsync(group.ScheduleUrl);
                    var groupContent = Encoding.GetEncoding(1251).GetString(groupData);

                    var tableSeparatorOpen = "<TABLE";
                    var tableSeparatorClose = "</TABLE>";

                    var tableIdx1Start = groupContent.IndexOf(tableSeparatorOpen, 0);
                    var tableIdx1End = groupContent.IndexOf(tableSeparatorClose, tableIdx1Start);
                    var tableIdx2Start = groupContent.IndexOf(tableSeparatorOpen, tableIdx1End);
                    var tableIdx2End = groupContent.IndexOf(tableSeparatorClose, tableIdx2Start);

                    var tableWeek1 = groupContent.Substring(tableIdx1Start, -tableIdx1Start + tableIdx1End + tableSeparatorClose.Length);
                    var tableWeek2 = groupContent.Substring(tableIdx2Start, -tableIdx2Start + tableIdx2End + tableSeparatorClose.Length);

                    weeksContent.Add(ScheduleWeekType.Первая, tableWeek1);
                    weeksContent.Add(ScheduleWeekType.Вторая, tableWeek2);
                }

                foreach (var weekContent in weeksContent)
                {
                    var weekContentValue = weekContent.Value;
                    weekContentValue = weekContentCleanupRegex1.Replace(weekContentValue, "${base}");
                    weekContentValue = weekContentCleanupRegex2.Replace(weekContentValue, "");
                    weekContentValue = weekContentValue.Replace("<BR>", "|").Replace("\r", "").Replace("\n", "");
                    weekContentValue = weekContentCleanupRegex3.Replace(weekContentValue, "");

                    var weekContentXml = XDocument.Parse(weekContentValue);
                    if (weekContentXml.Root.Name.LocalName != "TABLE")
                    {
                        throw new UnknownHtmlWeekException();
                    }
                    foreach (XElement scheduleDayXml in weekContentXml.Root.Elements())
                    {
                        if (scheduleDayXml.Name.LocalName != "TR")
                        {
                            throw new UnknownHtmlDayException();
                        }
                        var scheduleDayHeader = (XElement)scheduleDayXml.FirstNode;
                        ScheduleWeekDay dayType;
                        switch (scheduleDayHeader.Value)
                        {
                            case "Пары":
                            case "Время":
                            case "Вск":
                                continue;
                            case "Пнд":
                                dayType = ScheduleWeekDay.Понедельник;
                                break;
                            case "Втр":
                                dayType = ScheduleWeekDay.Вторник;
                                break;
                            case "Срд":
                                dayType = ScheduleWeekDay.Среда;
                                break;
                            case "Чтв":
                                dayType = ScheduleWeekDay.Четверг;
                                break;
                            case "Птн":
                                dayType = ScheduleWeekDay.Пятница;
                                break;
                            case "Сбт":
                                dayType = ScheduleWeekDay.Суббота;
                                break;
                            default:
                                throw new UnknownHtmlDayException();
                        }
                        scheduleDayHeader.Remove();
                        var scheduleDayElements = scheduleDayXml.Elements();
                        int currentPairNum = 0;
                        foreach (XElement scheduleDayElement in scheduleDayElements)
                        {
                            ++currentPairNum;
                            var parsedScheduleItems =
                                ScheduleItem.ParseFromRawData(
                                    result,
                                    group,
                                    dayType,
                                    weekContent.Key,
                                    currentPairNum,
                                    scheduleDayElement.Value.Trim()
                                );
                            scheduleItems.AddRange(parsedScheduleItems);
                        }
                    }
                }
            }

            result.ScheduleItems = scheduleItems.ToArray();
            result.StudentGroups = groups.ToArray();

            return result;
        }

        /// <summary>
        /// Загружает расписание из файла.
        /// </summary>
        /// <param name="filePath">Путь до файла.</param>
        /// <see cref="SaveToFile(string)"/>
        /// <returns>Экземпляр расписания.</returns>
        public static Schedule LoadFromFile(string filePath)
        {
            var rawData = System.IO.File.ReadAllText(filePath);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Schedule>(rawData);

            // fix links
            foreach (var scheduleItem in result.ScheduleItems)
            {
                var realGroup = result.StudentGroups.Single(i => i.Name == scheduleItem.StudentGroup.Name && i.ScheduleUrl == scheduleItem.StudentGroup.ScheduleUrl);
                scheduleItem.StudentGroup = realGroup;
                scheduleItem.Schedule = result;
            }
            foreach (var studentGroup in result.StudentGroups)
            {
                studentGroup.Schedule = result;
            }

            return result;
        }

        /// <summary>
        /// Сохраняет данные расписания в файл.
        /// Если файл уже существует, он будет перезаписан.
        /// </summary>
        /// <param name="filePath">Путь до файла.</param>
        public void SaveToFile(string filePath)
        {
            var rawData = Newtonsoft.Json.JsonConvert.SerializeObject(
                this,
                Newtonsoft.Json.Formatting.Indented
            );
            System.IO.File.WriteAllText(filePath, rawData);
        }
    }
}
