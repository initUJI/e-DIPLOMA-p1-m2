using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GetHumidityBlock : ActionCharacterBlock, WithRightSocket
{
    [SerializeField] protected XRSocketInteractor rightSocket;
    private Plant[] plants;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Execute(Dictionary<string, int> variables)
    {
        plants = GameObject.FindObjectsOfType<Plant>();
        StartCoroutine(c_Execute(variables));
    }

    IEnumerator c_Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        foreach (Plant p in plants)
        {
            if (p.characterInPlant())
            {
                p.humidityChecked = true;

                if (allPlantsChecked())
                {
                    //levelcompleted
                    LevelManager lm = FindObjectOfType<LevelManager>();
                    lm.saveCompletedLevel(lm.getActualLevel());
                }
            }
        }

        yield return new WaitUntil(() => base.IsFinished());
        isFinished = true;
    }

    private bool allPlantsChecked()
    {
        foreach (Plant p in plants)
        {
            if (!p.humidityChecked)
            {
                return false;
            }
        }
        return true;
    }

    public override bool IsFinished()
    {
        return base.IsFinished() && isFinished;
    }
}
