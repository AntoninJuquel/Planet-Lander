using System;
using UnityEngine;

namespace WaveSystem
{
    public interface IHandleWave
    {
        void OnNewWave();
        void OnWaveCleared();
        void OnWaveSpawn(GameObject toSpawn);
        void OnWaveFinished();

        event EventHandler<int> OnStartWave;
        event EventHandler OnStopWave;
    }
}