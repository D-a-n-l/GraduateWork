using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class StandOnTable : MonoBehaviour
{
    [Space(10)]
    [SerializeField]
    private MoveController player;

    [Space(10)]
    [SerializeField]
    private RaycastControl raycastControl;

    [Min(0.01f), Space(10)]
    [SerializeField]
    private float timeWhenCanSitDown = 1.5f;

    [Space(10)]
    [SerializeField]
    private string animationTriggerForStandUp;

    [Space(10)]
    [SerializeField]
    private CinemachineVirtualCamera mainVirtualCamera;

    [Space(10)]
    [SerializeField]
    private float angleWhenStandUp;

    private float defaultMinValueView;

    private float defaultMaxValueView;

    //private void Start()
    //{
    //   if (player == null || raycastControl == null || animationTriggerForStandUp == null || mainVirtualCamera == null)
    //        throw new UnassignedReferenceException();

    //    if (mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>())
    //    {
    //        defaultMinValueView = mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MinValue;

    //        defaultMaxValueView = mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxValue;
    //    }
    //    else
    //        throw new UnassignedReferenceException();
    //}

    public void StandUp(Vector3 lastPositionPlayer, SitOnChair chair)
    {
        StartCoroutine(player.ChangeParameters(animationTriggerForStandUp, false, false, timeWhenCanSitDown, true));

        player.transform.position = lastPositionPlayer;

        VirtualCameraControl.Control(mainVirtualCamera, angleWhenStandUp, defaultMinValueView, defaultMaxValueView, true);
        
        chair.PutHadns(false);

        StartCoroutine(chair.GetComponent<TextTips>().ChangeUse(timeWhenCanSitDown, true));

        StartCoroutine(raycastControl.CanStand(timeWhenCanSitDown, false, false));
    }
}