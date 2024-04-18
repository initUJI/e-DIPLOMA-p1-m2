using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : NumberOfBlocks
{
    private int actualLevel;

    [SerializeField] private GameObject Level1;
    [SerializeField] private GameObject Level2;
    [SerializeField] private GameObject Level3;
    [SerializeField] private GameObject Level4;
    [SerializeField] private GameObject Level5;
    [SerializeField] private GameObject Level6;

    private GameManager gameManager;
    public bool alreadyCreated;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        changeLevel(lastLevelCompleted() + 1);
        alreadyCreated = false;
    }
    public int getActualLevel()
    {
        return actualLevel;
    }

    public void changeLevel(int num)
    {
        Block[] blocks = GameObject.FindObjectsOfType<Block>();

        foreach (Block block in blocks)
        {
            if (block.GetType().ToString() != "MainBlock")
            {
                Destroy(block);
            }
        }

        switch (num)
        {
            case 6: blocksForLevel6(); actualLevel = 6; break;
            case 5: blocksForLevel5(); actualLevel = 5; break;
            case 4: blocksForLevel4(); actualLevel = 4; break;
            case 3: blocksForLevel3(); actualLevel = 3; break;
            case 2: blocksForLevel2(); actualLevel = 2; break;
            case 1: blocksForLevel1(); actualLevel = 1; break;
            case 0: blocksForTest(); actualLevel = 0; break;
        }

        createLevel(actualLevel);
    }

    private void createLevel(int num)
    {
        GameObject level = gameObject;

        switch (num)
        {
            case 6: level = Instantiate(Level6); break;
            case 5: level = Instantiate(Level5); break;
            case 4: level = Instantiate(Level4); break;
            case 3: level = Instantiate(Level3); break;
            case 2: level = Instantiate(Level2); break;
            case 1: level = Instantiate(Level1); break;
            default: level = Instantiate(Level1); break;
        }

        level.transform.SetParent(this.transform);
        level.transform.localPosition = Vector3.zero;

        alreadyCreated = true;
        gameManager.NewLevel();
    }

    public void saveCompletedLevel(int num)
    {
        switch (num)
        {
            case 6: PlayerPrefs.SetInt("Level6Completed", 1); break;
            case 5: PlayerPrefs.SetInt("Level5Completed", 1); break;
            case 4: PlayerPrefs.SetInt("Level4Completed", 1); break;
            case 3: PlayerPrefs.SetInt("Level3Completed", 1); break;
            case 2: PlayerPrefs.SetInt("Level2Completed", 1); break;
            case 1: PlayerPrefs.SetInt("Level1Completed", 1); break;
        }
    }

    public bool checkCompletedLevel(int num)
    {
        switch (num)
        {
            case 6: return PlayerPrefs.GetInt("Level6Completed", 0) == 1; 
            case 5: return PlayerPrefs.GetInt("Level5Completed", 0) == 1; 
            case 4: return PlayerPrefs.GetInt("Level4Completed", 0) == 1;
            case 3: return PlayerPrefs.GetInt("Level3Completed", 0) == 1;
            case 2: return PlayerPrefs.GetInt("Level2Completed", 0) == 1;
            case 1: return PlayerPrefs.GetInt("Level1Completed", 0) == 1; 
            default: return false;
        }
    }

    public int lastLevelCompleted()
    {
        if (checkCompletedLevel(6)) return 6;
        else if (checkCompletedLevel(5)) return 5;
        else if (checkCompletedLevel(4)) return 4;
        else if (checkCompletedLevel(3)) return 3;
        else if (checkCompletedLevel(2)) return 2;
        else if (checkCompletedLevel(1)) return 1;
        return 0;
    }
}
