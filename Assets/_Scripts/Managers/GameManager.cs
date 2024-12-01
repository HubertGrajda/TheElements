using System.Collections;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private bool _isGamePaused;
        private Coroutine _changeTimeScaleCoroutine;
        
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
                Time.timeScale = Mathf.Lerp(startingTimeScale, targetTimeScale, timer / transitionTime);
                yield return null;
            }
        }
    }
}