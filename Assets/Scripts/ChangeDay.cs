using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDay : MonoBehaviour
{
    [SerializeField] private float durationDay;

    [SerializeField] private AnimationCurve intensitySun;

    public float timeOfDay;

    private Light sun;

    private void Start()
    {
        sun = GetComponent<Light>();
    }

    private void Update()
    {
        timeOfDay += Time.deltaTime / durationDay;
        if (timeOfDay >= 1) timeOfDay -= 1;

        sun.transform.localRotation = Quaternion.Euler(360f * timeOfDay, 150f, 0f);

        sun.intensity = intensitySun.Evaluate(timeOfDay);
    }
}