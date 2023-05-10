using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class Focus : MonoBehaviour
{
    private void OnMouseUp()
    {
        SceneManager.Get().TransitionTo("StartScene");
        AudioManager.instance.PlayMusic();
    }
}
