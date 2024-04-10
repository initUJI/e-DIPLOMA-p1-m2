using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [HideInInspector] public GameObject character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            character = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            character = null;
        }
    }

    public bool characterInPlant()
    {
        return character != null;
    }
}
