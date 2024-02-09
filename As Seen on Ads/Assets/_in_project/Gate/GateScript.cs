using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateScript : MonoBehaviour
{
    [Header("Operation Settings")]
    public string operation = "sin(HP)";
    public bool doDebug = false;

    [Header("Previews (Don't edit)")]
    [SerializeField] string expressionPreview;
    TextMeshProUGUI textComp;

    // Start is called before the first frame update
    void Start()
    {
        DoMath();
        textComp = GetComponentInChildren<TextMeshProUGUI>();
        textComp.text = operation;
    }

    public float DoMath()
    {
        expressionPreview = MyFunctions.FormatExpression(operation, PlayerScript.playerHP);
        float result = MyFunctions.EvaluateExpression(expressionPreview);
        if(doDebug == true)
            Debug.Log("[Operation]\n" + operation + " \n [Result]" + result);
        return result;
    }

    public void Destroy()
    {
        Debug.Log("[GATE]\nPassed: " + transform.parent.name);
        Destroy(transform.parent.gameObject);
    }
}

