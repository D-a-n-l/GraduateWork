using System.Collections;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class SitOnPC : MonoBehaviour
{
    [Space(5)]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string nameAnimation = "isOn";

    [Space(10)]
    [SerializeField]
    private MoveController player;

    [Space(10)]
    [SerializeField]
    private RaycastControl raycast;

    [Space(10)]
    [SerializeField]
    private string textSit;

    [SerializeField]
    private string textStand;

    [Space(10)]
    [SerializeField]
    private GameObject crosshair;

    [Space(10)]
    [SerializeField]
    private float timeBlendWhenSitOrStand;

    [Space(10)]
    [SerializeField]
    private CinemachineVirtualCamera mainVirtualCamera;

    [Space(10)]
    [SerializeField]
    private CinemachineVirtualCamera supportVirtualCamera;

    [SerializeField] 
    private Vector3 supportVirtualCameraPosition;

    [SerializeField]
    private Vector3 supportVirtualCameraRotation;

    private CinemachinePOV mainPOV;

    private float mainValueX;

    private float mainValueY;

    private float mainValueHorizontalMin;

    private float mainValueHorizontalMax;

    private float mainValueVerticalMin;

    private float mainValueVerticalMax;

    private bool yes = true;

    private void Start()
    {
        mainPOV = mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

        if (mainPOV == null)
            throw new UnassignedReferenceException();
    }

    public void Sit()
    {
        SitOrStand(false, true, nameAnimation, true);
        CanUse(false, textStand, timeBlendWhenSitOrStand);
    }

    public void Stand()
    {
        SitOrStand(true, false, null, true);
        CanUse(true, textSit, timeBlendWhenSitOrStand);
    }

    public void StandAndOffLaptop()
    {
        SitOrStand(true, false, nameAnimation, false, true);
        CanUse(true, textSit, timeBlendWhenSitOrStand);
    }

    private void CanUse(bool use, string textTip, float timeOfDelay)
    {
        StartCoroutine(UseCan(use, textTip, timeOfDelay));
    }

    private IEnumerator UseCan(bool use, string textTip, float timeOfDelay)
    {
        if (use == true)
        {
            yield return new WaitForSeconds(timeOfDelay);

            mainPOV.m_HorizontalAxis.m_MinValue = mainValueHorizontalMin;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueHorizontalMax;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueVerticalMin;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueVerticalMax;

            StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
        }
        else
        {
            mainPOV.m_HorizontalAxis.m_MinValue = mainValueX;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueX;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueY;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueY;

            yield return new WaitForSeconds(timeOfDelay);

            mainPOV.m_HorizontalAxis.m_MinValue = mainValueHorizontalMin;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueHorizontalMax;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueVerticalMin;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueVerticalMax;
        }
    }

    private void SitOrStand(bool mainVCam, bool sit, string id, bool enabled, bool offLaptop = false)
    {
        mainVirtualCamera.gameObject.SetActive(mainVCam);

        supportVirtualCamera.gameObject.SetActive(!mainVCam);

        supportVirtualCamera.transform.SetPositionAndRotation(supportVirtualCameraPosition, 
            Quaternion.Euler(supportVirtualCameraRotation));

        mainValueX = mainPOV.m_HorizontalAxis.Value;
        mainValueY = mainPOV.m_VerticalAxis.Value;

        if (sit == true)
        {
            if(yes == true)
            {
                mainValueHorizontalMin = mainPOV.m_HorizontalAxis.m_MinValue;
                mainValueHorizontalMax = mainPOV.m_HorizontalAxis.m_MaxValue;
            
                mainValueVerticalMin = mainPOV.m_VerticalAxis.m_MinValue;
                mainValueVerticalMax = mainPOV.m_VerticalAxis.m_MaxValue;

                yes = false;
            }

            raycast.TimeBlendCinemachine(timeBlendWhenSitOrStand);

            StartCoroutine(raycast.CanStand(0f, true, true));

            StartCoroutine(CursorControl.Control(timeBlendWhenSitOrStand, CursorLockMode.Confined, true, 1));
         }
        else
        {
            mainPOV.m_HorizontalAxis.m_MinValue = mainValueX;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueX;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueY;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueY;

            mainPOV.m_HorizontalAxis.Value = mainValueX;
            mainPOV.m_VerticalAxis.Value = mainValueY;

            mainPOV.m_HorizontalAxis.m_Wrap = false;

            StartCoroutine(raycast.CanStand(timeBlendWhenSitOrStand, true, false));

            StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
        }

        crosshair.SetActive(mainVCam);

        animator.SetBool(id, enabled);
    }
}