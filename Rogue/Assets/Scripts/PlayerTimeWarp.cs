using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeWarp : MonoBehaviour
{
    public AnimationCurve curveX, curveY;
    public Keyframe[] keyframesX;
    public Keyframe[] keyframesY;
    int curveTime;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Timewarp();
        }
    }

    public void Timewarp() //Main method - Time warps player
    {
        int ReturnNumber = GetReturnTime();
        transform.position = new Vector3(keyframesX[ReturnNumber].value, keyframesY[ReturnNumber].value, 0);

    }
    void StartCurve() //For creating curve
    {
        Debug.Log("Key count:" + curveX.keys.Length);
        for (int i = 1; i < 11; i++)
        {
            curveX.AddKey(i, transform.position.x);
            curveY.AddKey(i, transform.position.x);
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
                curveTime = 1;
            }
            
            keyframesX[curveTime].value = (int)transform.position.x;
            keyframesY[curveTime].value = (int)transform.position.y;
            curveX.keys = keyframesX;
            curveY.keys = keyframesY;

            yield return new WaitForSeconds(1.0f);


        }
    }
    int GetReturnTime()
    {
        int number = 1;

        return number;
    }
}
