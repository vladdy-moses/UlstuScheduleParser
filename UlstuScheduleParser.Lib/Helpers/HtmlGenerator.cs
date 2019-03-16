using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UlstuScheduleParser.Lib.Helpers
{
    public static class HtmlGenerator
    {
        public static string GetAuditorySchedule(Models.Schedule schedule, string auditory)
        {
            var scheduleItems = schedule.ScheduleItems.Where(i => i.Auditory == auditory);
            var template = System.IO.File.ReadAllText("Templates/AuditoryScheduleTemplate.html");

            template = template.Replace("{AuditoryNumber}", auditory);

            foreach (Models.ScheduleWeekType scheduleWeekType in Enum.GetValues(typeof(Models.ScheduleWeekType)))
            {
                var result = new StringBuilder();
                var weekNum = (int)scheduleWeekType;

                result.Append("<table class=\"table table-sm table-bordered table-hover table-striped\">");
                result.Append("<thead class=\"thead-light\"><tr>");
                for (int i = 0; i <= 8; i++)
                {
                    result.Append("<th>").Append(i == 0 ? "Пары" : i.ToString()).Append("</th>");
                }
                result.Append("</tr></thead>");
                for (int weekDay = 1; weekDay <= 6; weekDay++)
                {
                    var scheduleWeekDay = (Models.ScheduleWeekDay)weekDay;
                    result.Append("<tr>");
                    result.Append("<th>").Append(scheduleWeekDay.ToString()).Append("</th>");
                    for (int pairNum = 1; pairNum <= 8; pairNum++)
                    {
                        result.Append("<td>");

                        var currentScheduleItems = scheduleItems.Where(i => i.WeekType == scheduleWeekType && i.WeekDay == scheduleWeekDay && i.PairNum == pairNum);
                        if (currentScheduleItems.Any())
                        {
                            var scheduleItemData = currentScheduleItems.GroupBy(i => new
                            {
                                i.Discipline,
                                i.Teacher
                            },
                            (i, j) => new
                            {
                                i.Discipline,
                                i.Teacher,
                                Groups = string.Join(", ", j.Select(k => k.StudentGroup.Name).Distinct())
                            });
                            var tdData = string.Join("<hr />", scheduleItemData.Select(i => i.Discipline + "<br /><small><em>" + i.Teacher + " (" + i.Groups + ")</em></small>"));
                            result.Append(tdData);
                        }
                        else
                        {
                            result.Append("-");
                        }

                        result.Append("</td>");
                    }
                    result.Append("</tr>");
                }
                result.Append("</table>");

                template = template.Replace("{Week" + weekNum + "}", result.ToString());
            }

            return template;
        }
    }
}
