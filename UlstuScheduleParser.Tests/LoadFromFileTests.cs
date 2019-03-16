using NUnit.Framework;
using System;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace Tests
{
    public class LoadFromFileTests
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        [Test]
        public void Base()
        {
            var schedule = Schedule.LoadFromFile("Data/schedule20190316.json");
            Assert.NotZero(schedule.StudentGroups.Length);
            Assert.NotZero(schedule.ScheduleItems.Length);
            Assert.Pass();
        }

        [Test]
        public void ScheduleItems_LinksToGroups()
        {
            var schedule = Schedule.LoadFromFile("Data/schedule20190316.json");

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
            var schedule = Schedule.LoadFromFile("Data/schedule20190316.json");

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
            var schedule = Schedule.LoadFromFile("Data/schedule20190316.json");

            foreach (var studentGroup in schedule.StudentGroups)
            {
                if (studentGroup.Schedule == null || studentGroup.Schedule != schedule)
                    throw new Exception("Link from StudentGroup to Schedule is wrong");
            }

            Assert.Pass();
        }
    }
}