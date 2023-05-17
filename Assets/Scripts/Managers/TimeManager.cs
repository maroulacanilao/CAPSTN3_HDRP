using System;
using BaseCore;
using CustomEvent;
using UnityEngine;

namespace Managers
{
    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class TimeManager : Singleton<TimeManager>
    {
        [Header("Time Values (24hr format)")]
        [SerializeField] private int endingHour = 24;
        [SerializeField] private int startingHour = 6;
        [SerializeField] private int nightHour = 19;
        [SerializeField] private float timeScale = 1; // 1 second in real time = 1 minute in game

        [Header("Events")]
        public static readonly Evt<TimeManager> OnMinuteTick = new Evt<TimeManager>();
        public static readonly Evt<TimeManager> OnHourTick = new Evt<TimeManager>();
        public static readonly Evt<TimeManager> OnEndDay = new Evt<TimeManager>();
        public static readonly Evt<TimeManager> OnBeginDay = new Evt<TimeManager>();
        public static readonly Evt<TimeManager> OnNewWeek = new Evt<TimeManager>();
        public static readonly Evt<TimeManager> OnNewMonth = new Evt<TimeManager>();
        public static readonly Evt<TimeManager, bool> OnPauseTime = new Evt<TimeManager, bool>();
        public static readonly Evt<TimeManager> OnNightTime = new Evt<TimeManager>();

        private bool isTimePaused = false;
        private int dayCounter = 0;
        private bool didDayStart = false;
        private DateTime dateTime;

        private float timer = 0;

        #region Getters and Setters

        public static bool DidDayStart
        {
            get => Instance.didDayStart;
            set => Instance.didDayStart = value;
        }

        public static int EndingHour
        {
            get => Instance.endingHour;
            set => Instance.endingHour = value;
        }

        public static int StartingHour
        {
            get => Instance.startingHour;
            set => Instance.startingHour = value;
        }
        
        public static int NightHour
        {
            get => Instance.nightHour;
            set => Instance.nightHour = value;
        }

        public static float TimeScale
        {
            get => Instance.timeScale;
            set => Instance.timeScale = value;
        }

        public static int DayCounter => Instance.dayCounter;
        public static bool IsWeekend => CurrentDay is DayOfWeek.Saturday or DayOfWeek.Sunday;
        public static float GameTime => CurrentHour + CurrentMinute / 60f;
        public static bool IsTimePaused => Instance.isTimePaused;
        public static int CurrentHour => DateTime.Hour;
        public static int CurrentMinute => DateTime.Minute;
        public static int CurrentYear => DateTime.Year;
        public static Month CurrentMonth => (Month) DateTime.Month;
        public static int CurrentDate => DateTime.Day;
        public static DayOfWeek CurrentDay => DateTime.DayOfWeek;
        public static DateTime DateTime => Instance.dateTime;

        #endregion

        private void Start()
        {
            dateTime = DateTime.Now;

            //reset hour and minutes to 0:00
            dateTime = dateTime.AddHours(-CurrentHour);
            dateTime = dateTime.AddMinutes(-CurrentMinute);

            // reset hour to starting hour
            dateTime = dateTime.AddHours(startingHour);

            OnBeginDay.Invoke(this);

            isTimePaused = true;
            
            //TODO: remove this
            StartDay();
        }

        private void Update()
        {
            // for testing
            if (isTimePaused) return;
            
            if (timer >= timeScale)
            {
                dateTime = dateTime.AddMinutes(1);
                OnMinuteTick.Invoke(this);
                timer = 0;
                if (CurrentMinute != 0) return;
                
                OnHourTick.Invoke(this);

                if (CurrentHour == nightHour)
                {
                    OnNightTime.Invoke(this);
                }
                
                if (CurrentHour >= endingHour)
                {
                    EndDay();
                    isTimePaused = true;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        private void EndDay()
        {
            isTimePaused = true;

            didDayStart = false;

            // for checking if game over and other visual things
            OnEndDay.Invoke(this);
        }
        
        // called when starting a day to update time
        [ContextMenu("Start Day")]
        public static void StartDay()
        {
            Instance.isTimePaused = false;

            var _prevDateTime = new DateTime();
            _prevDateTime = DateTime;

            Instance.dateTime = DateTime.AddDays(1);
            Instance.dayCounter++;


            //reset hour and minutes to 0:00
            Instance.dateTime = DateTime.AddHours(-CurrentHour);
            Instance.dateTime = DateTime.AddMinutes(-CurrentMinute);

            // reset hour to starting hour
            Instance.dateTime = DateTime.AddHours(Instance.startingHour);

            OnBeginDay.Invoke(Instance);

            // check if new week month or year
            if (CurrentDay == DayOfWeek.Monday) OnNewWeek.Invoke(Instance);
            if (_prevDateTime.Month != DateTime.Month) OnNewMonth.Invoke(Instance);
            
            OnMinuteTick.Invoke(Instance);
        }

        public static void PauseTime(bool freezeTimescale_ = true)
        {
            if (freezeTimescale_) Time.timeScale = 0f;
            Instance.isTimePaused = true;
            OnPauseTime.Invoke(Instance, Instance.isTimePaused);
        }

        public static void ResumeTime()
        {
            Time.timeScale = 1f;
            Instance.isTimePaused = false;
            OnPauseTime.Invoke(Instance, Instance.isTimePaused);
        }
    }
}