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
        float marginright = 1.5f;
        float marginforward = 0.4f;

        // Instanciar la ventana y posicionarla
        currentWindow = Instantiate(deleteWindowPrefab, transform);
        currentWindow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
        BoxCollider parentBoxCollider = transform.GetComponent<BoxCollider>();

        // Mueve la ventana hacia la derecha en función del ancho del BoxCollider del padre
        currentWindow.transform.localPosition = currentWindow.transform.right * (-parentBoxCollider.size.x + marginright);
        currentWindow.transform.localPosition = currentWindow.transform.forward * (-parentBoxCollider.size.z + marginforward);

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
