using System;
using CustomHelpers;
using Managers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Farming
{
    [System.Serializable]
    public class GrowingTileState : FarmTileState
    {
        private bool IsNextPhase = false;
        private int timeScale;

        public GrowingTileState(FarmTile farmTile_) : base(farmTile_)
        {
            tileState = TileState.Growing;
            timeScale = TimeManager.MinutePerTick;
        }

        public override void EnterLogic()
        {
            IsNextPhase = false;

            FarmTile.datePlanted = TimeManager.DateTime;
            FarmTile.timeRemaining = TimeSpan.FromMinutes(FarmTile.seedData.minutesToGrow).RoundToFive();
            
            ChangeRenderer();
            ChangeMaterial();
            TimeManager.OnMinuteTick.AddListener(UpdateTimeRemaining);
        }

        public override void ExitLogic()
        {
            FarmTile.timeRemaining = default;
            TimeManager.OnMinuteTick.RemoveListener(UpdateTimeRemaining);
        }

        private void UpdateTimeRemaining()
        {
            if (!FarmTile.isWatered) return;

            var _info = new GrowingJobInfo()
            {
                currentTime = CustomDateTime.FromDateTime(TimeManager.DateTime),
                datePlanted = CustomDateTime.FromDateTime(FarmTile.datePlanted),
                minutesToGrow = FarmTile.seedData.minutesToGrow,
                timeRemainingArray = new NativeArray<TimeSpan>(1, Allocator.TempJob),
                isReadyToHarvestArray = new NativeArray<bool>(1, Allocator.TempJob),
                isNextPhaseArray = new NativeArray<bool>(1, Allocator.TempJob)
            };
            
            var _job = new GrowingStateJob() { jobInfo = _info };
            
            var _handle = _job.Schedule();
            _handle.Complete();
            
            FarmTile.timeRemaining = _info.timeRemainingArray[0];
            var _isReadyToHarvest = _info.isReadyToHarvestArray[0];
            var _isNextPhase = _info.isNextPhaseArray[0];
            
            _info.timeRemainingArray.Dispose();
            _info.isReadyToHarvestArray.Dispose();
            _info.isNextPhaseArray.Dispose();

            if (_isReadyToHarvest)
            {
                FarmTile.ChangeState(FarmTile.readyToHarvestTileState);
                return;
            }
            
            if (_isNextPhase && !IsNextPhase)
            {
                UpdateAppearance();
            }
        }

        public void UpdateAppearance()
        {
            if (FarmTile.progress >= 0.3f)
            {
                IsNextPhase = true;
                FarmTile.plantRenderer.gameObject.SetActive(true);
                // ToDO: Add mat to farmtile
                // farmTile.soilMeshFilter.sprite = farmTile.defaultSoilSprite;
                FarmTile.plantRenderer.sprite = FarmTile.seedData.plantSprite;
                SoilMesh.mesh = TileState.Growing2.GetTileMesh();
            }
        }
    }

    [BurstCompile]
    public struct GrowingJobInfo
    {
        public CustomDateTime currentTime;
        public CustomDateTime datePlanted;
        public int minutesToGrow;

        public NativeArray<TimeSpan> timeRemainingArray;
        public NativeArray<bool> isReadyToHarvestArray;
        public NativeArray<bool> isNextPhaseArray;
    }
    
    [BurstCompile]
    public struct GrowingStateJob : IJob
    {
        public GrowingJobInfo jobInfo;
        
        public void Execute()
        {

            var _time = GetTimeSpanDifference(jobInfo.currentTime, jobInfo.datePlanted);
            var _timeRemaining = jobInfo.minutesToGrow - _time.TotalMinutes;
            jobInfo.timeRemainingArray[0] = TimeSpan.FromMinutes(_timeRemaining).RoundToFive();
            
            jobInfo.isReadyToHarvestArray[0] = _timeRemaining <= 0;
            
            jobInfo.isNextPhaseArray[0] = _timeRemaining <= jobInfo.minutesToGrow * 0.3f;
        }
        
        public CustomTimeSpan Difference(CustomDateTime dateTime1, CustomDateTime dateTime2)
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
        
        public int CalculateTotalSeconds(CustomDateTime dateTime)
        {
            int totalSeconds = 0;
            totalSeconds += dateTime.day * 86400;
            totalSeconds += dateTime.hour * 3600;
            totalSeconds += dateTime.minute * 60;
            totalSeconds += dateTime.second;
            return totalSeconds;
        }
        
        public TimeSpan GetTimeSpanDifference(CustomDateTime dateTime1, CustomDateTime dateTime2)
        {
            int totalSeconds1 = CalculateTotalSeconds(dateTime1);
            int totalSeconds2 = CalculateTotalSeconds(dateTime2);

            int deltaSeconds = totalSeconds1 - totalSeconds2;

            TimeSpan timeSpan = new TimeSpan(0, 0, deltaSeconds);
            return timeSpan;
        }
    }
}