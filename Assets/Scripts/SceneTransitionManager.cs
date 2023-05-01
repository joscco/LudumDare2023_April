using System;
using System.Collections;
using DG.Tweening;
using GameScene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public SpriteRenderer pizza;
    public SpriteRenderer pizzaSchieber;
    
        private const float PIZZA_OUT_OFFSET = 2000f;
        private const float PIZZA_AFTER_OFFSET = -2000f;
        private const float PIZZA_BETWEEN_OFFSET = 0f;
        private const float PIZZASCHIEBER_OUT_OFFSET = -2000f;
        private const float PIZZASCHIEBER_BETWEEN_OFFSET = 0f;

        private float _transitionTimeInSeconds = 1f;
        private float _timeCoveredInSeconds = 0.2f;
        private string _currentSceneName;
        private bool _inTransition;

        private static SceneTransitionManager _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }

        public static SceneTransitionManager Get()
        {
            return _instance;
        }

        public bool IsInTransition()
        {
            return _inTransition;
        }

        private void Start()
        {
            InstantUncoverScreen();
            SceneManager.LoadScene("FocusScene", LoadSceneMode.Additive);
            _currentSceneName = "FocusScene";
        }

        public void TransitionTo(String levelName)
        {
            if (!_inTransition)
            {
                StartCoroutine(FadeInStartAndFadeOut(levelName));
            }
            
        }

        private IEnumerator FadeInStartAndFadeOut(String levelName)
        {
            _inTransition = true;
            
            // Start Loading in Background
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            // Start Animation
            CoverScreen();

            // Once faded in, Scene can be changed
            yield return new WaitForSeconds(_transitionTimeInSeconds);
            if (_currentSceneName != null)
            {
                SceneManager.UnloadSceneAsync(_currentSceneName);
            }

            asyncLoad.allowSceneActivation = true;
            _currentSceneName = levelName;

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(_timeCoveredInSeconds);

            // Scene has transitioned, now reverse Animation
            UncoverScreen();
            _inTransition = false;
            yield return new WaitForSeconds(_transitionTimeInSeconds / 2);
            InitSceneManager();
        }
        
        private void InstantUncoverScreen()
        {
            pizza.transform.position = PIZZA_OUT_OFFSET * new Vector2(0, 1);
            pizzaSchieber.transform.position = PIZZASCHIEBER_OUT_OFFSET * new Vector2(0, 1);
        }

        private void UncoverScreen()
        {
            pizzaSchieber.transform.DOMoveY(PIZZASCHIEBER_OUT_OFFSET, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            pizza.transform.DOMoveY(PIZZA_AFTER_OFFSET, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
        }

        private void CoverScreen()
        {
            pizza.transform.position = PIZZA_OUT_OFFSET * new Vector2(0, 1);
            pizza.transform.DOMoveY(PIZZA_BETWEEN_OFFSET, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            pizzaSchieber.transform.DOMoveY(PIZZASCHIEBER_BETWEEN_OFFSET, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
        }

        private void InitSceneManager()
        {
            FindObjectOfType<StartSceneManager>()?.AfterFade();
        }

        public void ReloadCurrentScene()
        {
            TransitionTo(_currentSceneName);
        }
}
