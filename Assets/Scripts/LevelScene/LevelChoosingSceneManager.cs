using System;
using System.Linq;
using UnityEngine;

namespace GameScene.UI
{
    public class LevelChoosingSceneManager: MonoBehaviour
    {
        public static LevelChoosingSceneManager instance;
        public LevelSceneBackButton backButton;
        
        private LevelButton[] _levelButtons;
        private LevelButton _activeButton;

        private void Start()
        {
            instance = this;
            
            _levelButtons = GetComponentsInChildren<LevelButton>();
            SetActiveButton(Math.Max(1, Game.instance.GetUnlockedLevel()));
            UpdateLevelButtons();
        }

        public void SetActiveButton(int level)
        {
            int unlockedLevels = Game.instance.GetUnlockedLevel();
            if (level > 0 && level <= unlockedLevels + 1)
            {
                if (_activeButton)
                {
                    _activeButton.OnSetDeactive();
                }
                _activeButton = _levelButtons.First(button => button.level == level);
                _activeButton.OnSetActive();
            }
        }

        private void Update()
        {
            var move = InputManager.instance.GetMoveDirection();
            
            if (OptionScreen.instance.IsVisible())
            {
                // Option Screen is showing
                OptionScreen.instance.HandleMoveInput(move);
                if (InputManager.instance.GetEnterOrSpace())
                {
                    OptionScreen.instance.OnPressEnter();
                }
            }
            else
            {
                HandleMoveInput(move);
                if (InputManager.instance.GetEnterOrSpace())
                {
                    OnPressEnter();
                }
            }

            if (!SceneManager.Get().IsInTransition())
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    backButton.Trigger();
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    FindObjectOfType<OptionButton>()?.Trigger();
                }
            }
        }

        private void OnPressEnter()
        {
            if (_activeButton)
            {
                _activeButton.Trigger();
            }
        }

        private void HandleMoveInput(Vector2Int move)
        {
            int activeLevel = _activeButton ? _activeButton.level : 1;

            if (move.y != 0)
            {
               SetActiveButton(activeLevel + move.y); 
            }
            else
            {
                if (move.x == 1)
                {
                    SetActiveButton(Math.Clamp(activeLevel + 6, 7, 13));
                } else if (move.x == -1)
                {
                    SetActiveButton(Math.Clamp(activeLevel - 6, 1, 6));
                }
            }
        }

        private void UpdateLevelButtons()
        {
            int unlockedLevels = Game.instance.GetUnlockedLevel();
            foreach (var levelButton in _levelButtons)
            {
                if (levelButton.level <= unlockedLevels + 1)
                {
                    levelButton.SetAvailable();
                }
                else
                {
                    levelButton.SetInavailable();
                }
            }
        }
    }
}