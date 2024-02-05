using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject levelPrefab;
    public int levelNumber;
    public Vector3 startingPosition;
    public bool doAnimation;

    private void Start()
    {
        GameObject.Find("GameMaster").GetComponent<GameMaster>().currentLevelGameObject = transform.gameObject;
        GameObject.Find("GameMaster").GetComponent<GameMaster>().currentLevelNumber = levelNumber;
        startingPosition = transform.position;
        if (doAnimation) transform.position = new Vector3(transform.position.x, -10, transform.position.z);
    }

    private void Update()
    {
        if (transform.position != startingPosition)
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, 5f * Time.deltaTime);
    }
}
