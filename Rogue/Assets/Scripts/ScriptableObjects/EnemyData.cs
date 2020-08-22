using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptables/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string name;
    public string description;
    //sprite
    public Sprite sprite;
    //movement
    public float speedMovement;
    public float speedRotation;
    //other
    public float healthMaximum;

}
