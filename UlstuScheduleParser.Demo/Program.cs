using System;
using System.Linq;
using System.Threading.Tasks;
using UlstuScheduleParser.Lib.Models;

namespace UlstuScheduleParser.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute(args).Wait();
        }

        static async Task Execute(string[] args)
        {
            // Получение расписания с веб-сайта.
            var schedule = await Schedule.LoadFromWebSiteAsync();

            // Примеры использования библиотеки.
            GenerateAutorySchedule(schedule);
            GetHardestDay(schedule);

            // Конец примера
            Console.ReadKey();
        }

        /// <summary>
        /// Генерирует расписание аудиторий из расписания студентов.
        /// </summary>
        /// <param name="schedule">Экземпляр расписания.</param>
        static void GenerateAutorySchedule(Schedule schedule)
        {
            Console.WriteLine();
            Console.WriteLine("= Генерация расписания аудиторий =");
            Console.WriteLine($"Текущая директория: {Environment.CurrentDirectory}");
            var auditories = new[] { "3-317", "3-320", "3-321", "3-324", "3-325" };
            foreach (var auditory in auditories)
            {
                var html = Helpers.HtmlGenerator.GetAuditorySchedule(schedule, auditory);
                System.IO.File.WriteAllText($"aud_{auditory}.html", html);
            }
        }

        /// <summary>
        /// Поиск групп, в которых больше всего пар в день по всему университету.
        /// Кстати, на момент написания теста таких групп нашлось две, а пар в день - 6.
        /// </summary>
        /// <param name="schedule">Экземпляр расписания.</param>
        static void GetHardestDay(Schedule schedule)
        {
            Console.WriteLine();
            Console.WriteLine("= Поиск группы и дня с самым большим числом пар =");

            var q = (
                from si in schedule.ScheduleItems
                group si by new { si.StudentGroup, si.WeekType, si.WeekDay } into g
                select new
                {
                    g.Key.StudentGroup,
                    g.Key.WeekType,
                    g.Key.WeekDay,
                    Pairs = g.Select(i => i.PairNum).Distinct().Count()
                }
                ).ToArray();
            var maxPairs = q.OrderByDescending(i => i.Pairs).Select(i => i.Pairs).First();
            var result = string.Join("\r\n\t", q.Where(i => i.Pairs == maxPairs).Select(i => i.StudentGroup.Name + " - " + i.WeekType.ToString() + " - " + i.WeekDay.ToString()));
            Console.WriteLine($"Больше всего пар - {maxPairs}:\n\t{result}");
        }
    }
}
