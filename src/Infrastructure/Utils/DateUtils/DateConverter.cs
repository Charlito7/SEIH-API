namespace Infrastructure.Utils.DateUtils;

public static class DateConverter
{
    public static DateTime TodayHaitiToUTC(int hour, int minute = 0)
    {
        TimeZoneInfo haitiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Port-au-Prince");

        DateTime todayLocal = DateTime.Today;
        todayLocal = todayLocal.AddDays(-1);

        DateTime haitiTime = todayLocal.AddHours(hour).AddMinutes(minute);

        if (hour < 0 || hour > 23)
            throw new ArgumentOutOfRangeException(nameof(hour), "L'heure doit être entre 0 et 23");

        if (minute < 0 || minute > 59)
            throw new ArgumentOutOfRangeException(nameof(minute), "Les minutes doivent être entre 0 et 59");

        haitiTime = DateTime.SpecifyKind(haitiTime, DateTimeKind.Unspecified);

        DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(haitiTime, haitiTimeZone);

        return utcTime;
    }
}
