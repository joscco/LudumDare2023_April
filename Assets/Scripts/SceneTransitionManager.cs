using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTransitionManager : MonoBehaviour
    {
        public CanvasGroup leftOverlay;
        public CanvasGroup rightOverlay;
        
        private float _transitionTimeInSeconds = 1f;
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
            leftOverlay.transform.position = -800f * new Vector2(1, 0);
            rightOverlay.transform.position = 800f * new Vector2(1, 0);
            TransitionTo("StartScene");
        }

        public void TransitionTo(String levelName)
        {
            StartCoroutine(FadeInStartAndFadeOut(levelName));
        }

        private IEnumerator FadeInStartAndFadeOut(String levelName)
        {
            _inTransition = true;
            
            // Start Loading in Background
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            // Start Animation
            leftOverlay.transform.DOMoveX(0, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            rightOverlay.transform.DOMoveX(0, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);

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

            // Scene has transitioned, now reverse Animation
            leftOverlay.transform.DOMoveX(-800, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            rightOverlay.transform.DOMoveX(800, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            
            _inTransition = false;
        }
    }
