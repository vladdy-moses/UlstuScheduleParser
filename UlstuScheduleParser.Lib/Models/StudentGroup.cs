using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UlstuScheduleParser.Lib.Models
{
    /// <summary>
    /// Учебная группа.
    /// </summary>
    public sealed class StudentGroup
    {
        /// <summary>
        /// Ссылка на расписание.
        /// </summary>
        [JsonIgnore]
        public Schedule Schedule { get; set; }

        /// <summary>
        /// Название группы. Например, ИСТбд-12.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на расписание группы на веб-сайте УлГТУ.
        /// </summary>
        public string ScheduleUrl { get; set; }

        /// <summary>
        /// Элементы расписания текущей учебной группы при их наличии в расписании.
        /// </summary>
        [JsonIgnore]
        public ScheduleItem[] ScheduleItems => this.Schedule?.ScheduleItems?.Where(i => i.StudentGroup == this)?.ToArray();

        public override string ToString()
        {
            return Name + ", " + ScheduleUrl;
        }

        /// <summary>
        /// Парсинг списка учебных групп с веб-сайта.
        /// </summary>
        /// <param name="schedule">Экземпляр расписания.</param>
        /// <param name="scheduleHref">Ссылка на страницу со списком учебных групп.</param>
        /// <param name="scheduleData">Контент страницы со списком учебных групп.</param>
        /// <returns>Массив учебных групп.</returns>
        public static StudentGroup[] ParseFromScheduleData(Schedule schedule, string scheduleHref, byte[] scheduleData)
        {
            var scheduleContent = Encoding.GetEncoding(1251).GetString(scheduleData);
            var groupHrefRegex = new Regex(@"HREF=""(?'url'[^\""]*)"".*Roman"">(?'name'.+)<\/FONT>");
            var groups = new List<StudentGroup>();

            var groupHrefMatches = groupHrefRegex.Matches(scheduleContent);
            foreach (Match groupHrefMatch in groupHrefMatches)
            {
                var groupName = groupHrefMatch.Groups["name"].Value;
                var groupHref = groupHrefMatch.Groups["url"].Value;
                groupHref = scheduleHref.Substring(0, scheduleHref.LastIndexOf('/') + 1) + groupHref;
                groups.Add(new StudentGroup()
                {
                    Schedule = schedule,
                    Name = groupName,
                    ScheduleUrl = groupHref
                });
            }

            return groups.ToArray();
        }
    }
}
