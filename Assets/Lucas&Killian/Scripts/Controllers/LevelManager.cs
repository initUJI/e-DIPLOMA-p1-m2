using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : NumberOfBlocks
{
    // Start is called before the first frame update
    void Start()
    {
        changeLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            case 6: blocksForLevel6(); break;
            case 5: blocksForLevel5(); break;
            case 4: blocksForLevel4(); break;
            case 3: blocksForLevel3(); break;
            case 2: blocksForLevel2(); break;
            case 1: blocksForLevel1(); break;
        }
    }
}
