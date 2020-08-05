using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeWarp : MonoBehaviour
{
    public AnimationCurve curveX, curveY;
    public Keyframe[] keyframesX;
    public Keyframe[] keyframesY;
    public int curveTime;
    public Transform transform;
    
    void Start()
    {
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();

        transform = GetComponent<Transform>();
        StartCurve();
        curveTime = 0;
        StartCoroutine(ChangeKey());
    }
    
    void Update() //For player input
    {
        if (Input.GetKeyDown("space"))
        {
            Timewarp();
        }
    }

    public void Timewarp() //Main method - Time warps player
    {
        int ReturnNumber = GetReturnTime(2);
        transform.position = new Vector3(keyframesX[ReturnNumber].value, keyframesY[ReturnNumber].value, 0);

    }
    void StartCurve() //For creating curve
    {
        
        for (int i = 0; i < 10; i++)
        {
            curveX.AddKey(i, transform.position.x);
            curveY.AddKey(i, transform.position.y);
            Debug.Log("Key count:" + curveX.keys.Length);

        }
        keyframesX = curveX.keys;
        keyframesY = curveY.keys;

    }
    IEnumerator ChangeKey()
    {
        while (true)
        {
            if (curveTime != 9)
            {
                curveTime++;
            }
            else
            {
                curveTime = 0;
            }
            
            keyframesX[curveTime].value = (int)transform.position.x;
            keyframesY[curveTime].value = (int)transform.position.y;
            curveX.keys = keyframesX;
            curveY.keys = keyframesY;

            yield return new WaitForSeconds(1.0f);


        }
    }
    int GetReturnTime(int HowFarGoBack)
    {
        Debug.Log("How far go back:" + HowFarGoBack.ToString() + ",curveTime" + curveTime.ToString());
        int number = 1;
        if (HowFarGoBack > curveTime)
        {
            number = (curveTime - HowFarGoBack) + 10;
        }
        else
        {
            number = curveTime - HowFarGoBack;
        }
        Debug.Log("Return number:" + number.ToString());
        return number;
    }
}
