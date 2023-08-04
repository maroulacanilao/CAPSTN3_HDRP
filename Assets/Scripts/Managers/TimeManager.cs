using System;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using ScriptableObjectData;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Managers
{
    [DefaultExecutionOrder(-99)]
    public class TimeManager : Singleton<TimeManager>
    {
        [SerializeField] private ProgressionData progressionData;
        [SerializeField] private TimeSettings timeSettings;

        [Header("Events")]
        public static readonly Evt OnMinuteTick = new Evt();
        public static readonly Evt OnHourTick = new Evt();
        public static readonly Evt OnEndDay = new Evt();
        public static readonly Evt OnBeginDay = new Evt();
        public static readonly Evt OnNewWeek = new Evt();
        public static readonly Evt OnNewMonth = new Evt();
        public static readonly Evt<bool> OnPauseTime = new Evt<bool>();
        public static readonly Evt OnNightTime = new Evt();

        private bool isTimePaused = true;
        private bool didDayStart = false;
        private DateTime dateTime;
        private DateTime endTime;
        
        private bool timeStarted = false;

        #region Getters and Setters

        public static bool DidDayStart
        {
            get => Instance.didDayStart;
            set => Instance.didDayStart = value;
        }

        public static int EndingHour => Instance.timeSettings.endingHour;

        public static int StartingHour => Instance.timeSettings.startingHour;

        public static int NightHour => Instance.timeSettings.nightHour;

        public static float TimeScale => Instance.timeSettings.tickRate;

        public static int MinutePerTick => Instance.timeSettings.minutePerTick;
        
        public static int MaxDays => Instance.timeSettings.maxDays;
        
        public static int DaysLeft => MaxDays - DayCounter;

        public static bool IsWeekend => CurrentDay is DayOfWeek.Saturday or DayOfWeek.Sunday;
        public static float GameTime => CurrentHour + ( CurrentMinute/ 60f);
        public static bool IsTimePaused => Instance.isTimePaused;
        public static int DayCounter => Instance.progressionData.dayCounter;
        public static int CurrentHour => DateTime.Hour;
        public static int CurrentMinute => DateTime.Minute;
        public static int CurrentYear => DateTime.Year;
        public static int CurrentDate => DateTime.Day;
        public static DayOfWeek CurrentDay => DateTime.DayOfWeek;
        public static DateTime DateTime => Instance.dateTime;
        public static DateTime EndTime => Instance.endTime;

        #endregion

        private float timer;
        // For Debugging purposes
        private string timeText;

        protected override void Awake()
        {
            base.Awake();
            
        }

        private void OnEnable()
        {
            ResumeTime();
        }

        private void OnDisable()
        {
            PauseTime();
        }
        
        private void Update()
        {
            if(!timeStarted) return;
            if(isTimePaused) return;
            
            var _job = new TimeManagerJob()
            {
                dateTime = CustomDateTime.FromDateTime(dateTime),
                endTime = CustomDateTime.FromDateTime(endTime),
                minutePerTick = MinutePerTick,
                nightHour = NightHour,
                tickRate = timeSettings.tickRate,
                deltaTime = Time.deltaTime,
                timer = timer,
                didMinuteTick = new NativeArray<bool>(1, Allocator.TempJob),
                didHourTick = new NativeArray<bool>(1, Allocator.TempJob),
                isEndDay = new NativeArray<bool>(1, Allocator.TempJob),
                isNightTime = new NativeArray<bool>(1, Allocator.TempJob),
                timerResult = new NativeArray<float>(1, Allocator.TempJob),
                dateTimeResult = new NativeArray<CustomDateTime>(1, Allocator.TempJob)
            };
            
            var _jobHandle = _job.Schedule();
            _jobHandle.Complete();
            
            timer = _job.timerResult[0];

            if (_job.didMinuteTick[0])
            {
                dateTime = _job.dateTimeResult[0].ToDateTime();
                OnMinuteTick.Invoke();
                
                if(_job.didHourTick[0]) OnHourTick.Invoke();
                
                if(_job.isNightTime[0]) OnNightTime.Invoke();
                
                if (_job.isEndDay[0])
                {
                    EndDay();
                    isTimePaused = true;
                }
                
                timer = 0;
            }
            
            _job.timerResult.Dispose();
            _job.didMinuteTick.Dispose();
            _job.didHourTick.Dispose();
            _job.isEndDay.Dispose();
            _job.isNightTime.Dispose();
            _job.dateTimeResult.Dispose();
        }

        public static void StartTime(DateTime dateTime_ = default) 
        {
            if (Instance.timeStarted)
            {
                if(dateTime_ == default) Instance.dateTime = new DateTime(2023, 1, 1);
                return;
            }
            Instance.timeStarted = true;
            Instance.dateTime = dateTime_ == default ? new DateTime(2023, 1, 1) : dateTime_;
            BeginDay();
        }

        // called when starting a day to update time
        [NaughtyAttributes.Button("Start Day")]
        public static void BeginDay()
        {
            var _prevDateTime = new DateTime();
            _prevDateTime = DateTime;

            Instance.dateTime = DateTime.AddDays(1);


            //reset hour and minutes to 0:00
            Instance.dateTime = DateTime.AddHours(-CurrentHour);
            Instance.dateTime = DateTime.AddMinutes(-CurrentMinute);

            // reset hour to starting hour
            Instance.dateTime = DateTime.AddHours(StartingHour);

            OnBeginDay.Invoke();

            var _end = new DateTime(Instance.dateTime.Year, Instance.dateTime.Month, Instance.dateTime.Day, EndingHour, 0, 0);
            
            Instance.endTime = Instance.timeSettings.isEndingHourNextDay ? _end.AddDays(1) : _end;

            // check if new week month or year
            if (CurrentDay == DayOfWeek.Monday) OnNewWeek.Invoke();
            if (_prevDateTime.Month != DateTime.Month) OnNewMonth.Invoke();
            
            OnMinuteTick.Invoke();
            OnHourTick.Invoke();

            Time.timeScale = 1;

            Instance.timeText = $"{CurrentDay} {CurrentDate} {CurrentHour}:{CurrentMinute}";
            Debug.Log(Instance.timeText);
            
            Instance.isTimePaused = false;
        }
        
        
        [NaughtyAttributes.Button("EndDay")]
        public static void EndDay()
        {
            Instance.isTimePaused = true;

            Instance.didDayStart = false;
            
            Instance.progressionData.dayCounter++;
            
            Debug.Log($"{DaysLeft} Days Left");

            OnEndDay.Invoke();
        }

        [NaughtyAttributes.Button("Pause Time")]
        private void PauseTimeBTN()
        {
            PauseTime(false);
        }
        
        public static void  PauseTime(bool pauseTimeScale_ = true)
        {
            Instance.isTimePaused = true;
            if (pauseTimeScale_) Time.timeScale = 0;
            OnPauseTime.Invoke(Instance.isTimePaused);
        }

        [NaughtyAttributes.Button("Resume Time")]
        private void ResumeTimeBTN()
        {
            PauseTime(false);
        }
        
        public static void ResumeTime(bool resumeTimeScale_ = true)
        {
            if(!Instance.timeStarted) return;
            Time.timeScale = 1;
            
            if(!Instance.isTimePaused) return;
            Instance.isTimePaused = false;
            
            
            OnPauseTime.Invoke(Instance.isTimePaused);
        }

        private void UpdateTime()
        {
            if(isTimePaused) return;
            
            dateTime = dateTime.AddMinutes(MinutePerTick).RoundToFive();
            OnMinuteTick.Invoke();

            Instance.timeText = $"{CurrentDay} {CurrentDate} {CurrentHour}:{CurrentMinute}";
            
            if (CurrentMinute != 0) return;
                
            OnHourTick.Invoke();

            if (CurrentHour == NightHour)
            {
                OnNightTime.Invoke();
            }
            
            if(dateTime < endTime) return;

            EndDay();
            isTimePaused = true;
        }
        
        public static void AddMinutes(int minutes_)
        {
            Instance.dateTime = Instance.dateTime.AddMinutes(minutes_).RoundToFive();
            OnMinuteTick.Invoke();
        }
        
        public static void SetDateTime(DateTime dateTime_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.dateTime = dateTime_;
            OnMinuteTick.Invoke();
        }

        public static bool IsNight()
        {
            if(Instance.IsEmptyOrDestroyed()) return false;
            return CurrentHour >= NightHour || CurrentHour < 5;
        }

        public static void ResetData()
        {
            Instance.timeStarted = false;
            
        }
    }
    
    [BurstCompile]
    public struct TimeManagerJob : IJob
    {
        public CustomDateTime dateTime;
        public CustomDateTime endTime;
        public int minutePerTick;
        public int nightHour;
        public float tickRate;
        public float deltaTime;
        public float timer;

        public NativeArray<float> timerResult;
        public NativeArray<CustomDateTime> dateTimeResult;
        public NativeArray<bool> didMinuteTick;
        public NativeArray<bool> didHourTick;
        public NativeArray<bool> isNightTime;
        public NativeArray<bool> isEndDay;
        
        public void Execute()
        {
            dateTimeResult[0] = dateTime;
            didMinuteTick[0] = false;
            didHourTick[0] = false;
            isEndDay[0] = false;
            
            var _timerResult = timer + deltaTime;
            
            timerResult[0] = _timerResult;
            
            var _minutePassed = _timerResult >= tickRate;
            didMinuteTick[0] = _minutePassed;
            
            if (!_minutePassed) return;
            
            var _dateResult = dateTime.AddMinutesRoundedToFive(minutePerTick);
            dateTimeResult[0] = _dateResult;

            if (_dateResult.minute != 0) return;
            
            didHourTick[0] = true;
            if(_dateResult.hour == nightHour) isNightTime[0] = true;
            
            if(CompareCustomDateTime(dateTime,endTime) >= 0 ) isEndDay[0] = true;
        }
        
        public int CompareCustomDateTime(CustomDateTime dateTime1, CustomDateTime dateTime2)
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
    }
}