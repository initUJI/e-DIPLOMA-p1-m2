using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CanBeDeleted : MonoBehaviour
{
    public XRController principalHandController;
    XRRayInteractor principalRay;
    public XRBaseInteractable interactable;
    public bool primaryButtonValue;
    public bool openWindow;
    public GameObject currentWindow;

    [SerializeField] GameObject deleteWindowPrefab;
    private EventsManager eventsManager;

    private void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        currentWindow = null;
        openWindow = false;

        GameManager.InvokeAfterInit(this, () =>
        {
            principalHandController = GameManager.principalHandController;
            if (principalHandController != null)
            {
                principalRay = principalHandController.GetComponent<XRRayInteractor>();
            }   
        });       
    }

    private void Update()
    {
        
        if (interactable != null && principalHandController != null && principalRay != null)
        {
            InputDevice device = principalHandController.inputDevice;
            if (device != null && device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue))
            {
                if (principalRay.IsHovering(interactable) && primaryButtonValue && !openWindow)
                {
                    // faire apparaitre la fenetre si elle n'existe pas
                    currentWindow = Instantiate(deleteWindowPrefab, transform);
                    currentWindow.transform.parent = transform;
                    currentWindow.transform.localPosition = new Vector3(0, transform.position.y + 0.5f, transform.position.z - 0.5f);

                    openWindow = true;
                    Debug.Log("Ouverture !");

                    if (eventsManager == null)
                    {
                        eventsManager = FindFirstObjectByType<EventsManager>();
                    }

                    eventsManager.deleteWindowOpen();
                } else if(!primaryButtonValue && openWindow)
                {
                    // fermer la fenetre
                    currentWindow.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();
                    //Destroy(currentWindow);
                    Debug.Log("Fermeture !");
                    openWindow = false;

                    if (eventsManager == null)
                    {
                        eventsManager = FindFirstObjectByType<EventsManager>();
                    }

                    eventsManager.deleteWindowClose();
                }
            }
        }
    }
}
