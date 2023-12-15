using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManagment : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void ResetsButton(int timeOfDelay)
    {
        StartCoroutine(Delay(timeOfDelay));
    }

    private IEnumerator Delay(int timeOfDelay)
    {
        button.interactable = false;

        yield return new WaitForSeconds(timeOfDelay);

        button.interactable = true;
    }
}