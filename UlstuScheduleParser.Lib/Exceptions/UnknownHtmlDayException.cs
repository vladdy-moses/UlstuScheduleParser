using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    public class UnknownHtmlDayException : ScheduleParseException
    {
        public UnknownHtmlDayException() : base("Не удалось прочитать день расписания.") { }
    }
}
