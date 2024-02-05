using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectBlock : Block, WithAssociatedString
{

    [SerializeField] string associatedString;
    public string getAssociatedString()
    {
        return associatedString;
    }

    public override void Start()
    {
        base.Start();
        TMP_Text textMeshPro = GetComponentInChildren<TMP_Text>();
        textMeshPro.text = associatedString;
    }
}
