using System;
using System.Collections.Generic;
using System.Text;

namespace UlstuScheduleParser.Lib.Models
{
    /// <summary>
    /// День недели.
    /// Воскресенья нет, потому что в этот день нет пар.
    /// </summary>
    public enum ScheduleWeekDay
    {
        Понедельник = 1,
        Вторник = 2,
        Среда = 3,
        Четверг = 4,
        Пятница = 5,
        Суббота = 6
    }
}
