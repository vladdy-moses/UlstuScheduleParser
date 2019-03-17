using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UlstuScheduleParser.Lib.Models;

namespace UlstuScheduleParser.Tests
{
    public class StudentGroupTests
    {
        [Test]
        public void ParseGroups()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var scheduleData = System.IO.File.ReadAllBytes("Data/raspisan.html");

            var groups = StudentGroup.ParseFromScheduleData(null, "http://www.ulstu.ru/schedule/students/part1/raspisan.htm", scheduleData);

            Assert.AreEqual(90, groups.Length);
            Assert.IsNotNull(groups.SingleOrDefault(i => i.Name == "ИСТбд-11"));
            Assert.Pass();
        }
        
        [Test]
        public void ToStringTest()
        {
            var group = new StudentGroup()
            {
                Name = "ТЕСТбд-12",
                ScheduleUrl = "http://ulstu.ru"
            };

            Assert.IsTrue(group.ToString().Contains(group.Name));
            Assert.IsTrue(group.ToString().Contains(group.ScheduleUrl));
            Assert.Pass();
        }
    }
}
