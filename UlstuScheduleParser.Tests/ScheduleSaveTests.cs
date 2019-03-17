using NUnit.Framework;
using System;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace UlstuScheduleParser.Tests
{
    public class ScheduleSaveTests
    {
        private Schedule schedule;

        [SetUp]
        public void Setup()
        {
            schedule = Schedule.LoadFromFile("Data/schedule20190316.json");
        }

        [Test]
        public void StringComparing()
        {
            var source = System.IO.File.ReadAllText("Data/schedule20190316.json");
            schedule.SaveToFile("Data/schedule20190316.json.user");
            var result = System.IO.File.ReadAllText("Data/schedule20190316.json.user");
            Assert.AreEqual(source, result);
        }
    }
}