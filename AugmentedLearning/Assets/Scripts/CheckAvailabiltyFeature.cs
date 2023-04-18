using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

public class CheckAvailabiltyFeature : MonoBehaviour
{
    [SerializeField]
    Button m_ImageTracking;
    public Button ImageTracking
    {
        get => m_ImageTracking;
        set => m_ImageTracking = value;
    }

   
    // Start is called before the first frame update
    void Start()
    {
        var imageDescriptors = new List<XRImageTrackingSubsystemDescriptor>();
        SubsystemManager.GetSubsystemDescriptors(imageDescriptors);

        if (imageDescriptors.Count > 0)
        {
            m_ImageTracking.interactable = true;
        }
    }

    
}
