using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при неожиданных данных в ячейке расписания.
    /// </summary>
    public sealed class UnexpectedPairStringException : ScheduleParseException
    {
        public UnexpectedPairStringException() : base("Неожиданные данные в ячейке с расписанием.") { }
    }
}
