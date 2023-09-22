using System;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class PauseManager
    {
        private static PauseManager _instance;
        
        public static PauseManager Instance => _instance ??= new PauseManager();

        private float _timeScale;

        private PauseManager()
        {
            _timeScale = Time.timeScale;
        }

        public void PauseGame()
        {
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        public void ResumeGame()
        {
            Time.timeScale = _timeScale;
            AudioListener.pause = false;
        }
    }
}