using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при отсутствии групп при парсинге расписания с сайта УлГТУ.
    /// </summary>
    public class NoStudentGroupException : ScheduleParseException
    {
        public NoStudentGroupException() : base("Не найдено ни одной учебной группы в расписании.") { }
    }
}
