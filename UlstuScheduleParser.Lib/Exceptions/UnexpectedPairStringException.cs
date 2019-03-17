using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    public class UnexpectedPairStringException : ScheduleParseException
    {
        public UnexpectedPairStringException() : base("Неожиданные данные в ячейке с расписанием.") { }
    }
}
