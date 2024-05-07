using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deleteConstraints(Rigidbody rg)
    {
        rg.constraints = RigidbodyConstraints.None;
    }

    public void activeConstraints(Rigidbody rg)
    {
        rg.constraints = RigidbodyConstraints.FreezeAll;
    }
}
