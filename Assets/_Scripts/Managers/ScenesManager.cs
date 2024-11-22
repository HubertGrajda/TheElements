using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        private const string MAIN_MENU_SCENE = "MainMenu";

        [SerializeField] private Animator levelLoader;
    
        private static readonly int FadeIn = Animator.StringToHash(FADE_IN);
        private static readonly int FadeOut = Animator.StringToHash(FADE_OUT);

        private const string FADE_OUT = "FadeOut";
        private const string FADE_IN = "FadeIn";

        public void LoadMainMenu()
        {
            LaunchSceneFromName(MAIN_MENU_SCENE);
        }
    
        private void LaunchSceneFromName(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
        
            StartCoroutine(LoadLevel(MAIN_MENU_SCENE));
        }
    
        public void PreviousScene()
        {
            var prevSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
            StartCoroutine(LoadLevel(prevSceneIndex));
        }
    
        public void RestartScene()
        {
            var currSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadLevel(currSceneIndex));
        }
    
        public void NextScene()
        {
            var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            StartCoroutine(LoadLevel(nextSceneIndex));
        }

        public void LaunchInitScene()
        {
            StartCoroutine(LoadLevel(0));
        }
    
        private IEnumerator LoadLevel(int sceneIndex)
        {
            yield return BeforeLoad();
        
            SceneManager.LoadScene(sceneIndex);
        }
    
        private IEnumerator LoadLevel(string sceneName)
        {
            yield return BeforeLoad();

            SceneManager.LoadScene(sceneName);
        }

        private IEnumerator BeforeLoad()
        {
            SceneManager.sceneLoaded += AfterLoad;
        
            levelLoader.SetTrigger(FadeIn);
        
            yield return new WaitUntil(() => levelLoader.GetCurrentAnimatorStateInfo(0).IsName(FADE_IN));
            yield return new WaitWhile(() => levelLoader.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1);
        
            UIManager.Instance.HideCurrentView();
        }

        private void AfterLoad(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= AfterLoad;
            levelLoader.SetTrigger(FadeOut);
        }
    }
}