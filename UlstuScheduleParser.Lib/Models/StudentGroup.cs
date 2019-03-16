using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UlstuScheduleParser.Lib.Models
{
    public class StudentGroup
    {
        [JsonIgnore]
        public Schedule Schedule { get; set; }
        public string Name { get; set; }
        public string ScheduleUrl { get; set; }

        [JsonIgnore]
        public ScheduleItem[] ScheduleItems => this.Schedule?.ScheduleItems?.Where(i => i.StudentGroup == this)?.ToArray();

        public override string ToString()
        {
            return Name + ", " + ScheduleUrl;
        }
    }
}
