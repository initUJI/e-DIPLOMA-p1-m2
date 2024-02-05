using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Events;
//using UnityEngine.InputSystem;
//using UnityEngine.iOS;
//using UnityEngine.XR.OpenXR.Input;

public class DeleteWindowController : MonoBehaviour
{
    public GameObject deleteWindowPrefabs;
    private GameObject deleteWindow;
    public XRRayInteractor principalInteractor;
    public XRRayInteractor secondaryInteractor;
    //public GameObject block;

    private bool isInstantiate;
    private int i;



    // Start is called before the first frame update
    void Start()
    {
        GameManager.InvokeAfterInit(this, () =>
        {
            principalInteractor = GameManager.principalHandController.GetComponent<XRRayInteractor>();
            secondaryInteractor = GameManager.secondaryHandController.GetComponent<XRRayInteractor>();

        });

        isInstantiate = false;
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (principalInteractor != null && principalInteractor.IsHovering(GetComponent<XRGrabInteractable>()) && secondaryInteractor.IsHovering(GetComponent<XRGrabInteractable>())) {
            if (!isInstantiate) {
                Debug.Log(i+1);
                deleteWindow = Instantiate(deleteWindowPrefabs, transform);
                deleteWindow.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z + 0.25f);
                isInstantiate = true;
            }
        }
    }
    public void Delete()
    {
        Debug.Log(FindChildDeleteWindow());
        Destroy(FindChildDeleteWindow().parent.gameObject);
    }

    public void Cancel()
    {
        isInstantiate = false;
        Destroy(FindChildDeleteWindow().gameObject);
    }

    Transform FindChildDeleteWindow()
    {
        Transform windowToDelete = null;
        foreach (Transform child in transform){
            if (child.CompareTag("DeleteWindow")){
               windowToDelete = child;
               break;
            }
        }
        return windowToDelete;
    }
    

}