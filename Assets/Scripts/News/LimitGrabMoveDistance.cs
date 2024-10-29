using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LimitGrabMoveDistance : MonoBehaviour
{
    public float minDistance = 0.5f; // Distancia m�nima permitida desde la posici�n inicial
    public float maxDistance = 2.0f; // Distancia m�xima permitida desde la posici�n inicial

    private XRGrabInteractable grabInteractable;
    private Vector3 initialPosition; // Posici�n inicial cuando se agarra
    private XRController activeController; // Controlador que est� agarrando el objeto
    private bool isGrabbed = false;

    void Start()
    {
        // Obtiene el XRGrabInteractable y asigna eventos
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        if (isGrabbed && activeController != null)
        {
            // Calcula la distancia desde la posici�n inicial
            float currentDistance = Vector3.Distance(initialPosition, transform.position);

            // Verifica el bot�n MoveObjectIn y MoveObjectOut en el controlador activo
            if (currentDistance >= maxDistance)
            {
                // Si se ha alcanzado la distancia m�xima, desactiva el movimiento hacia adelante
                activeController.moveObjectIn = InputHelpers.Button.None;
            }
            else
            {
                // Si no se ha alcanzado la distancia m�xima, activa el movimiento hacia adelante
                activeController.moveObjectIn = InputHelpers.Button.PrimaryAxis2DUp;
            }

            if (currentDistance <= minDistance)
            {
                // Si se ha alcanzado la distancia m�nima, desactiva el movimiento hacia atr�s
                activeController.moveObjectOut = InputHelpers.Button.None;
            }
            else
            {
                // Si no se ha alcanzado la distancia m�nima, activa el movimiento hacia atr�s
                activeController.moveObjectOut = InputHelpers.Button.PrimaryAxis2DDown;
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Marca el objeto como agarrado, guarda la posici�n inicial y el controlador activo
        isGrabbed = true;
        initialPosition = transform.position;

        // Determina el controlador que est� interactuando
        activeController = args.interactorObject.transform.GetComponent<XRController>();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Restablece el estado al soltar el objeto y reactiva los botones
        isGrabbed = false;

        if (activeController != null)
        {
            activeController.moveObjectIn = InputHelpers.Button.PrimaryAxis2DUp;
            activeController.moveObjectOut = InputHelpers.Button.PrimaryAxis2DDown;
        }

        activeController = null;
    }

    void OnDestroy()
    {
        // Limpia los eventos al destruir el objeto
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
