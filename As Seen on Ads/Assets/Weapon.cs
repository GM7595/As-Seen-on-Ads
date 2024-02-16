using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float slowdownFactor = 0.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;
    public float maxTurnAngle = 30f;
    public float snapBackSpeed = 10f;

    [Header("Shooting Settings")]
    public float fireRate = 10f;
}