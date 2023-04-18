using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class BackButton : MonoBehaviour
{
    [SerializeField]
    GameObject m_BackButton;
    public GameObject backButton
    {
        get => m_BackButton;
        set => m_BackButton = value;
    }

    void Start()
    {
        if (Application.CanStreamedLevelBeLoaded("MenuScene"))
        {
            m_BackButton.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButtonPressed();
        }
    }

    public void BackButtonPressed()
    {
        if (Application.CanStreamedLevelBeLoaded("MenuScene"))
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
            LoaderUtility.Deinitialize();
        }
    }
}

