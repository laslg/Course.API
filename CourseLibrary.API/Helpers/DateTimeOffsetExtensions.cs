using System;

namespace CourseLibrary.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var curDate = DateTime.UtcNow;
            int age = curDate.Year - dateTimeOffset.Year;

            if (curDate < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
