using System;
using System.Globalization;
using UnityEngine;

namespace MagmaLabs.Utilities{
[Serializable]
public struct SerializableDateTime
{
    [SerializeField] private int year;
    [SerializeField] private int month;
    [SerializeField] private int day;
    [SerializeField] private int hour;
    [SerializeField] private int minute;
    [SerializeField] private int second;

    public SerializableDateTime(DateTime dt)
    {
        dt = dt.ToUniversalTime();
        year = dt.Year;
        month = dt.Month;
        day = dt.Day;
        hour = dt.Hour;
        minute = dt.Minute;
        second = dt.Second;
    }

    public SerializableDateTime(string s)
    {
        // Expected format: "yyyy-MM-dd HH:mm:ss UTC" (from ToString)
        DateTime dt;
        s = (s ?? string.Empty).Trim();
        // Try exact parse matching the ToString format
        if (DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt)
            || DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt)
            || DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt))
        {
            dt = dt.ToUniversalTime();
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            hour = dt.Hour;
            minute = dt.Minute;
            second = dt.Second;
        }
        else
        {
            // fallback to now (UTC)
            dt = DateTime.UtcNow;
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            hour = dt.Hour;
            minute = dt.Minute;
            second = dt.Second;
        }
    }

    public DateTime ToDateTimeUtc()
    {
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
    }

    public void FromDateTimeUtc(DateTime dt)
    {
        dt = dt.ToUniversalTime();
        year = dt.Year;
        month = dt.Month;
        day = dt.Day;
        hour = dt.Hour;
        minute = dt.Minute;
        second = dt.Second;
    }

    public override string ToString()
    {
        return string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2} UTC", year, month, day, hour, minute, second);
    }
}

}
