using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            MainMenu,
            DuringGameplay,
            DuringCutscene,
            DuringLoading
        }

        private GameState _currentGameState;
        public UnityAction<GameState> onGameStateChanged;

        private bool _isGamePaused;
        public bool IsGamePaused => _isGamePaused;
    
        private void Start()
        {
            Managers.AudioManager.PlaySound("MainMenuMusic1");
        }
    
        public void QuitTheGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    
        public void PauseGame()
        {
            _isGamePaused = true;
            Time.timeScale = 0;
            Managers.InputManager.PlayerActions.Disable();
        }
    
        public void ResumeGame()
        {
            _isGamePaused = false;
            Time.timeScale = 1;
            Managers.InputManager.PlayerActions.Enable();
        }
    
        public IEnumerator ChangeTimeScale(float targetTimeScale, float transitionTime)
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

        public void ChangeState(GameState newState)
        {
            _currentGameState = newState;
            onGameStateChanged?.Invoke(_currentGameState);
        }
    }
}