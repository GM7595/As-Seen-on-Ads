using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class OperationWeaponPair
{
    public enum ObjectType { Gate, Weapon }
    public ObjectType leftObjectType;
    public ObjectType rightObjectType;
    public string leftOperation; // Only applicable if leftObjectType is Gate
    public string rightOperation; // Only applicable if rightObjectType is Gate
    public GameObject leftPrefab; // Only applicable if leftObjectType is Weapon
    public GameObject rightPrefab; // Only applicable if rightObjectType is Weapon
}

public class GateManager2 : MonoBehaviour
{
    [Header("Gate Parameters")]
    public GameObject gatePrefab;
    public GameObject weaponPrefab;
    public float gateDistanceInterval = 20f;
    public float gateXDelta = 2.5f;
    public float gateY = 1.75f;

    [Header("Assign Operations/Weapons")]
    public List<OperationWeaponPair> operationWeaponPairs = new List<OperationWeaponPair>();

    void Awake()
    {
        // Create a temporary list to store valid pairs
        List<OperationWeaponPair> validPairs = new List<OperationWeaponPair>();

        // Filter out invalid pairs and populate the temporary list
        foreach (OperationWeaponPair pair in operationWeaponPairs)
        {
            if ((pair.leftObjectType == OperationWeaponPair.ObjectType.Gate && !string.IsNullOrEmpty(pair.leftOperation)) ||
                (pair.leftObjectType == OperationWeaponPair.ObjectType.Weapon && pair.leftPrefab != null) ||
                (pair.rightObjectType == OperationWeaponPair.ObjectType.Gate && !string.IsNullOrEmpty(pair.rightOperation)) ||
                (pair.rightObjectType == OperationWeaponPair.ObjectType.Weapon && pair.rightPrefab != null))
            {
                validPairs.Add(pair);
            }
        }

        // Iterate over the valid pairs to instantiate objects
        for (int i = 0; i < validPairs.Count; i++)
        {
            GameObject parent = Instantiate(new GameObject(), this.transform);
            parent.transform.position = new Vector3(0, 0, i * gateDistanceInterval + gateDistanceInterval);
            parent.name = i.ToString();

            // Instantiate left object
            if (validPairs[i].leftObjectType == OperationWeaponPair.ObjectType.Gate)
            {
                GameObject leftGate = Instantiate(gatePrefab, parent.transform);
                leftGate.transform.position = new Vector3(-gateXDelta, gateY, parent.transform.position.z);
                leftGate.GetComponent<GateScript>().operation = validPairs[i].leftOperation;
            }
            else if (validPairs[i].leftObjectType == OperationWeaponPair.ObjectType.Weapon)
            {
                GameObject leftWeapon = Instantiate(validPairs[i].leftPrefab, parent.transform);
                leftWeapon.transform.position = new Vector3(-gateXDelta, gateY, parent.transform.position.z);
            }

            // Instantiate right object
            if (validPairs[i].rightObjectType == OperationWeaponPair.ObjectType.Gate)
            {
                GameObject rightGate = Instantiate(gatePrefab, parent.transform);
                rightGate.transform.position = new Vector3(gateXDelta, gateY, parent.transform.position.z);
                rightGate.GetComponent<GateScript>().operation = validPairs[i].rightOperation;
            }
            else if (validPairs[i].rightObjectType == OperationWeaponPair.ObjectType.Weapon)
            {
                GameObject rightWeapon = Instantiate(validPairs[i].rightPrefab, parent.transform);
                rightWeapon.transform.position = new Vector3(gateXDelta, gateY, parent.transform.position.z);
            }
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
        foreach (OperationWeaponPair pair in operationWeaponPairs)
        {
            // Calculate the Z-coordinate based on your desired logic
            float zPosition = operationWeaponPairs.IndexOf(pair) * gateDistanceInterval + gateDistanceInterval;

            // Draw and label left object
            if (pair.leftObjectType == OperationWeaponPair.ObjectType.Gate)
            {
                DrawGate(new Vector3(-gateXDelta, gateY, zPosition), pair.leftOperation);
            }
            else if (pair.leftObjectType == OperationWeaponPair.ObjectType.Weapon)
            {
                DrawWeapon(new Vector3(-gateXDelta, gateY, zPosition));
            }

            // Draw and label right object
            if (pair.rightObjectType == OperationWeaponPair.ObjectType.Gate)
            {
                DrawGate(new Vector3(gateXDelta, gateY, zPosition), pair.rightOperation);
            }
            else if (pair.rightObjectType == OperationWeaponPair.ObjectType.Weapon)
            {
                DrawWeapon(new Vector3(gateXDelta, gateY, zPosition));
            }
        }
    }

    private void DrawGate(Vector3 position, string operation)
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(position, new Vector3(0.5f, 0.5f, 0.5f));
        Handles.Label(position, operation, StyleLabel(Color.green, Color.black, Color.white, 16));
    }

    private void DrawWeapon(Vector3 position)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, 1);
    }

    private GUIStyle StyleLabel(Color color, Color bgColor, Color borderColor, int fontSize)
    {
        GUIStyle style = new GUIStyle();
        // Background and border
        Texture2D backgroundTexture = MakeBorderedTex(200, 200, bgColor, borderColor);
        style.normal.background = backgroundTexture;
        // Text Properties
        style.normal.textColor = color;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = fontSize;

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
