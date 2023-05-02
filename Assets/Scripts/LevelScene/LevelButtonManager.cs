using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace GameScene.UI
{
    public class LevelButtonManager: MonoBehaviour, SceneManager
    {
        private LevelButton[] _levelButtons;
        private void Start()
        {
            _levelButtons = GetComponentsInChildren<LevelButton>();
            UpdateLevelButtons();
        }

        public void AfterFade()
        {
            // Nothing is enough
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