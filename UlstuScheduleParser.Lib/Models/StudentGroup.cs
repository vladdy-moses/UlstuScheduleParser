using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public static StudentGroup[] ParseFromScheduleData(Schedule schedule, string scheduleHref, string scheduleContent)
        {
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
