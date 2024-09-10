using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketsControl : MonoBehaviour
{
    private XRSocketInteractor[] sockets; // Todos los sockets dentro del objeto y sus hijos
    private XRGrabInteractable _interactable;

    void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        // Obtenemos todos los sockets en los hijos
        sockets = GetComponentsInChildren<XRSocketInteractor>();

        // Asignamos los eventos para cuando el objeto es agarrado o soltado
        _interactable.selectEntered.AddListener(OnGrabbed);
        _interactable.selectExited.AddListener(OnReleased);
    }

    // Devuelve los sockets que se encuentran en los hijos
    public XRSocketInteractor[] getSockets()
    {
        return sockets;
    }

    // Desactiva todos los sockets en los hijos
    public void DeactivateSockets()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.gameObject.SetActive(false); // Desactiva el objeto del socket
        }
    }

    // Activa los sockets vacíos en los hijos cuando se suelta el objeto
    public void ActivateSockets(SelectExitEventArgs args)
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            if (socket.firstInteractableSelected == null) // Solo activa los sockets vacíos
            {
                socket.gameObject.SetActive(true);
            }
        }
    }

    // Activa o desactiva los sockets dependiendo de si se está usando un XRRayInteractor
    public void ToggleSockets(SelectEnterEventArgs args)
    {
        bool shouldActivate = args.interactorObject.transform.GetComponent<XRRayInteractor>() ? false : true;

        foreach (XRSocketInteractor socket in sockets)
        {
            if (socket.firstInteractableSelected == null) // Solo manipula sockets vacíos
            {
                socket.gameObject.SetActive(shouldActivate);
            }
        }
    }

    // Evento cuando se agarra el objeto
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        ToggleSockets(args);
    }

    // Evento cuando se suelta el objeto
    private void OnReleased(SelectExitEventArgs args)
    {
        ActivateSockets(args);
    }
}
