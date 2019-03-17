using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    public class UnknownHtmlWeekException : ScheduleParseException
    {
        public UnknownHtmlWeekException() : base("Не удалось прочитать неделю расписания.") { }
    }
}
