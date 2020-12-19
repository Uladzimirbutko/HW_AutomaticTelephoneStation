using System;

namespace HW_ATS
{
    static class  RandomDateTime
        {
            private static DateTime start = new DateTime(2020, 11, 19);
            private static Random gen = new Random();
            private static int range = (DateTime.Today - start).Days;

        public static DateTime Next()
            {
                return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
            }
        }
}