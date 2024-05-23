using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCameraController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log(GameManager.character);
        if (GameManager.character != null) {
            transform.position = GameManager.character.gameObject.transform.position + new Vector3(0f, 35f, 0f); 
        }
        
    }
}
