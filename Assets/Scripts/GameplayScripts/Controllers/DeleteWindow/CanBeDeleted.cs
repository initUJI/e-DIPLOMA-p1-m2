using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CanBeDeleted : MonoBehaviour
{
    public XRController principalHandController;
    private XRRayInteractor principalRay;
    public XRBaseInteractable interactable;
    public bool primaryButtonValue;
    public bool openWindow;
    public GameObject currentWindow;

    [SerializeField] private GameObject deleteWindowPrefab;
    private EventsManager eventsManager;

    private void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        openWindow = false;

        // Cache references for performance
        principalHandController = GameObject.Find("RightHand Controller")?.GetComponent<XRController>();
        principalRay = principalHandController?.GetComponent<XRRayInteractor>();
    }

    private void Update()
    {
        if (interactable == null || principalHandController == null || principalRay == null || openWindow)
            return;

        // Cache input device once per frame
        InputDevice device = principalHandController.inputDevice;
        if (device != null && device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue))
        {
            if (primaryButtonValue && principalRay.IsHovering(interactable))
            {
                OpenDeleteWindow();
            }
        }
    }

    private void OpenDeleteWindow()
    {
        currentWindow = Instantiate(deleteWindowPrefab, transform);
        currentWindow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
        currentWindow.transform.localPosition = currentWindow.transform.right * -0.5f;
        currentWindow.transform.LookAt(principalHandController.transform);

        openWindow = true;

        if (eventsManager == null)
        {
            eventsManager = FindObjectOfType<EventsManager>();
        }
        eventsManager?.deleteWindowOpen();
    }

    public void CloseDeleteWindow()
    {
        if (currentWindow != null)
        {
            currentWindow.GetComponent<BouncyScaleScript>()?.f_ScaleUpOrDown();
            openWindow = false;

            if (eventsManager == null)
            {
                eventsManager = FindObjectOfType<EventsManager>();
            }
            eventsManager?.deleteWindowClose();
        }
    }
}
