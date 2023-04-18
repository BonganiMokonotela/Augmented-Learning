using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class AugmentedObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    private Dictionary<string, GameObject> placedPrefabs = new Dictionary<string, GameObject>();

    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in prefabs)
        {
            GameObject gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            gameObject.name = prefab.name;
            placedPrefabs.Add(prefab.name, gameObject);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage trackedImage in obj.added)
        {
            UpadateImage(trackedImage);
        }
        
        foreach (ARTrackedImage trackedImage in obj.updated)
        {
            UpadateImage(trackedImage);
        }
        
        foreach (ARTrackedImage trackedImage in obj.removed)
        {
            placedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpadateImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        Vector3 vector3 = trackedImage.transform.position;

        GameObject gameObject = placedPrefabs[name];

        gameObject.transform.position = vector3;

        gameObject.SetActive(true);

        foreach (GameObject prefab in placedPrefabs.Values)
        {
            if (prefab.name != imageName)
            {
                prefab.SetActive(false);
            }
        }
    }








    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
