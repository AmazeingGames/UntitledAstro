using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To Flare: If you're reading this script and confused what a Scriptable Object is, I would encourage you to watch this video -> https://youtu.be/aPXvoWVabPY - Amazeing
[CreateAssetMenu(fileName = "New Obstacle", menuName = "Obstacle")]
public class ObstacleData : ScriptableObject
{
    [field: SerializeField] public GameObject ObstacleReference { get; private set; }
    [field: SerializeField] public bool ShouldSpin { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public Vector3 ConstantScale { get; private set; } = new Vector3(38, 38, 15);

    [field: SerializeField] public bool MatchScale { get; private set; } = true;


}
