using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_ImageTrackingMenu;
    public GameObject ImageTrackingMenu
    {
        get { return m_ImageTrackingMenu; }
        set { m_ImageTrackingMenu = value; }
    }

   

    static void LoadScene(string sceneName)
    {
        LoaderUtility.Initialize();
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void ScannerButtonPressed()
    {
        LoadScene("ScannerScene");
    }


}
