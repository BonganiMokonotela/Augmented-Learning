using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System;

[RequireComponent(typeof(ARTrackedImageManager))]
public class AugmentedObject : MonoBehaviour, ISerializationCallbackReceiver
{

    [SerializeField]
    public GameObject FitToScan;

    [Serializable]
    struct PrefabName
    {
        public string imageGuid;

        public GameObject prefab;

        public PrefabName(Guid guid, GameObject prefab)
        {
            imageGuid = guid.ToString();

            this.prefab = prefab;
        }
    }


    [SerializeField]
    [HideInInspector]
    List<PrefabName> m_PrefabsList = new List<PrefabName>();

    Dictionary<Guid, GameObject> m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
    Dictionary<Guid, GameObject> m_Instantiated = new Dictionary<Guid, GameObject>();
    ARTrackedImageManager m_TrackedImageManager;

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;


    public XRReferenceImageLibrary ImageLibrary
    {
        get { return m_ImageLibrary; }
        set { m_ImageLibrary = value; }
    }

    public void OnBeforeSerialize()
    {
        m_PrefabsList.Clear();
        foreach (var kvp in m_PrefabsDictionary)
        {
            m_PrefabsList.Add(new PrefabName(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
        foreach (var entry in m_PrefabsList)
        {
            m_PrefabsDictionary.Add(Guid.Parse(entry.imageGuid), entry.prefab);
        }
    }


    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var trackedImage in obj.added)
        {
            var minLocalScalar = Mathf.Min(trackedImage.size.x, trackedImage.size.y) / 2;
            trackedImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
            AssignPrefab(trackedImage);
        }
       
    }

    void AssignPrefab(ARTrackedImage trackedImage)
    {
        if (m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab))
        {
            m_Instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);

            FitToScan.SetActive(false);
        }
        

    }

    public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
    {
        return m_PrefabsDictionary.TryGetValue(referenceImage.guid, out var prefab) ? prefab : null;
    }




#if UNITY_EDITOR
   
    [CustomEditor(typeof(AugmentedObject))]
    class PrefabImagePairManagerInspector : Editor
    {
        List<XRReferenceImage> m_ReferenceImages = new List<XRReferenceImage>();
        bool m_IsExpanded = true;

        bool HasLibraryChanged(XRReferenceImageLibrary library)
        {
            if (library == null)
                return m_ReferenceImages.Count == 0;

            if (m_ReferenceImages.Count != library.count)
                return true;

            for (int i = 0; i < library.count; i++)
            {
                if (m_ReferenceImages[i] != library[i])
                    return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            //customized inspector
            var behaviour = serializedObject.targetObject as AugmentedObject;

            serializedObject.Update();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }

            var libraryProperty = serializedObject.FindProperty(nameof(m_ImageLibrary));
            EditorGUILayout.PropertyField(libraryProperty);
            var library = libraryProperty.objectReferenceValue as XRReferenceImageLibrary;

            //check library changes
            if (HasLibraryChanged(library))
            {
                if (library)
                {
                    var tempDictionary = new Dictionary<Guid, GameObject>();
                    foreach (var referenceImage in library)
                    {
                        tempDictionary.Add(referenceImage.guid, behaviour.GetPrefabForReferenceImage(referenceImage));
                    }
                    behaviour.m_PrefabsDictionary = tempDictionary;
                }
            }

            // update current
            m_ReferenceImages.Clear();
            if (library)
            {
                foreach (var referenceImage in library)
                {
                    m_ReferenceImages.Add(referenceImage);
                }
            }

            //show prefab list
            m_IsExpanded = EditorGUILayout.Foldout(m_IsExpanded, "Prefab List");
            if (m_IsExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUI.BeginChangeCheck();

                    var tempDictionary = new Dictionary<Guid, GameObject>();
                    foreach (var image in library)
                    {
                        var prefab = (GameObject)EditorGUILayout.ObjectField(image.name, behaviour.m_PrefabsDictionary[image.guid], typeof(GameObject), false);
                        tempDictionary.Add(image.guid, prefab);
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "Update Prefab");
                        behaviour.m_PrefabsDictionary = tempDictionary;
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
