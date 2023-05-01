using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class StartSceneManager : MonoBehaviour, SceneManager
{
    public TitleAnimation titleAnimation;

    private void Start()
    {
        titleAnimation.Hide();
    }

    public void AfterFade()
    {
        titleAnimation.FadeIn();
    }
}
