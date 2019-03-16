using NUnit.Framework;
using System;
using System.Linq;
using UlstuScheduleParser.Lib.Models;

namespace Tests
{
    public class ScheduleItemTests
    {
        [Test(Description = "������������ ������ ����")]
        [TestCase("���.������ �������� � �������� PR � ������� |���������� � � 6-720 |")]
        [TestCase("���.������ �������� � �������� PR � �������|���������� � � 6-720|")]
        [TestCase("���.������ �������� � �������� PR � ������� | ���������� � � 6-720 ||")]
        [TestCase("���.������ �������� � �������� PR � ������� | ���������� � � 6-720")]
        public void Single(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.�����������, ScheduleWeekType.������, 1, rawData);
            Assert.AreEqual(1, scheduleItems.Length);
            Assert.AreEqual("���.������ �������� � �������� PR � �������", scheduleItems[0].Discipline);
            Assert.AreEqual("���������� � �", scheduleItems[0].Teacher);
            Assert.AreEqual("6-720", scheduleItems[0].Auditory);
            Assert.Pass();
        }

        [Test(Description = "��� ���������")]
        [TestCase("������|������1 5-555|��������|������2 6-666")]
        public void DoublePairs(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.�����������, ScheduleWeekType.������, 1, rawData);
            Assert.AreEqual(2, scheduleItems.Length);
            var lecture = scheduleItems.Single(i => i.Discipline == "������");
            Assert.AreEqual("5-555", lecture.Auditory);
            Assert.AreEqual("������1", lecture.Teacher);
            var practice = scheduleItems.Single(i => i.Discipline == "��������");
            Assert.AreEqual("6-666", practice.Auditory);
            Assert.AreEqual("������2", practice.Teacher);
            Assert.Pass();
        }

        [Test(Description = "��� �������������")]
        [TestCase("������ | ������1 4-444| ������2 5-555 |������3 6-666")]
        public void TripleTeachers(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.�����������, ScheduleWeekType.������, 1, rawData);
            Assert.AreEqual(3, scheduleItems.Length);
            var item1 = scheduleItems.Single(i => i.Teacher == "������1");
            Assert.AreEqual("������", item1.Discipline);
            Assert.AreEqual("4-444", item1.Auditory);
            var item2 = scheduleItems.Single(i => i.Teacher == "������2");
            Assert.AreEqual("������", item2.Discipline);
            Assert.AreEqual("5-555", item2.Auditory);
            var item3 = scheduleItems.Single(i => i.Teacher == "������3");
            Assert.AreEqual("������", item3.Discipline);
            Assert.AreEqual("6-666", item3.Auditory);
            Assert.Pass();
        }

        [Test(Description = "��� ��������������")]
        [TestCase("������|  6-���")]
        public void WithoutTeachers(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.�����������, ScheduleWeekType.������, 1, rawData);
            Assert.AreEqual(1, scheduleItems.Length);
            Assert.IsEmpty(scheduleItems[0].Teacher);
            Assert.AreEqual("6-���", scheduleItems[0].Auditory);
            Assert.Pass();
        }

        [Test(Description = "��������� ��������, ��������� ��������")]
        [TestCase("������|������1 6-666|6-666|�������� 123|3-317")]
        public void Complex(string rawData)
        {
            var scheduleItems = ScheduleItem.ParseFromRawData(null, null, ScheduleWeekDay.�����������, ScheduleWeekType.������, 1, rawData);
            Assert.AreEqual(3, scheduleItems.Length);
            Assert.AreEqual(2, scheduleItems.Count(i => i.Discipline == "������"));
            Assert.AreEqual(1, scheduleItems.Count(i => i.Discipline == "�������� 123"));
            Assert.AreEqual(2, scheduleItems.Count(i => string.IsNullOrEmpty(i.Teacher)));
            Assert.AreEqual(2, scheduleItems.Count(i => i.Auditory == "6-666"));
            Assert.Pass();
        }
    }
}