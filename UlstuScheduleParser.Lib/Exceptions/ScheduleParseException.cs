using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    /// <summary>
    /// Базовый класс для обозначения исключений парсинга расписания с сайта УлГТУ.
    /// </summary>
    public abstract class ScheduleParseException : Exception
    {
        public ScheduleParseException(string message) : base(message) { }
    }
}
