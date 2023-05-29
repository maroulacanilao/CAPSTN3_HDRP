using System;
using System.Threading;
using System.Threading.Tasks;
using BaseCore;
using CustomEvent;
using Player;
using UnityEngine;

namespace Managers
{
    public class TimeManager : Singleton<TimeManager>
    {
        [Header("Time Values (24hr format)")]
        [SerializeField] private int endingHour = 24;
        [SerializeField] private int startingHour = 6;
        [SerializeField] private int nightHour = 19;
        [SerializeField] private float timeScale = 1; // 1 second in real time = 1 minute in game

        [Header("Events")]
        public static readonly Evt OnMinuteTick = new Evt();
        public static readonly Evt OnHourTick = new Evt();
        public static readonly Evt OnEndDay = new Evt();
        public static readonly Evt OnBeginDay = new Evt();
        public static readonly Evt OnNewWeek = new Evt();
        public static readonly Evt OnNewMonth = new Evt();
        public static readonly Evt<bool> OnPauseTime = new Evt<bool>();
        public static readonly Evt OnNightTime = new Evt();

        private bool isTimePaused = false;
        private int dayCounter = 0;
        private bool didDayStart = false;
        private DateTime dateTime;
        private bool isTimeLoopRunning;
        private CancellationTokenSource timerCTS; // CTS = CancellationTokenSource

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

            OnBeginDay.Invoke();

            isTimePaused = true;
            //TODO: remove this
            StartDay();
        }

        private void OnEnable()
        {
            if(timerCTS != null) ResumeTime();
        }

        private void OnDisable()
        {
            PauseTime();
        }


        private void OnDestroy()
        {
            timerCTS?.Cancel();
            timerCTS?.Dispose();
        }

        // called when starting a day to update time
        [NaughtyAttributes.Button("Start Day")]
        public void StartDay()
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

            OnBeginDay.Invoke();

            // check if new week month or year
            if (CurrentDay == DayOfWeek.Monday) OnNewWeek.Invoke();
            if (_prevDateTime.Month != DateTime.Month) OnNewMonth.Invoke();
            
            OnMinuteTick.Invoke();
            OnHourTick.Invoke();

            timerCTS?.Cancel();
            timerCTS?.Dispose();
            timerCTS = new CancellationTokenSource();
            Time.timeScale = 1;
            StartMainTimeLoop();
        }
        
        
        [NaughtyAttributes.Button("EndDay")]
        private void EndDay()
        {
            isTimePaused = true;
            isTimeLoopRunning = false;

            didDayStart = false;

            // for checking if game over and other visual things
            OnEndDay.Invoke();
        }

        [NaughtyAttributes.Button("Pause Time")]
        public void PauseTime()
        {
            timerCTS.Cancel();
            isTimePaused = true;
            isTimeLoopRunning = false;
            OnPauseTime.Invoke(Instance.isTimePaused);
            PlayerInputController.OnPlayerCanMove.Invoke(false);
        }

        [NaughtyAttributes.Button("Resume Time")]
        public void ResumeTime()
        {
            if(!isTimePaused) return;
            isTimePaused = false;
            
            OnPauseTime.Invoke(Instance.isTimePaused);
            
            timerCTS?.Cancel();
            timerCTS?.Dispose();
            timerCTS = new CancellationTokenSource();
            PlayerInputController.OnPlayerCanMove.Invoke(true);
            StartMainTimeLoop();
        }

        private async void StartMainTimeLoop()
        {
            try
            {
                int _delay = Mathf.RoundToInt(timeScale * 1000);
            
                while (!timerCTS.Token.IsCancellationRequested)
                {
                    await Task.Delay(_delay, timerCTS.Token);
                    if(timerCTS.Token.IsCancellationRequested) break;
                    UpdateTime();
                }
            }
            catch (Exception e)
            {
                isTimePaused = true;
                timerCTS = new CancellationTokenSource();
            }
        }

        private void UpdateTime()
        {
            if(isTimePaused) return;
            
            dateTime = dateTime.AddMinutes(1);
            OnMinuteTick.Invoke();

            if (CurrentMinute != 0) return;
                
            OnHourTick.Invoke();

            if (CurrentHour == nightHour)
            {
                OnNightTime.Invoke();
            }

            if (CurrentHour < endingHour) return;
            
            EndDay();
            isTimePaused = true;
        }
    }
}