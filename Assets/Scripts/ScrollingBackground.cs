using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private const float _scrollSpeed = 2f;
    private Vector2 _savedOffset;

    private void Start()
    {
        _savedOffset = meshRenderer.sharedMaterial.mainTextureOffset;
    }

    private void Update()
    {
        var x = Mathf.Repeat(Time.time * _scrollSpeed, 1);
        var offset = new Vector2(x, _savedOffset.y);
        meshRenderer.sharedMaterial.mainTextureOffset = offset;
    }

    private void OnDisable()
    {
        meshRenderer.sharedMaterial.mainTextureOffset = _savedOffset;
    }
}
