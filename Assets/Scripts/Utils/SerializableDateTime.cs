using System;
using UnityEngine;

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
