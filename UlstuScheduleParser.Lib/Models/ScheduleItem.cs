using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
