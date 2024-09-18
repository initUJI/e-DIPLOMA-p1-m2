using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketsControl : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor[] sockets; // Todos los sockets dentro del objeto y sus hijos
    private XRGrabInteractable _interactable;

    void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();

        // Obtenemos todos los sockets en los hijos si no se asignaron manualmente
        if (sockets == null || sockets.Length == 0)
        {
            sockets = GetComponentsInChildren<XRSocketInteractor>();
        }

        // Asignamos los eventos para cuando el objeto es agarrado o soltado
        _interactable.selectEntered.AddListener(OnGrabbed);
        _interactable.selectExited.AddListener(OnReleased);

        // Asignamos la validación personalizada para cada socket
        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(OnObjectInserted); // Evento cuando se inserta un objeto
            socket.selectExited.AddListener(OnObjectRemoved);   // Evento cuando se retira un objeto
        }
    }

    // Devuelve los sockets que se encuentran en los hijos
    public XRSocketInteractor[] getSockets()
    {
        return sockets;
    }

    // Desactiva todos los sockets en los hijos
    public void DeactivateSockets()
    {
        if (sockets == null)
        {
            sockets = GetComponentsInChildren<XRSocketInteractor>();
        }
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.gameObject.SetActive(false); // Desactiva el objeto del socket
        }
    }

    // Activa los sockets vacíos en los hijos cuando se suelta el objeto
    public void ActivateSockets()
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

    // Evento cuando se inserta un objeto en un socket
    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        IXRSelectInteractable insertedObject = args.interactableObject;

        // Comprobamos si el objeto ya está en otro socket
        if (IsObjectInAnySocket(insertedObject, args.interactorObject as XRSocketInteractor))
        {
            Debug.LogWarning("El objeto ya está en otro socket. Cancelando la inserción...");

            // Cancelar la inserción en este socket utilizando interactionManager
            XRSocketInteractor socket = args.interactorObject as XRSocketInteractor;
            socket.interactionManager.CancelInteractableSelection(insertedObject);
            return;
        }

        Debug.Log("Objeto insertado correctamente.");
    }

    // Evento cuando se retira un objeto de un socket
    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        IXRSelectInteractable removedObject = args.interactableObject;
        Debug.Log("Objeto removido del socket.");
    }

    // Función para comprobar si el objeto ya está en otro socket (excluyendo el socket actual)
    private bool IsObjectInAnySocket(IXRSelectInteractable interactable, XRSocketInteractor currentSocket)
    {
        // Recorremos todos los sockets en la escena
        XRSocketInteractor[] allSockets = FindObjectsOfType<XRSocketInteractor>();

        foreach (var socket in allSockets)
        {
            // Ignorar el socket actual para evitar que bloquee su propia inserción
            if (socket == currentSocket)
                continue;

            // Comparamos los transforms de los objetos en lugar de las referencias directas
            if (socket.hasSelection && socket.firstInteractableSelected != null &&
                socket.firstInteractableSelected.transform == interactable.transform)
            {
                return true;
            }
        }

        // Si no está en ningún otro socket, devolvemos false
        return false;
    }

    // Evento cuando se agarra el objeto
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        ToggleSockets(args);
    }

    // Evento cuando se suelta el objeto
    private void OnReleased(SelectExitEventArgs args)
    {
        XRSocketInteractor socket = args.interactorObject as XRSocketInteractor;

        if (socket != null && socket.hasSelection && socket.firstInteractableSelected != null)
        {
            Debug.LogWarning("El socket está ocupado, liberando el objeto correctamente.");
        }

        ActivateSockets();
    }
}
