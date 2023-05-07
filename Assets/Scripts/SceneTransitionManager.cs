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
    
        private const float PizzaOutOffset = 2000f;
        private const float PizzaAfterOffset = -2000f;
        private const float PizzaBetweenOffset = 0f;
        private const float PizzaschieberOutOffset = -2000f;
        private const float PizzaschieberBetweenOffset = 0f;

        private float _transitionTimeInSeconds = 1f;
        private float _timeCoveredInSeconds = 0.2f;
        private string _currentSceneName;
        private bool _inTransition;

        private int _currentLevel;

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

        public void TransitionTo(String levelName, int level = 0)
        {
            if (!_inTransition)
            {
                StartCoroutine(FadeInStartAndFadeOut(levelName, level));
            }
            
        }

        private IEnumerator FadeInStartAndFadeOut(String levelName, int level)
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
            InitSceneManager(level);
            UncoverScreen();
            _inTransition = false;
        }
        
        private void InstantUncoverScreen()
        {
            pizza.transform.position = PizzaOutOffset * new Vector2(0, 1);
            pizzaSchieber.transform.position = PizzaschieberOutOffset * new Vector2(0, 1);
        }

        private void UncoverScreen()
        {
            pizzaSchieber.transform.DOMoveY(PizzaschieberOutOffset, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            pizza.transform.DOMoveY(PizzaAfterOffset, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
        }

        private void CoverScreen()
        {
            pizza.transform.position = PizzaOutOffset * new Vector2(0, 1);
            pizza.transform.DOMoveY(PizzaBetweenOffset, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            pizzaSchieber.transform.DOMoveY(PizzaschieberBetweenOffset, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
        }

        private void InitSceneManager(int level)
        {
            FindObjectOfType<StartSceneManager>()?.AfterSceneStart();

            if (level != 0)
            {
                FindObjectOfType<LevelManager>()?.StartLevel(level);
            }
        }

        public void ReloadCurrentLevel()
        {
            StartLevel(_currentLevel);
        }

        public void LoadNextLevel()
        {
            StartLevel(_currentLevel + 1);
        }

        public void StartLevel(int level)
        {
            _currentLevel = level;
            TransitionTo("GameLevelScene", level);
        }

        public int GetCurrentLevel()
        {
            return _currentLevel;
        }
}
