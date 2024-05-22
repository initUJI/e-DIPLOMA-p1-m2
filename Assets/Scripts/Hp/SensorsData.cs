using HP.Omnicept.Messaging.Messages;
using HP.Omnicept.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensorsData : MonoBehaviour
{

    // Lazy cache of GliaBehaviour 
    private GliaBehaviour _gliaBehaviour = null;
    private GliaBehaviour gliaBehaviour
    {
        get
        {
            if (_gliaBehaviour == null)
            {
                _gliaBehaviour = FindObjectOfType<GliaBehaviour>();
            }

            return _gliaBehaviour;
        }
    }

    int _heartRate;
    CognitiveLoad _cognitiveLoad;
    Vector3 _eyeTracking;
    Texture2D _faceImage;

    public Vector3 deviation;

    Material _Facemat;

    GameObject xrHead;
    [SerializeField] GameObject _lookingAt;
    [SerializeField] GameObject _FaceCameraPlane;
    [SerializeField] TMP_Text _heartRateText;
    [SerializeField] TMP_Text _ObjectLookedText;
    [SerializeField] TMP_Text _EyeTrackVectorText;
    [SerializeField] TMP_Text _CognitiveLoadText;

    bool lookingAtObject = false;
    bool lookingAtObjectWithHead = false;

    string _objectLookingName = "";
    string _objectLookingWithHeadName = "";

    MeshRenderer selected = null;

    public void Awake()
    {
        xrHead = Camera.main.gameObject;
        StartListeningToHeartRate();
        StartListeningToEyeTracking();
        StartListeningToCognitiveLoad();
        StartListeningImageFace(); 
        if(_ObjectLookedText) _ObjectLookedText.SetText($"Did not Hit");

        if(_CognitiveLoadText) _CognitiveLoadText.SetText($"Calibrating...\nIt takes around 45 seconds");
    }

    private void Start()
    {
        if(_FaceCameraPlane) _Facemat = _FaceCameraPlane.GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        f_LookingAt();
    }

    // Subscribe to Heart Rate messages
    public void StartListeningToHeartRate()
    {
        gliaBehaviour.OnHeartRate.AddListener(AccumulateHeartRate);
    }

    // Unsubscribe to Heart Rate messages 
    public void StopListeningToHeartRate()
    {
        gliaBehaviour.OnHeartRate.RemoveListener(AccumulateHeartRate);
    }

    public void StartListeningToEyeTracking()
    {
        gliaBehaviour.OnEyeTracking.AddListener(GetEyeTracking);
    }

    public void StartListeningToCognitiveLoad()
    {
        gliaBehaviour.OnCognitiveLoad.AddListener(GetCognitiveLoad);
    }

    public void StartListeningImageFace()
    {
        gliaBehaviour.OnCameraImage.AddListener(GetFaceImage);
    }

    private void GetFaceImage(CameraImage cameraImage)
    {
        if (!_Facemat) return;

        _faceImage = new Texture2D(400, 400, TextureFormat.R8, false);
        _faceImage.LoadRawTextureData(cameraImage.ImageData);
        _faceImage.Apply();

        _Facemat.mainTexture = _faceImage; 
    }

    private void GetCognitiveLoad(CognitiveLoad cognitiveLoad)
    {
        _cognitiveLoad = cognitiveLoad;
        if(_CognitiveLoadText) _CognitiveLoadText.SetText($"{_cognitiveLoad.CognitiveLoadValue.ToString("0.0000")} \nDeviation:\n{_cognitiveLoad.StandardDeviation.ToString("0.0000")}");

        HPGameManager.Instance._dataManager.AddCognitiveLoadStatus(_cognitiveLoad.CognitiveLoadValue, _cognitiveLoad.StandardDeviation);
    }

    private void GetEyeTracking(EyeTracking eyeTracking)
    {
        _eyeTracking = new Vector3(-eyeTracking.CombinedGaze.X, eyeTracking.CombinedGaze.Y, eyeTracking.CombinedGaze.Z);
        //_eyeTracking = new Vector3(-eyeTracking.LeftEye.Gaze.X, eyeTracking.LeftEye.Gaze.Y, eyeTracking.LeftEye.Gaze.Z); // saca el vector solo del ojo izquierdo
        if(_EyeTrackVectorText) _EyeTrackVectorText.SetText(_eyeTracking + "");
    }
    private void AccumulateHeartRate(HeartRate heartRate)
    {
        _heartRate = (int) heartRate.Rate;

        //Change Text Value
       if(_heartRateText) _heartRateText.SetText(_heartRate + "");

        HPGameManager.Instance._dataManager.AddHeartRateStatus(_heartRate);
    }
    public void f_LookingAt() 
    {
        RaycastHit hit;
        Vector3 origin = xrHead.transform.position;
        origin += deviation;
        Quaternion headDirection = Quaternion.Euler(xrHead.transform.rotation.eulerAngles);
        Vector3 headDir = headDirection * Vector3.forward;

        Debug.DrawRay(origin, headDir.normalized * 20f);
        //HEAD Direction Raycast
        if (Physics.Raycast(origin, headDir.normalized, out hit, Mathf.Infinity))
        {
            _objectLookingWithHeadName = hit.collider.gameObject.name;

            if (GetHighestParent(hit.collider.gameObject.transform).name.Contains("Level"))
            {
                _objectLookingWithHeadName += ": Part of the level";
            }
            else if (GetHighestParent(hit.collider.gameObject.transform).name.Contains("Block"))
            {
                _objectLookingWithHeadName += ": Part of a Block";
            }
            else
            {
                _objectLookingWithHeadName += ": Part of " + GetHighestParent(hit.collider.gameObject.transform).name;
            }

            if (!lookingAtObjectWithHead)
            {
                lookingAtObjectWithHead = true;
                HPGameManager.Instance._dataManager.AddEyeAction(eDataEyeTracking.ENTRA_HEAD, _objectLookingWithHeadName);
            }
        }
        else
        {
            if (lookingAtObjectWithHead)
            {
                lookingAtObjectWithHead = false;
                HPGameManager.Instance._dataManager.AddEyeAction(eDataEyeTracking.SALE_HEAD, _objectLookingWithHeadName);
                _objectLookingWithHeadName = "";
            }
        }

        //Transformamos el vector del eyetracking al axis de la camara en el mundo de unity
        Vector3 eyeDir = headDirection * _eyeTracking;
        Debug.DrawRay(origin, eyeDir.normalized * 100);

        //EYE Direction Raycast
        if (Physics.Raycast(origin, eyeDir.normalized, out hit, Mathf.Infinity))
        {
            _objectLookingName = hit.collider.gameObject.name;

            if (GetHighestParent(hit.collider.gameObject.transform).name.Contains("Level"))
            {
                _objectLookingName += ": Part of the level";
            }
            else if (GetHighestParent(hit.collider.gameObject.transform).name.Contains("Block"))
            {
                _objectLookingName += ": Part of a Block";
            }
            else
            {
                _objectLookingName += ": Part of " + GetHighestParent(hit.collider.gameObject.transform).name;
            }

            if (!lookingAtObject)
            {
                lookingAtObject = true;
                HPGameManager.Instance._dataManager.AddEyeAction(eDataEyeTracking.ENTRA, _objectLookingName);
            }
            
            //Debug
            if(_lookingAt) _lookingAt.transform.position = hit.point;
            if(_ObjectLookedText) _ObjectLookedText.SetText(_objectLookingName);

            if(selected != null) selected.enabled = false;

            if(hit.collider.tag.Equals("Cube") || hit.collider.tag.Equals("Sphere") || hit.collider.tag.Equals("Capsule"))
                selected = getChildGameObject(hit.collider.gameObject, "Quad")?.GetComponent<MeshRenderer>();

            if(selected != null) selected.enabled = true;
        }
        else
        {
            if (lookingAtObject)
            {
                lookingAtObject = false;
                HPGameManager.Instance._dataManager.AddEyeAction(eDataEyeTracking.SALE, _objectLookingName);
                _objectLookingName = "";
            }
        }
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public static Transform GetHighestParent(Transform currentTransform)
    {
        // Iterar a trav�s de los padres hasta llegar al padre m�s alto
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        // Devuelve el Transform del padre m�s alto
        return currentTransform;
    }
}
