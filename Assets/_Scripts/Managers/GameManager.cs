using System.Collections;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isGamePaused;
        private Coroutine _changeTimeScaleCoroutine;
        
        private const float DEFAULT_FIXED_DELTA_TIME = 0.02f;
        
        public void QuitTheGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    
        public void PauseGame()
        {
            if (_isGamePaused) return;
            
            _isGamePaused = true;
            Time.timeScale = 0;
            InputsManager.Instance.PlayerActions.Disable();
        }
    
        public void ResumeGame()
        {
            if (!_isGamePaused) return;
            
            _isGamePaused = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = DEFAULT_FIXED_DELTA_TIME;
            InputsManager.Instance.PlayerActions.Enable();
        }

        public void ChangeTimeScale(float timeScale, float transitionTime)
        {
            if (_changeTimeScaleCoroutine != null)
            {
                StopCoroutine(_changeTimeScaleCoroutine);
            }
            
            _changeTimeScaleCoroutine = StartCoroutine(ChangeTimeScaleOverTime(timeScale, transitionTime));
        }
        
        private IEnumerator ChangeTimeScaleOverTime(float targetTimeScale, float transitionTime)
        {
            var startingTimeScale = Time.timeScale;
            var timer = 0f;
            
            while (timer <= transitionTime)
            {
                timer += Time.unscaledDeltaTime;
                var timescale = Mathf.Lerp(startingTimeScale, targetTimeScale, timer / transitionTime);
                Time.timeScale = timescale;
                Time.fixedDeltaTime = DEFAULT_FIXED_DELTA_TIME * timescale;
                yield return null;
            }
        }
    }
}