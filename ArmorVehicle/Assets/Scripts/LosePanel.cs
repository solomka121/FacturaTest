using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _loseLabel;

    public void Show() 
    {
        gameObject.SetActive(true);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.5f).SetEase(Ease.InOutCubic);
        
        _loseLabel.transform.localScale = Vector3.zero;
        _loseLabel.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.InOutBack);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
