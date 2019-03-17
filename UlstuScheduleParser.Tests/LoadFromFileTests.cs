using NUnit.Framework;
using System;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace UlstuScheduleParser.Tests
{
    public class LoadFromFileTests
    {
        private Schedule schedule;

        [SetUp]
        public void Setup()
        {
            schedule = Schedule.LoadFromFile("Data/schedule20190316.json");
        }

        [Test]
        public void Base()
        {
            Assert.NotZero(schedule.StudentGroups.Length);
            Assert.NotZero(schedule.ScheduleItems.Length);
        }

        [Test]
        public void ScheduleItems_LinksToGroups()
        {
            foreach (var scheduleItem in schedule.ScheduleItems)
            {
                var group = schedule.StudentGroups.Single(i => i.Name == scheduleItem.StudentGroup.Name && i.ScheduleUrl == scheduleItem.StudentGroup.ScheduleUrl);
                Assert.AreEqual(group, scheduleItem.StudentGroup);
            }
        }

        [Test]
        public void ScheduleItems_LinksToSchedule()
        {
            foreach (var scheduleItem in schedule.ScheduleItems)
            {
                Assert.AreEqual(schedule, scheduleItem.Schedule);
            }
        }

        [Test]
        public void StudentGroups_LinksToSchedule()
        {
            foreach (var studentGroup in schedule.StudentGroups)
            {
                Assert.AreEqual(schedule, studentGroup.Schedule);
            }
        }

        [Test]
        public void StudentGroups_HasScheduleItems()
        {
            foreach (var studentGroup in schedule.StudentGroups)
            {
                var scheduleItemsCount = schedule.ScheduleItems.Count(i => i.StudentGroup == studentGroup);
                Assert.AreEqual(scheduleItemsCount, studentGroup.ScheduleItems.Count());
            }
        }
    }
}