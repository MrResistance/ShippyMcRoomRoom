using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptables/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string name;
    public string description;
    //sprite
    [PreviewField(75)]
    public Sprite sprite;
    public Color colourSprite;
    public float scale; //for 
    //movement
    public float speedMovement;
    public float speedRotation;
    //other
    public float healthMaximum;

}
