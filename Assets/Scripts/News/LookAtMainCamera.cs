using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LookAtMainCamera : MonoBehaviour
{
    public float targetYRotation = 0f; // Rotaci�n adicional en el eje Y (0, 90, 180, etc.)

    private Transform mainCameraTransform;
    private Quaternion initialRotation; // Rotaci�n inicial en el mundo cuando se spawnea
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    void Start()
    {
        // Obt�n la referencia a la c�mara principal y guarda la rotaci�n inicial del objeto
        mainCameraTransform = Camera.main.transform;
        initialRotation = transform.rotation;

        // Configura los eventos de agarre en el XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        // Solo aplica la rotaci�n hacia la c�mara mientras est� agarrado
        if (isGrabbed)
        {
            // Calcula la direcci�n hacia la c�mara en el plano XZ (ignorando el eje Y)
            Vector3 directionToCamera = mainCameraTransform.position - transform.position;
            directionToCamera.y = 0; // Mantiene la altura constante

            // Si la direcci�n es v�lida, calcula la rotaci�n hacia la c�mara
            if (directionToCamera != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

                // Aplica la rotaci�n adicional en el eje Y definida en targetYRotation
                targetRotation *= Quaternion.Euler(0, targetYRotation, 0);

                // Mant�n la rotaci�n inicial en X y Z, y ajusta solo el eje Y
                transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, targetRotation.eulerAngles.y, initialRotation.eulerAngles.z);
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Activa el comportamiento de mirar a la c�mara al agarrar
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Desactiva el comportamiento manteniendo la rotaci�n final al soltar
        isGrabbed = false;
    }

    void OnDestroy()
    {
        // Limpia los eventos cuando se destruye el objeto
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
