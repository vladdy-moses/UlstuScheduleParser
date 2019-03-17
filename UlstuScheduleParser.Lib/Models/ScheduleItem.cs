using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UlstuScheduleParser.Lib.Exceptions;

namespace UlstuScheduleParser.Lib.Models
{
    public class ScheduleItem
    {
        [JsonIgnore]
        public Schedule Schedule { get; set; }
        public StudentGroup StudentGroup { get; set; }
        public ScheduleWeekType WeekType { get; set; }
        public ScheduleWeekDay WeekDay { get; set; }
        public int PairNum { get; set; }
        public string RawData { get; set; }
        public string Discipline { get; set; }
        public string Teacher { get; set; }
        public string Auditory { get; set; }

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
            var scheduleItemElements = scheduleItemRaw.Split('|', StringSplitOptions.RemoveEmptyEntries);
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
