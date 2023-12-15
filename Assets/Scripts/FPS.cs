using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private float score;

    private void Update()
    {
        score = (int)(1f / Time.unscaledDeltaTime);

        Debug.Log(score);
    }
}
