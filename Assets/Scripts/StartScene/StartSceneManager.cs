using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class StartSceneManager : MonoBehaviour, SceneManager
{
    public TitleAnimation titleAnimation;

    private void Start()
    {
        titleAnimation.Hide();
    }

    public void AfterSceneStart()
    {
        DOVirtual.DelayedCall(0.5f, () => titleAnimation.FadeIn());
    }
}
