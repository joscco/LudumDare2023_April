using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class Focus : MonoBehaviour
{
    private void OnMouseUp()
    {
        SceneTransitionManager.Get().TransitionTo("StartScene");
        AudioManager.instance.PlayMusic();
    }
}
