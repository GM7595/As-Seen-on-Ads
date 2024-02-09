using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class GateOperationPair
{
    public string leftOperation;
    public string rightOperation;
}

public class GateManager : MonoBehaviour
{
    [Header("Gate Parameters")]
    public GameObject gatePrefab;
    public float gateDistanceInterval = 20f;
    public float gateXDelta = 2.5f;
    public float gateY = 1.75f;

    [Header("Assign Operations")]
    public List<GateOperationPair> gateOperationPairs = new List<GateOperationPair>();

    

    void Awake()
    {
        // Create a temporary list to store valid pairs
        List<GateOperationPair> validPairs = new List<GateOperationPair>();

        // Filter out invalid pairs and populate the temporary list
        foreach (GateOperationPair pair in gateOperationPairs)
        {
            if (pair.leftOperation != "" && pair.rightOperation != "")
            {
                validPairs.Add(pair);
            }
        }

        // Iterate over the valid pairs to instantiate gates
        for (int i = 0; i < validPairs.Count; i++)
        {
            //Make an empty parent for organization
            GameObject parent = Instantiate(new GameObject(), this.transform);
            parent.transform.position = new Vector3(0, 0, i * gateDistanceInterval + gateDistanceInterval);
            parent.name = i.ToString();

            //Make a left and right pair, and assign the operations
            GameObject left = Instantiate(gatePrefab, parent.transform);
            left.transform.position = new Vector3(-gateXDelta, gateY, parent.transform.position.z);
            left.GetComponent<GateScript>().operation = validPairs[i].leftOperation;

            GameObject right = Instantiate(gatePrefab, parent.transform);
            right.transform.position = new Vector3(gateXDelta, gateY, parent.transform.position.z);
            right.GetComponent<GateScript>().operation = validPairs[i].rightOperation;
        }
    }

#if UNITY_EDITOR
    // OnDrawGizmos is called in the editor to draw gizmos
    private void OnDrawGizmos()
    {
        DrawGates();
    }

    // OnDrawGizmosSelected is called only if the object is selected
    private void OnDrawGizmosSelected()
    {
        DrawGates();
    }

    private void DrawGates()
    {
        foreach (GateOperationPair pair in gateOperationPairs)
        {
            Gizmos.color = Color.blue; // Set the gizmo color

            // Calculate the Z-coordinate based on your desired logic
            float gateZ = gateDistanceInterval * gateOperationPairs.IndexOf(pair) + gateDistanceInterval;

            // Draw and label left gate
            Gizmos.DrawWireCube(new Vector3(-gateXDelta, gateY, gateZ), Vector3.one);
            if (!string.IsNullOrEmpty(pair.leftOperation))
            {
                Handles.Label(new Vector3(-gateXDelta, gateY, gateZ), pair.leftOperation, StyleLabel(Color.green, Color.black, Color.white));
            }

            // Draw and label right gate
            Gizmos.DrawWireCube(new Vector3(gateXDelta, gateY, gateZ), Vector3.one);
            if (!string.IsNullOrEmpty(pair.rightOperation))
            {
                Handles.Label(new Vector3(gateXDelta, gateY, gateZ), pair.rightOperation, StyleLabel(Color.green, Color.black, Color.white));
            }
        }
    }

    private GUIStyle StyleLabel(Color color, Color bgColor, Color borderColor)
    {
        GUIStyle style = new GUIStyle();
        // Background and border
        Texture2D backgroundTexture = MakeBorderedTex(50, 50, bgColor, borderColor);
        style.normal.background = backgroundTexture;
        // Text Properties
        style.normal.textColor = color;
        style.alignment = TextAnchor.MiddleCenter;

        return style;
    }

    private Texture2D MakeBorderedTex(int width, int height, Color bgColor, Color borderColor)
    {
        Color[] pix = new Color[width * height];

        // Fill the texture with the border color
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = borderColor;
        }

        // Fill the inner area with the background color
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                pix[y * width + x] = bgColor;
            }
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
#endif

}
