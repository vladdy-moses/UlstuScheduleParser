using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace UlstuScheduleParser.Tests
{
    public class ScheduleLoadTests
    {
        private static Schedule scheduleFile;
        private static Schedule scheduleWeb;
        private static object _lockObj = new object();

        private static IEnumerable<Schedule> GetSchedules()
        {
            lock (_lockObj)
            {
                if (scheduleFile == null)
                {
                    scheduleFile = Schedule.LoadFromFile("Data/schedule20190316.json");
                }
                if (scheduleWeb == null)
                {
                    var scheduleWebTask = Schedule.LoadFromWebSiteAsync();
                    scheduleWebTask.Wait();
                    scheduleWeb = scheduleWebTask.Result;
                }
            }

            yield return scheduleFile;
            yield return scheduleWeb;
        }

        [Test]
        [TestCaseSource(nameof(GetSchedules))]
        public void Base(Schedule schedule)
        {
            Assert.NotZero(schedule.StudentGroups.Length);
            Assert.NotZero(schedule.ScheduleItems.Length);
        }

        [Test]
        [TestCaseSource(nameof(GetSchedules))]
        public void ScheduleItems_LinksToGroups(Schedule schedule)
        {
            foreach (var scheduleItem in schedule.ScheduleItems)
            {
                var group = schedule.StudentGroups.Single(i => i.Name == scheduleItem.StudentGroup.Name && i.ScheduleUrl == scheduleItem.StudentGroup.ScheduleUrl);
                Assert.AreEqual(group, scheduleItem.StudentGroup);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetSchedules))]
        public void ScheduleItems_LinksToSchedule(Schedule schedule)
        {
            foreach (var scheduleItem in schedule.ScheduleItems)
            {
                Assert.AreEqual(schedule, scheduleItem.Schedule);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetSchedules))]
        public void StudentGroups_LinksToSchedule(Schedule schedule)
        {
            foreach (var studentGroup in schedule.StudentGroups)
            {
                Assert.AreEqual(schedule, studentGroup.Schedule);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetSchedules))]
        public void StudentGroups_HasScheduleItems(Schedule schedule)
        {
            foreach (var studentGroup in schedule.StudentGroups)
            {
                var scheduleItemsCount = schedule.ScheduleItems.Count(i => i.StudentGroup == studentGroup);
                Assert.AreEqual(scheduleItemsCount, studentGroup.ScheduleItems.Count());
            }
        }
    }
}