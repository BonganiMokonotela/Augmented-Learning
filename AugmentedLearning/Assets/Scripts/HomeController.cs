using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RectTransform))]
public class HomeController : MonoBehaviour
{
    RectTransform rectTransform;

    #region Getter
    static HomeController instance;
    public static HomeController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<HomeController>();
            if (instance == null)
                Debug.LogError("HomeController not found");
            return instance;
        }
    }
    #endregion Getter

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(0, 0f);
    }

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(0, 0.3f).SetDelay(delay);
    }

    public void Hide(float delay = 0f)
    {

        rectTransform.DOAnchorPosX(rectTransform.rect.width * -1, 0.3f).SetDelay(delay);
    }

    public void ShowGuidanceMenu()
    {
        Hide();
        GuidanceController.Instance.Show();
    }

   
}
