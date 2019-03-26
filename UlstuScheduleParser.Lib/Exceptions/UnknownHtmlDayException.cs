using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при неожиданной HTML-разметке дня расписания в учебной группе.
    /// </summary>
    public sealed class UnknownHtmlDayException : ScheduleParseException
    {
        public UnknownHtmlDayException() : base("Не удалось прочитать день расписания.") { }
    }
}
