using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsManager : MonoBehaviour
{
    public void deleteConstraints(Rigidbody rg)
    {
        rg.constraints = RigidbodyConstraints.None;
        rg.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void activeConstraints(Rigidbody rg)
    {

        rg.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void resetRotation()
    {
        //transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, 0);
    }
}
