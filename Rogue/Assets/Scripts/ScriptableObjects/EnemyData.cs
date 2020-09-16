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
    //health
    public float healthMaximum;
    //shield
    public float shieldMaximum;
    public float shieldRechargeRate; //Amount of shield that is returned
    //scoring
    public float points;

    //test
    //[ValueDropdown("testValues")]
    //public string test;
    
    /*private ValueDropdownList<string> testValues = new ValueDropdownList<string>()
    {
        {"Test","one"},
        {"Test2","two"},
        {"Test3","three"},
    };*/

    public GameObject explosionPrefab;

    }

