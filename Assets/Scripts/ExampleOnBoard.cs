using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ExampleOnBoard : MonoBehaviour
{
    [SerializeField] 
    private InputField answerA;

    [SerializeField] 
    private CinemachineVirtualCamera mainVirtualCamera;

    [SerializeField] 
    private RaycastControl e;

    [SerializeField] 
    private string answerB;

    private void Start()
    {
        answerA.ActivateInputField();
    }

    public void Example()
    {
        if(answerA.text == answerB && Input.GetKeyDown(KeyCode.Return))
        {
            ConditionExample(1);
        }
        else if (answerA.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            ConditionExample(-1);
        }
    }

    private void ConditionExample(int value)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "Mouse X";
        mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "Mouse Y";

        Destroy(gameObject);
    }
}