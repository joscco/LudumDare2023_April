using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyBar : MonoBehaviour
{
    public TextMeshPro moneyText;
    public SpriteRenderer innerMoneyBar;
    private int _moneyNeeded = 1;

    public void SetMoneyNeeded(int moneyNeeded)
    {
        _moneyNeeded = moneyNeeded;
    }

    public void SetMoneyAcquired(int money)
    {
        moneyText.text = money + "/" + _moneyNeeded;
        var raw = innerMoneyBar.sprite.texture.width * Math.Clamp(1f * money / _moneyNeeded, 0f, 1f);
        innerMoneyBar.size = new Vector2(raw, innerMoneyBar.size.y);
    }

    public void TweenMoneyAcquired(int money)
    {
        moneyText.text = money + "/" + _moneyNeeded;
        moneyText.transform.DOScale(1.05f, 0.3f).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Yoyo);
        var raw = innerMoneyBar.sprite.texture.width * Math.Clamp(1f * money / _moneyNeeded, 0f, 1f);
        DoScaleInnerBar(new Vector2(raw,
            innerMoneyBar.size.y));
    }

    private void DoScaleInnerBar(Vector2 size)
    {
        DOTween.To(() => innerMoneyBar.size,
                (val) => { innerMoneyBar.size = val; },
                size, 
                0.3f)
            .SetEase(Ease.InOutBack);
    }
}
