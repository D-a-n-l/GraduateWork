using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GrabbingObject : MonoBehaviour
{
    [SerializeField, Min(.1f)]
    private float grabPower = 10f;

    [SerializeField, Min(.1f)]
    private float scrollSpeed = 200f;

    [SerializeField] 
    private Transform offsetGrab;

    [SerializeField] 
    private Slider changeSensivitityMouse;

    [SerializeField] 
    private GameObject descriptionObject;

    [SerializeField]
    private Text descriptionText;

    [SerializeField] 
    private CinemachineVirtualCamera virtualCamera;

    private Camera mainCamera;

    private TurnObject currentTurnObject;

    private MoveController player;

    private Animator animator;

    private Rigidbody rigidbodyOdject;

    private RaycastHit hit;

    private Transform positionHit;

    private bool rotateObject = false;


    private bool grab = false;

    public bool _grab { get { return grab; } }


    private bool currentTurnObjectUse;

    public bool CurrentTurnObjectUse { get { return currentTurnObject; } }

    private void Start()
    {
        mainCamera = Camera.main;

        player = FindObjectOfType<MoveController>().GetComponent<MoveController>();

        animator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit, 20f) &&
                hit.collider.TryGetComponent(out TurnObject turnObject) &&
                hit.collider.TryGetComponent(out TextTips textTips) && 
                hit.distance <= textTips._distanceToUse)
            {
                grab = !grab;

                currentTurnObject = turnObject;

                currentTurnObjectUse = currentTurnObject._use;

                rigidbodyOdject = hit.rigidbody;

                positionHit = hit.transform;

                rigidbodyOdject.constraints = RigidbodyConstraints.None;

                currentTurnObject.SetUse(false);
                
                if (currentTurnObject._usePlacePlant == true)
                {
                    currentTurnObject.SetPlacePlant(true);
                    StartCoroutine(currentTurnObject.DisabledCollision(true));
                }
                else
                    Debug.Log("Некуда ставить");

                if (currentTurnObject._useDescription == true)
                    UseDescription(true);
                else
                    UseDescription(false);

                if (grab == false)
                {
                    if (currentTurnObject._usePlacePlant == true)
                        currentTurnObject.SetPlacePlant(false);
                    else
                        Debug.Log("Некуда ставить");

                    currentTurnObject.SetUse(true);

                    currentTurnObject = null;

                    UseDescription(false);
                }
                Debug.Log(grab);
            }
        }

        if (Input.GetMouseButton(1) && currentTurnObject != null)
        {
            currentTurnObject.RotateAroundObject();

            CanMoveCamera(RigidbodyConstraints.FreezePosition, false, 0, CursorLockMode.Confined);

            rotateObject = true;
        }
        else if(Input.GetMouseButtonUp(1) && rotateObject == true)
        {
            CanMoveCamera(RigidbodyConstraints.None, true, 100, CursorLockMode.Locked);

            rotateObject = false;
        }
    }

    private void FixedUpdate()
    {
        if (grab == true && rotateObject == false)
        {
            if (Input.GetKey(KeyCode.F))
            {
                currentTurnObject.ReferenceTurn(hit);
            }

            offsetGrab.position = offsetGrab.position + (mainCamera.transform.forward) * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

            rigidbodyOdject.velocity = (offsetGrab.position - (positionHit.position + rigidbodyOdject.centerOfMass)) * grabPower;

            if (offsetGrab.localPosition.z >= 0.9f)
            {
                offsetGrab.localPosition = new Vector3(offsetGrab.localPosition.x, offsetGrab.localPosition.y, 0.9f);
            }
            else if (offsetGrab.localPosition.z <= 0.4f)
            {
                offsetGrab.localPosition = new Vector3(offsetGrab.localPosition.x, offsetGrab.localPosition.y, 0.4f);
            }
        }
    }

    public void SetGrab(bool set)
    {
        grab = set;
    }

    public void SetUse(bool set)
    {
        currentTurnObject.SetUse(set);
    }

    public void SetPlacePlant(bool place)
    {
        currentTurnObject.SetPlacePlant(place);
    }

    public void UseDescription(bool setActive)
    {
        descriptionObject.SetActive(setActive);

        if (setActive == true)
        {
            descriptionText.text = currentTurnObject._description;
        }
        else
        {
            descriptionText.text = null;
        }
    }

    private void CanMoveCamera(RigidbodyConstraints rigidbodyConstraints, bool enabledCamera, int numberMultiple, CursorLockMode cursorLockMode)
    {
        //virtualCamera.GetCinemachineComponent<CinemachineHardLockToTarget>().enabled = enabledCamera;

        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = numberMultiple * changeSensivitityMouse.value;
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = numberMultiple * changeSensivitityMouse.value;

        hit.rigidbody.velocity = Vector3.zero;

        hit.rigidbody.angularVelocity = Vector3.zero;

        rigidbodyOdject.constraints = rigidbodyConstraints;

        Cursor.lockState = cursorLockMode;
        Cursor.visible = !enabledCamera;

        player.enabled = enabledCamera;

        animator.enabled = enabledCamera;

        rotateObject = !enabledCamera;
    }
}