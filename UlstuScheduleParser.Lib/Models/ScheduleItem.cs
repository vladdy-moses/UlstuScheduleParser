using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UlstuScheduleParser.Lib.Exceptions;

namespace UlstuScheduleParser.Lib.Models
{
    /// <summary>
    /// Элемент расписания (пара).
    /// </summary>
    public class ScheduleItem
    {
        /// <summary>
        /// Ссылка на расписание.
        /// </summary>
        [JsonIgnore]
        public Schedule Schedule { get; set; }

        /// <summary>
        /// Учебная группа.
        /// </summary>
        public StudentGroup StudentGroup { get; set; }

        /// <summary>
        /// Тип недели. В УлГТУ существует несколько недель.
        /// </summary>
        public ScheduleWeekType WeekType { get; set; }

        /// <summary>
        /// День недели.
        /// </summary>
        public ScheduleWeekDay WeekDay { get; set; }

        /// <summary>
        /// Номер пары.
        /// </summary>
        public int PairNum { get; set; }

        /// <summary>
        /// Исходные данные с веб-сайта.
        /// Переводы строк (<br />) заменены символом "|".
        /// </summary>
        public string RawData { get; set; }

        /// <summary>
        /// Название дисциплины.
        /// </summary>
        public string Discipline { get; set; }

        /// <summary>
        /// ФИО преподавателя.
        /// </summary>
        public string Teacher { get; set; }

        /// <summary>
        /// Номер аудитории.
        /// </summary>
        public string Auditory { get; set; }

        /// <summary>
        /// Получение элементов расписания из ячейки.
        /// </summary>
        /// <param name="schedule">Экземпляр расписания.</param>
        /// <param name="studentGroup">Учебная группа.</param>
        /// <param name="scheduleWeekDay">День недели.</param>
        /// <param name="scheduleWeekType">Тип недели.</param>
        /// <param name="pairNum">Номер пары.</param>
        /// <param name="scheduleItemRaw">Подготовленные данные из ячейки расписниая.</param>
        /// <returns>Массив экземпляров расписания. Массив, потому что в одной ячейке может быть несколько элементов (разные подгруппы).</returns>
        public static ScheduleItem[] ParseFromRawData(Schedule schedule, StudentGroup studentGroup, ScheduleWeekDay scheduleWeekDay, ScheduleWeekType scheduleWeekType, int pairNum, string scheduleItemRaw)
        {
            var scheduleItemCleanupRegex = new Regex(@"\s*\|\s*");
            var auditoryRegex = new Regex(@"^([1-6]-|3_).*$");
            var scheduleItems = new List<ScheduleItem>();
            if (string.IsNullOrEmpty(scheduleItemRaw) || scheduleItemRaw == "_")
            {
                return scheduleItems.ToArray();
            }
            scheduleItemRaw = scheduleItemCleanupRegex.Replace(scheduleItemRaw, "|");
            var scheduleItemElements = scheduleItemRaw.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string currentDiscipline = null;
            bool lastItemWasDiscipline = false;
            foreach (var scheduleItemElement in scheduleItemElements)
            {
                var lastItemElement = scheduleItemElement.Split(' ').Last();
                if (auditoryRegex.IsMatch(lastItemElement))
                {
                    // second string
                    lastItemWasDiscipline = false;

                    string auditory = lastItemElement;
                    string teacher = "";
                    if (auditory.Length != scheduleItemElement.Length)
                    {
                        teacher = scheduleItemElement.Substring(0, scheduleItemElement.Length - auditory.Length - 1).Trim();
                    }

                    scheduleItems.Add(new ScheduleItem()
                    {
                        Schedule = schedule,
                        StudentGroup = studentGroup,
                        PairNum = pairNum,
                        WeekDay = scheduleWeekDay,
                        WeekType = scheduleWeekType,
                        RawData = scheduleItemRaw,
                        Discipline = currentDiscipline,
                        Auditory = auditory,
                        Teacher = teacher
                    });
                }
                else
                {
                    // first string
                    if (lastItemWasDiscipline)
                    {
                        throw new UnexpectedPairStringException();
                    }
                    currentDiscipline = scheduleItemElement;
                    lastItemWasDiscipline = true;
                }
            }
            if (lastItemWasDiscipline)
            {
                // TODO: fix this issue
                //throw new UnexpectedPairStringException();
            }
            return scheduleItems.ToArray();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (StudentGroup != null)
                sb.Append(StudentGroup.Name).Append(", ");

            sb.Append(WeekType).Append(", ");
            sb.Append(WeekDay).Append(", ");

            sb.Append(PairNum);

            if (!string.IsNullOrEmpty(Discipline))
                sb.Append(" ").Append(Discipline);

            return sb.ToString();
        }
    }
}
