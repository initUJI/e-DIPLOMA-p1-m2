using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject[] levelStars;
    public GameObject[] levels;
    public GameObject currentLevelGameObject;
    public int currentLevelNumber;

    public void StageBeaten(int i)
    {
        levelStars[i-1].SetActive(true);
    }
}
