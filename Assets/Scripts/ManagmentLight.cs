using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagmentLight : MonoBehaviour
{
    [SerializeField] private GameObject algoritmLight;

    [SerializeField] private GameObject mathfLight;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Playerr"))
        {
            LightOnOff(false);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Playerr"))
        {
            LightOnOff(false);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Playerr"))
        {
            LightOnOff(true);
        }
    }

    private void LightOnOff(bool on)
    {
        algoritmLight.SetActive(on);
        mathfLight.SetActive(on);
    }
}