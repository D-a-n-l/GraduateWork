using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_360 : MonoBehaviour
{
    [SerializeField] private float speedRotationOnSecond;

    private Transform objectTurning;

    private float timeTurnOver;

    private void Start()
    {
        objectTurning = GetComponent<Transform>();
    }

    private void Update()
    {
        timeTurnOver += Time.deltaTime / speedRotationOnSecond;

        if (timeTurnOver >= 1) timeTurnOver -= 1;

        objectTurning.localRotation = Quaternion.Euler(0f, 360f * timeTurnOver, 0f);
    }
}