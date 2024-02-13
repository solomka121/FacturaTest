using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _winLabel;
    
    public void Show() 
    {
        gameObject.SetActive(true);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InOutCubic);
        
        _winLabel.transform.localScale = Vector3.zero;
        _winLabel.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.InOutBack);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
