using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int WaveNumber;
    public int remainingEnemies;
    public bool isWaveChanging;
    void Start()
    {
        NextWave();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaveUpdate()
    {
        while (true)
        {
            if (remainingEnemies == 0 && isWaveChanging == false)
            {
                isWaveChanging = true;
                NextWave();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void NextWave()
    {
        WaveNumber++;
        this.gameObject.GetComponent<GameUIManager>().ChangeWaveNumber(WaveNumber);
        remainingEnemies += 1;
        isWaveChanging = false; 

    }
}
