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
            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            var schedule = await Schedule.LoadFromWebSiteAsync();
            var auditories = new[] { "3-317", "3-320", "3-321", "3-324", "3-325" };
            foreach (var auditory in auditories)
            {
                var html = Helpers.HtmlGenerator.GetAuditorySchedule(schedule, auditory);
                System.IO.File.WriteAllText($"aud_{auditory}.html", html);
            }
        }
    }
}
