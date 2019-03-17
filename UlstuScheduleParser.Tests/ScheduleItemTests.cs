using NUnit.Framework;
using System;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace UlstuScheduleParser.Tests
{
    public class ScheduleItemTests
    {
        [Test(Description = "Обыкновенная запись пары")]
        [TestCase("лек.Законы мышления в практике PR и рекламы |Гоношилина И Г 6-720 |")]
        [TestCase("лек.Законы мышления в практике PR и рекламы|Гоношилина И Г 6-720|")]
        [TestCase("лек.Законы мышления в практике PR и рекламы | Гоношилина И Г 6-720 ||")]
        [TestCase("лек.Законы мышления в практике PR и рекламы | Гоношилина И Г 6-720")]
        public void Single(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.Понедельник, ScheduleWeekType.Первая, 1, rawData);
            Assert.AreEqual(1, scheduleItems.Length);
            Assert.AreEqual("лек.Законы мышления в практике PR и рекламы", scheduleItems[0].Discipline);
            Assert.AreEqual("Гоношилина И Г", scheduleItems[0].Teacher);
            Assert.AreEqual("6-720", scheduleItems[0].Auditory);
        }

        [Test(Description = "Две подгруппы")]
        [TestCase("лекция|Препод1 5-555|практика|Препод2 6-666")]
        public void DoublePairs(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.Понедельник, ScheduleWeekType.Первая, 1, rawData);
            Assert.AreEqual(2, scheduleItems.Length);
            var lecture = scheduleItems.Single(i => i.Discipline == "лекция");
            Assert.AreEqual("5-555", lecture.Auditory);
            Assert.AreEqual("Препод1", lecture.Teacher);
            var practice = scheduleItems.Single(i => i.Discipline == "практика");
            Assert.AreEqual("6-666", practice.Auditory);
            Assert.AreEqual("Препод2", practice.Teacher);
        }

        [Test(Description = "Три преподавателя")]
        [TestCase("лекция | Препод1 4-444| Препод2 5-555 |Препод3 6-666")]
        public void TripleTeachers(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.Понедельник, ScheduleWeekType.Первая, 1, rawData);
            Assert.AreEqual(3, scheduleItems.Length);
            var item1 = scheduleItems.Single(i => i.Teacher == "Препод1");
            Assert.AreEqual("лекция", item1.Discipline);
            Assert.AreEqual("4-444", item1.Auditory);
            var item2 = scheduleItems.Single(i => i.Teacher == "Препод2");
            Assert.AreEqual("лекция", item2.Discipline);
            Assert.AreEqual("5-555", item2.Auditory);
            var item3 = scheduleItems.Single(i => i.Teacher == "Препод3");
            Assert.AreEqual("лекция", item3.Discipline);
            Assert.AreEqual("6-666", item3.Auditory);
        }

        [Test(Description = "Без преподавателей")]
        [TestCase("лекция|  6-Каф")]
        public void WithoutTeachers(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.Понедельник, ScheduleWeekType.Первая, 1, rawData);
            Assert.AreEqual(1, scheduleItems.Length);
            Assert.IsEmpty(scheduleItems[0].Teacher);
            Assert.AreEqual("6-Каф", scheduleItems[0].Auditory);
        }

        [Test(Description = "Несколько преподов, несколько подгрупп")]
        [TestCase("лекция|Препод1 6-666|6-666|практика 123|3-317")]
        public void Complex(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.Понедельник, ScheduleWeekType.Первая, 1, rawData);
            Assert.AreEqual(3, scheduleItems.Length);
            Assert.AreEqual(2, scheduleItems.Count(i => i.Discipline == "лекция"));
            Assert.AreEqual(1, scheduleItems.Count(i => i.Discipline == "практика 123"));
            Assert.AreEqual(2, scheduleItems.Count(i => string.IsNullOrEmpty(i.Teacher)));
            Assert.AreEqual(2, scheduleItems.Count(i => i.Auditory == "6-666"));
        }

        [Test]
        [TestCase(false, false, Description = "Без группы, без преподавателя")]
        [TestCase(false, true, Description = "Без группы, с преподавателем")]
        [TestCase(true, false, Description = "С группой, без преподавателя")]
        [TestCase(true, true, Description = "С группой, с преподавателем")]
        public void ToStringTest_WithoutGroup(bool withGroup, bool withDiscipline)
        {
            var scheduleItem = new ScheduleItem()
            {
                Teacher = "Иванов И.И.",
                Auditory = "3-319",
                WeekType = ScheduleWeekType.Первая,
                WeekDay = ScheduleWeekDay.Понедельник,
                PairNum = 4
            };
            if (withGroup)
            {
                scheduleItem.StudentGroup = new StudentGroup()
                {
                    Name = "ТЕСТбд-11",
                    ScheduleUrl = "http://ulstu.ru/"
                };
            }
            if (withDiscipline)
            {
                scheduleItem.Discipline = "пр. НИР";
            }

            var ts = scheduleItem.ToString();

            Assert.IsTrue(ts.Contains(scheduleItem.WeekDay.ToString()));
            Assert.IsTrue(ts.Contains(scheduleItem.PairNum.ToString()));
            if (withGroup)
            {
                Assert.IsTrue(ts.Contains(scheduleItem.StudentGroup.Name));
            }
            if (withDiscipline)
            {
                Assert.IsTrue(ts.Contains(scheduleItem.Discipline));
            }
        }
    }
}