using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabHandler : MonoBehaviour
{
   /* private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // A�adir listeners para los eventos de selecci�n
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnGrab(XRBaseInteractor interactor)
    {
        // Desactivar las constraints de posici�n del Rigidbody
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            Debug.Log("Constraints desactivadas al agarrar el objeto.");
        }
    }

    void OnRelease(XRBaseInteractor interactor)
    {
        // Activar las constraints de posici�n del Rigidbody
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            Debug.Log("Constraints activadas al soltar el objeto.");
        }
    }

    void OnDestroy()
    {
        // Limpiar listeners para evitar referencias colgantes
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.RemoveListener(OnGrab);
            grabInteractable.onSelectExited.RemoveListener(OnRelease);
        }
    }*/
}

