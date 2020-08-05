using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoverScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve curveX, curveY;
    float timeElapsed;
    bool isMoving;
    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            isMoving = true;
            timeElapsed = 0f;
        }
        else
        {
            if (timeElapsed > curveX.length)
            {
                isMoving = false;
            }
            else
            {
                timeElapsed += Time.deltaTime;
                transform.position = new Vector2(curveX.Evaluate(timeElapsed), curveY.Evaluate(timeElapsed));
                transform.rotation = Quaternion.Euler(0, 0, 360 * curveX.Evaluate(timeElapsed));
            }
        }

    }
}
