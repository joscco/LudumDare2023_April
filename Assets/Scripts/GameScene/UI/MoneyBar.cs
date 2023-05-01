using System.Collections;
using System.Collections.Generic;
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
        innerMoneyBar.size = new Vector2(innerMoneyBar.sprite.texture.width * 1f * money / _moneyNeeded, innerMoneyBar.size.y);
    }
}
