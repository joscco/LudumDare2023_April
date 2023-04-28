using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public List<SpriteRenderer> letters;
    private List<Vector3> _initialPositions;
    public int yOffsetWhenFadeOut = -800;
    public float secondOffsetLetterAnimations = 0.1f;
    public float secondDurationLetterAnimation = 0.5f;

    private void Start()
    {
        _initialPositions = new List<Vector3>(letters.Select(letter => letter.transform.position));
    }

    public void Hide()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            letters[i].transform.position = _initialPositions[i] + yOffsetWhenFadeOut * Vector3.up;
        }
    }

    public void Show()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            letters[i].transform.position = _initialPositions[i];
        }
    }

    public void FadeIn()
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < letters.Count; i++)
        {
            seq.Insert(
                i * secondOffsetLetterAnimations,
                letters[i].transform.DOMoveY(
                        _initialPositions[i].y,
                        secondDurationLetterAnimation)
                    .SetEase(Ease.InOutBack)
            );
        }

        seq.Play();
    }

    public void FadeOut()
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < letters.Count; i++)
        {
            seq.Insert(
                i * secondOffsetLetterAnimations,
                letters[i].transform.DOMoveY(
                        _initialPositions[i].y + yOffsetWhenFadeOut,
                        secondDurationLetterAnimation)
                    .SetEase(Ease.InOutBack)
            );
        }

        seq.Play();
    }
}