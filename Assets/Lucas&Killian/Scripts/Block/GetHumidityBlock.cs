using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GetHumidityBlock : ActionCharacterBlock, WithRightSocket
{
    [SerializeField] protected XRSocketInteractor rightSocket;
    private Plant plant;

    public XRSocketInteractor getRightSocket()
    {
        return rightSocket;
    }

    public override void Execute(Dictionary<string, int> variables)
    {
        plant = GameObject.FindFirstObjectByType<Plant>();
        StartCoroutine(c_Execute(variables));
    }

    IEnumerator c_Execute(Dictionary<string, int> variables)
    {
        isFinished = false;

        if (plant.characterInPlant())
        {
            //completed
        }

        yield return new WaitUntil(() => base.IsFinished());
        isFinished = true;
    }

    public override bool IsFinished()
    {
        return base.IsFinished() && isFinished;
    }
}
