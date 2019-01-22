using System;

namespace PeopleSearch.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int CalculateAge(this DateTimeOffset dateTimeOffset)
        {
            var now = DateTime.UtcNow;
            var age = now.Year - dateTimeOffset.Year;
            if (now < dateTimeOffset.AddYears(age)) age--;

            return age;
        }
    }
}