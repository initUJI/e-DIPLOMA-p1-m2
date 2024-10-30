using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoDestroy : MonoBehaviour
{
    public XRGrabInteractable grabInteractable; // Reference to XRGrabInteractable component
    public float destroyDelay = 0.5f; // Time in seconds before the object is destroyed

    void Start()
    {
        if (grabInteractable != null)
        {
            // Start the coroutine to handle the delay and destruction
            StartCoroutine(DestroyAfterDelay());
        }
        else
        {
            Debug.LogWarning("No XRGrabInteractable component assigned.");
            Destroy(gameObject, destroyDelay); // Just destroy if there's no grab interactable
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        // Disable the XRGrabInteractable component
        grabInteractable.enabled = false;

        // Wait for most of the delay time (e.g., 90% of the delay)
        yield return new WaitForSeconds(destroyDelay - 0.1f);

        // Re-enable the XRGrabInteractable component briefly before destruction
        grabInteractable.enabled = true;

        // Wait a short moment and then destroy the object
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
