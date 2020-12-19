using System;

namespace HW_ATS
{
    static class  RandomDateTime
        {
            private static DateTime start;
            private static Random gen;
            private static int range;
            
            public static DateTime Next()
            {
                start = new DateTime(2020, 11, 19);
                gen = new Random();
                range = (DateTime.Today - start).Days;

                return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
            }

        }
}