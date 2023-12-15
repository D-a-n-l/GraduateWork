using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class SitOnChair : MonoBehaviour
{
    [SerializeField]
    private MoveController player;

    [Space(10)]
    [SerializeField]
    private RaycastControl raycastControl;

    [Min(0.01f), Space(10)]
    [SerializeField]
    private float timeWhenCanStandUp = 1.5f;

    [Space(10)]
    [SerializeField]
    private string animationTriggerForSitDown;

    [Space(10)]
    [SerializeField]
    private CinemachineVirtualCamera mainVirtualCamera;

    [SerializeField]
    private float angleWhenSitDown;

    [Space(10)]
    [SerializeField, Range(0, 360)]
    private float minAngleView;

    [SerializeField, Range(0, 360)]
    private float maxAngleView;

    [Space(10)]
    [SerializeField]
    private Vector3 offsetWhenSit;

    private TextTips textTips;

    private Vector3 lastPositionPlayer;

    private bool putDownHands = false;

    public Vector3 _lastPositionPlayer { get { return lastPositionPlayer; } }

    public bool _putDownHands { get { return putDownHands; } }

    //private void Start()
    //{
    //    textTips = GetComponent<TextTips>();

    //    if (player == null || raycastControl == null || animationTriggerForSitDown == null || mainVirtualCamera == null)
    //        throw new UnassignedReferenceException();
    //}

    public void Sit()
    {
        StartCoroutine(player.ChangeParameters(animationTriggerForSitDown, true, true, 0f, false));

        lastPositionPlayer = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        player.transform.position = new Vector3(transform.position.x + offsetWhenSit.x, transform.position.y + offsetWhenSit.y, transform.position.z + offsetWhenSit.z);

        VirtualCameraControl.Control(mainVirtualCamera, angleWhenSitDown, minAngleView, maxAngleView, false);

        PutHadns(true);

        StartCoroutine(textTips.ChangeUse(0f, false));

        StartCoroutine(raycastControl.CanStand(timeWhenCanStandUp, true, false));
    }

    public void PutHadns(bool trueOrFalse)
    {
        putDownHands = trueOrFalse;
    }
}