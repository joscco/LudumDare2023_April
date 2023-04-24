using System;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeedX = -0.5f;
    [SerializeField] private float scrollSpeedY = 0.5f;
    
    private MeshRenderer _meshRenderer;
    private Vector2 _savedOffset;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _savedOffset = _meshRenderer.sharedMaterial.mainTextureOffset;
    }

    private void Update()
    {
        var deltaX = Mathf.Repeat(Time.time, 1/Math.Abs(scrollSpeedX));
        var deltaY = Mathf.Repeat(Time.time, 1/Math.Abs(scrollSpeedY));
        var offset = new Vector2(deltaX * scrollSpeedX, deltaY * scrollSpeedY);
        _meshRenderer.sharedMaterial.mainTextureOffset = offset;
    }

    private void OnDisable()
    {
        _meshRenderer.sharedMaterial.mainTextureOffset = _savedOffset;
    }
}