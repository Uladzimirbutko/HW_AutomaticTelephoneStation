using System;

namespace HW_ATS
{
    static class  RandomDateTime
        {
            private static DateTime start = new DateTime(2020, 11, 19);
            private static int range = (DateTime.Today - start).Days;

            public static DateTime Next()
            {
                return start.AddDays(new Random().Next(range))
                    .AddHours(new Random().Next(1,24))
                    .AddMinutes(new Random().Next(0, 60))
                    .AddSeconds(new Random().Next(0, 60));
            }
        }
}