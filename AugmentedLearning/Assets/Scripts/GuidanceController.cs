using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class GuidanceController : MonoBehaviour
{
    RectTransform rectTransform;

    #region Getter
    static GuidanceController instance;
    public static GuidanceController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GuidanceController>();
            if (instance == null)
                Debug.LogError("HomeController not found");
            return instance;
        }
    }
    #endregion Getter

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(rectTransform.rect.width, 0f);
    }

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(0, 0.3f).SetDelay(delay);
    }

    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(rectTransform.rect.width, 0.3f).SetDelay(delay);
    }

    public void ShowHomeScreen()
    {
        Hide();
        HomeController.Instance.Show();
    }
}
