using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    public class NoStudentGroupException : ScheduleParseException
    {
        public NoStudentGroupException() : base("Не найдено ни одной учебной группы в расписании.") { }
    }
}
