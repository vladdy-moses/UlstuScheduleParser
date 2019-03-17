using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Exceptions
{
    public abstract class ScheduleParseException : Exception
    {
        public ScheduleParseException(string message) : base(message) { }
    }
}
