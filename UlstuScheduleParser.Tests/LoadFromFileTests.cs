using NUnit.Framework;
using System;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace Tests
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
            Assert.Pass();
        }

        [Test]
        public void ScheduleItems_LinksToGroups()
        {
            foreach (var scheduleItem in schedule.ScheduleItems)
            {
                var group = schedule.StudentGroups.Single(i => i.Name == scheduleItem.StudentGroup.Name && i.ScheduleUrl == scheduleItem.StudentGroup.ScheduleUrl);
                if (group != scheduleItem.StudentGroup)
                    throw new Exception("Link from ScheduleItem to StudentGroup is wrong");
            }

            Assert.Pass();
        }

        [Test]
        public void ScheduleItems_LinksToSchedule()
        {
            foreach (var scheduleItem in schedule.ScheduleItems)
            {
                if (scheduleItem.Schedule == null || scheduleItem.Schedule != schedule)
                    throw new Exception("Link from ScheduleItem to Schedule is wrong");
            }

            Assert.Pass();
        }

        [Test]
        public void StudentGroups_LinksToSchedule()
        {
            foreach (var studentGroup in schedule.StudentGroups)
            {
                if (studentGroup.Schedule == null || studentGroup.Schedule != schedule)
                    throw new Exception("Link from StudentGroup to Schedule is wrong");
            }

            Assert.Pass();
        }

        [Test]
        public void StudentGroups_HasScheduleItems()
        {
            foreach (var studentGroup in schedule.StudentGroups)
            {
                var scheduleItemsCount = schedule.ScheduleItems.Count(i => i.StudentGroup == studentGroup);
                Assert.AreEqual(scheduleItemsCount, studentGroup.ScheduleItems.Count());
            }

            Assert.Pass();
        }
    }
}