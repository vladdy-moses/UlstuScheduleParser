using System;
using System.Linq;

namespace UlstuScheduleParser.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var st = Lib.Models.Schedule.LoadFromWebSite();
            st.Wait();
            var s = st.Result;
            s.SaveToFile("D:\\schedule20190316.json");

            var ss = Lib.Models.Schedule.LoadFromFile("text.json");

            var aud = "3-317";
            var res = Helpers.HtmlGenerator.GetAuditorySchedule(ss, aud);
            System.IO.File.WriteAllText("D:\\test.html", res);
            
        }
    }
}
