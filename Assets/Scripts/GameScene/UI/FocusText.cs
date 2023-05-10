using DG.Tweening;
using TMPro;
using UnityEngine;

public class FocusText : MonoBehaviour
{
    public TextMeshPro text;
    void Start()
    {
        text.color = new Color(0, 0, 0, 0);
        text.DOFade(1, 0.5f);
        transform.DOScale(1.025f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}
