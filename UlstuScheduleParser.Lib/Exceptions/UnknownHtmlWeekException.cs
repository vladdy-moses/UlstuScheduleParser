using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при неожиданной HTML-разметке недели расписания в учебной группе.
    /// </summary>
    public class UnknownHtmlWeekException : ScheduleParseException
    {
        public UnknownHtmlWeekException() : base("Не удалось прочитать неделю расписания.") { }
    }
}
