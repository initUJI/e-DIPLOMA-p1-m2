using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using UnityEngine.WSA;
//using UnityEngine.XR.Interaction.Toolkit;

public class ForBlock : ConditionalBlock
{
    bool forIsFinished;
    Coroutine forCoroutine;

    public override void Start()
    {
        base.Start();
        forIsFinished = false;
    }
    public override void Execute(Dictionary<string, int> variables)
    {
        this.variables = variables;

        if (forCoroutine != null)
        {
            StopCoroutine(forCoroutine);
        }

        forCoroutine = StartCoroutine(c_ExecuteFor());
    }

    public IEnumerator c_ExecuteFor()
    {
        forIsFinished = false;

        while (conditionChecked(variables))
        {
            Debug.Log("Condition vérifiée !");

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Launch and wait the end of c_Execute
            yield return (currentCoroutine = StartCoroutine(c_Execute()));
        }

        forIsFinished = true;
    }

    public override bool IsFinished()
    {
        return forIsFinished;
    }

}
