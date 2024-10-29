using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketsControl : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor[] sockets; // Sockets asignados manualmente o detectados en el objeto y sus hijos
    private XRGrabInteractable _interactable;

    void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();

        // Obtener todos los sockets en los hijos y combinarlos con los asignados manualmente
        List<XRSocketInteractor> allSockets = new List<XRSocketInteractor>(GetComponentsInChildren<XRSocketInteractor>());

        if (sockets != null)
        {
            foreach (var socket in sockets)
            {
                if (!allSockets.Contains(socket))
                {
                    allSockets.Add(socket);
                }
            }
        }

        sockets = allSockets.ToArray();

        // Asignar los eventos solo para sockets, no para el interactor de "mano"
        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(OnObjectInserted);
            socket.selectExited.AddListener(OnObjectRemoved);
        }

        // Verificamos si el objeto fue agarrado o soltado con un interactor de "mano"
        if (_interactable != null)
        {
            _interactable.selectEntered.AddListener(args =>
            {
                // Solo activar `OnGrabbed` si es un interactor de "mano" (por ejemplo, XRRayInteractor)
                if (args.interactorObject is XRRayInteractor)
                {
                    OnGrabbed(args);
                }
            });

            _interactable.selectExited.AddListener(args =>
            {
                // Solo activar `OnReleased` si es un interactor de "mano" (por ejemplo, XRRayInteractor)
                if (args.interactorObject is XRRayInteractor)
                {
                    OnReleased(args);
                }
            });
        }
    }


    // Devuelve los sockets que se encuentran en los hijos
    public XRSocketInteractor[] GetSockets()
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
    public void ActivateSockets()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.gameObject.SetActive(true);
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

        //Debug.Log("Objeto insertado correctamente.");
    }

    // Evento cuando se retira un objeto de un socket
    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        IXRSelectInteractable removedObject = args.interactableObject;
        //Debug.Log("Objeto removido del socket.");
    }

    // Función para comprobar si el objeto ya está en otro socket (excluyendo el socket actual)
    private bool IsObjectInAnySocket(IXRSelectInteractable interactable, XRSocketInteractor currentSocket)
    {
        foreach (var socket in sockets)
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

        return false;
    }

    // Evento cuando se agarra el objeto
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        DeactivateSockets();
    }

    // Evento cuando se suelta el objeto
    private void OnReleased(SelectExitEventArgs args)
    {
       /* XRSocketInteractor socket = args.interactorObject as XRSocketInteractor;

        if (socket != null && socket.hasSelection )
        {
            Debug.LogWarning("El socket está ocupado, liberando el objeto correctamente.");
        }*/

        ActivateSockets();
    }
}