using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonsController : MonoBehaviour
{
public float deadTime = 1.0f;
private bool _deadTimeActive = false;
    private EventsManager eventsManager;
    public void onTriggerEnterDelete() 
    {
        transform.parent.parent.gameObject.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();

        if (eventsManager == null)
        {
            eventsManager = FindFirstObjectByType<EventsManager>();
        }

        eventsManager.deleteBlock(transform.parent.parent.gameObject);
    }
}
