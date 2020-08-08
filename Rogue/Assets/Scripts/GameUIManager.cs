using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI t_wavenumber;
    void Start()
    {
        InvokeRepeating("Pond", 2f, 3f);
    }
    void Pond()
    {
        Debug.Log("Honlo, time is: " + Time.time);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeWaveNumber(int number)
    {
        if (number != 0)
        {
            t_wavenumber.text = "Wave " + number.ToString();
        }
        else
            t_wavenumber.text = "Boss Level";

    }
}
