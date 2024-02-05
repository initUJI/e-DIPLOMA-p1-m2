using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonsController : MonoBehaviour
{
public float deadTime = 1.0f;
private bool _deadTimeActive = false;

    public void onTriggerEnterDelete() 
    {
        transform.parent.parent.gameObject.GetComponent<BouncyScaleScript>().f_ScaleUpOrDown();
    }
}
