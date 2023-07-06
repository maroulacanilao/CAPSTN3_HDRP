using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

[BurstCompile]
public struct CustomDateTime
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;
    
    public int CompareMyDateTime(CustomDateTime dateTime1, CustomDateTime dateTime2)
    {
        if (dateTime1.year > dateTime2.year)
            return 1;
        else if (dateTime1.year < dateTime2.year)
            return -1;

        if (dateTime1.month > dateTime2.month)
            return 1;
        else if (dateTime1.month < dateTime2.month)
            return -1;

        if (dateTime1.day > dateTime2.day)
            return 1;
        else if (dateTime1.day < dateTime2.day)
            return -1;

        if (dateTime1.hour > dateTime2.hour)
            return 1;
        else if (dateTime1.hour < dateTime2.hour)
            return -1;

        if (dateTime1.minute > dateTime2.minute)
            return 1;
        else if (dateTime1.minute < dateTime2.minute)
            return -1;

        if (dateTime1.second > dateTime2.second)
            return 1;
        else if (dateTime1.second < dateTime2.second)
            return -1;

        // If all fields are equal, return 0 (equal)
        return 0;
    }
    
    public CustomDateTime AddMinutes(int minutes)
    {
        int totalMinutes = (year * 365 * 24 * 60) + (month * 30 * 24 * 60) + (day * 24 * 60) + (hour * 60) + minute + minutes;

        CustomDateTime newDateTime = new CustomDateTime();
        newDateTime.year = totalMinutes / (365 * 24 * 60);
        newDateTime.month = (totalMinutes % (365 * 24 * 60)) / (30 * 24 * 60);
        newDateTime.day = ((totalMinutes % (365 * 24 * 60)) % (30 * 24 * 60)) / (24 * 60);
        newDateTime.hour = (((totalMinutes % (365 * 24 * 60)) % (30 * 24 * 60)) % (24 * 60)) / 60;
        newDateTime.minute = (((totalMinutes % (365 * 24 * 60)) % (30 * 24 * 60)) % (24 * 60)) % 60;
        newDateTime.second = second;

        return newDateTime;
    }
    
    public CustomDateTime AddMinutesRoundedToFive(int minutes)
    {
        int totalMinutes = (year * 365 * 24 * 60) + (month * 30 * 24 * 60) + (day * 24 * 60) + (hour * 60) + minute + minutes;

        int roundedMinutes = (int)(Math.Round(totalMinutes / 5.0) * 5);

        CustomDateTime newDateTime = new CustomDateTime();
        newDateTime.year = roundedMinutes / (365 * 24 * 60);
        newDateTime.month = (roundedMinutes % (365 * 24 * 60)) / (30 * 24 * 60);
        newDateTime.day = ((roundedMinutes % (365 * 24 * 60)) % (30 * 24 * 60)) / (24 * 60);
        newDateTime.hour = (((roundedMinutes % (365 * 24 * 60)) % (30 * 24 * 60)) % (24 * 60)) / 60;
        newDateTime.minute = (((roundedMinutes % (365 * 24 * 60)) % (30 * 24 * 60)) % (24 * 60)) % 60;
        newDateTime.second = second;

        return newDateTime;
    }
    
    public DateTime ToDateTime()
    {
        return new DateTime(year, month, day, hour, minute, second);
    }

    public static CustomDateTime FromDateTime(DateTime dateTime)
    {
        return new CustomDateTime
        {
            year = dateTime.Year,
            month = dateTime.Month,
            day = dateTime.Day,
            hour = dateTime.Hour,
            minute = dateTime.Minute,
            second = dateTime.Second
        };
    }
}

[BurstCompile]
public struct CustomTimeSpan
{
    public int days;
    public int hours;
    public int minutes;
    public int seconds;

    public static CustomTimeSpan Subtract(CustomDateTime dateTime1, CustomDateTime dateTime2)
    {
        int totalSeconds1 = CalculateTotalSeconds(dateTime1);
        int totalSeconds2 = CalculateTotalSeconds(dateTime2);

        int deltaSeconds = totalSeconds1 - totalSeconds2;
        
        CustomTimeSpan timeSpan;
        timeSpan.days = deltaSeconds / 86400;
        timeSpan.hours = (deltaSeconds % 86400) / 3600;
        timeSpan.minutes = (deltaSeconds % 3600) / 60;
        timeSpan.seconds = deltaSeconds % 60;
        return timeSpan;
    }

    private static int CalculateTotalSeconds(CustomDateTime dateTime)
    {
        int totalSeconds = 0;
        totalSeconds += dateTime.day * 86400;
        totalSeconds += dateTime.hour * 3600;
        totalSeconds += dateTime.minute * 60;
        totalSeconds += dateTime.second;
        return totalSeconds;
    }
    
    public int GetTotalMinutes()
    {
        int totalMinutes = (days * 24 * 60) + (hours * 60) + minutes;
        return totalMinutes;
    }
}



