using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketsControl : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor[] sockets; // Sockets assigned manually or detected in the object and its children
    private XRGrabInteractable _interactable;

    private XRSocketInteractor currentSocket; // The socket currently holding this interactable

    void Start()
    {
        _interactable = GetComponent<XRGrabInteractable>();

        // Get all sockets in the children and combine them with manually assigned ones
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

        // Assign events only for sockets, not for hand interactors
        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(OnObjectInserted);
            socket.selectExited.AddListener(OnObjectRemoved);
        }

        // Check if the object was grabbed or released with a hand interactor
        if (_interactable != null)
        {
            _interactable.selectEntered.AddListener(args =>
            {
                if (args.interactorObject is XRRayInteractor)
                {
                    OnGrabbed(args);
                }
            });

            _interactable.selectExited.AddListener(args =>
            {
                if (args.interactorObject is XRRayInteractor)
                {
                    OnReleased(args);
                }
            });
        }
    }

    // Returns sockets found in the children
    public XRSocketInteractor[] GetSockets()
    {
        return sockets;
    }

    // Deactivates all sockets in the children
    public void DeactivateSockets()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.gameObject.SetActive(false);
        }
    }

    // Activates empty sockets in the children when the object is released
    public void ActivateSockets()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.gameObject.SetActive(true);
        }
    }

    // Toggles sockets depending on whether an XRRayInteractor is used
    public void ToggleSockets(SelectEnterEventArgs args)
    {
        bool shouldActivate = args.interactorObject.transform.GetComponent<XRRayInteractor>() ? false : true;

        foreach (XRSocketInteractor socket in sockets)
        {
            if (socket.firstInteractableSelected == null)
            {
                socket.gameObject.SetActive(shouldActivate);
            }
        }
    }

    // Event when an object is inserted into a socket
    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        IXRSelectInteractable insertedObject = args.interactableObject;

        // Check if the object is already in another socket
        if (IsObjectInAnySocket(insertedObject, args.interactorObject as XRSocketInteractor))
        {
            //Debug.LogWarning("The object is already in another socket. Cancelling insertion...");

            XRSocketInteractor socket = args.interactorObject as XRSocketInteractor;
            socket.interactionManager.CancelInteractableSelection(insertedObject);
            return;
        }

        // Store the socket this interactable is currently in
        currentSocket = args.interactorObject as XRSocketInteractor;
       // Debug.Log("Object inserted into socket: " + currentSocket.name);
    }

    // Event when an object is removed from a socket
    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        IXRSelectInteractable removedObject = args.interactableObject;

        // Clear the currentSocket reference since the object has been removed
        if (currentSocket == args.interactorObject as XRSocketInteractor)
        {
            //Debug.Log("Object removed from socket: " + currentSocket.name);
            currentSocket = null;
        }
    }

    // Function to check if the object is already in another socket (excluding the current one)
    private bool IsObjectInAnySocket(IXRSelectInteractable interactable, XRSocketInteractor currentSocket)
    {
        foreach (var socket in sockets)
        {
            if (socket == currentSocket) continue;

            if (socket.hasSelection && socket.firstInteractableSelected != null &&
                socket.firstInteractableSelected.transform == interactable.transform)
            {
                return true;
            }
        }

        return false;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {

        // If the object is currently in a socket, remove it
        if (currentSocket != null)
        {
            //Debug.Log("Temporarily disabling and re-enabling socket: " + currentSocket.name);

            // Start the coroutine to disable and re-enable the socket
            StartCoroutine(TemporarilyDisableSocket(currentSocket));

            // Clear the currentSocket reference since the object is no longer in the socket
            currentSocket = null;
        }

        DeactivateSockets();
    }

    // Coroutine to disable and re-enable the socket with a delay
    private IEnumerator TemporarilyDisableSocket(XRSocketInteractor socket)
    {
        socket.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        socket.gameObject.SetActive(true);

        //Debug.Log("Socket re-enabled: " + socket.name);
    }



    // Event when the object is released
    private void OnReleased(SelectExitEventArgs args)
    {
        ActivateSockets();
    }
}
