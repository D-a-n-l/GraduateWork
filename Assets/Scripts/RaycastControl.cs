using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.Profiling;

public class RaycastControl : MonoBehaviour
{
    public MoveController player;



    public Animator animator;

    public LayerMask mask;

    [Space(10)]
    [SerializeField]
    private Transform offsetCamera;

    [Space(10)]
    [SerializeField]
    private GameObject crosshair;

    [Space(10)]
    [SerializeField] 
    private Text textOnTips;

    [Space(10)]
    [SerializeField] 
    private GameObject wireLength;

    [Space(10)]
    [SerializeField]
    private Text textWireLength;

    [Space(10)]
    [Min(0.001f)]
    [SerializeField]
    private float radius;

    [Space(10)]
    [SerializeField]
    private KeyCode raycastKeyCode;

    private Camera mainCamera;

    private CanvasManagment canvasManagment;

    private CinemachineBrain cinemachineBrain;

    private GrabbingObject grabbingObject;



    private MovingObject mov;

    private TurnObject currentTurnObject;

    private Rigidbody rigidbodyOdject;

    [SerializeField, Min(.1f)]
    private float grabPower = 10f;

    [SerializeField, Min(.1f)]
    private float scrollSpeed = 200f;

    private bool currentTurnObjectUse;

    private Transform positionHit;

    [SerializeField]
    private GameObject descriptionObject;

    [SerializeField]
    private Text descriptionText;

    private bool rotateObject = false;


    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private Slider changeSensivitityMouse;

    private RaycastHit hit;


    private SitOnChair chair;

    private SitOnPC pc;

    private CreateMesh createMesh;

    private Outline _currentOutline;

    private TextTips _currentTextTips;

    private float timeBlendCinemachine;

    private bool canStand = false;

    private bool enable = false;

    private bool usePc = false;

    private bool grab = false;

    public bool useWire = false;

    public Transform currentObject;

    public Transform offset;

    private static ProfilerMarker profilerMarker = new ProfilerMarker(ProfilerCategory.Loading, "outlines");
    //public float _timeBlendCinemachine { get { return timeBlendCinemachine; } }

    private void Start()
    {
        mainCamera = Camera.main;

        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();

        timeBlendCinemachine = cinemachineBrain.m_DefaultBlend.m_Time;
    }

    private void Update()
    {
        profilerMarker.Begin();
        if (Physics.SphereCast(mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), radius, out hit, 100))
        {
            if (hit.collider.TryGetComponent(out Outline outline) && hit.collider.TryGetComponent(out TextTips _textTips)
                && (hit.distance <= _textTips._distanceToUse) && (_textTips._use == true || canStand == true))
            {
                if (hit.collider.TryGetComponent(out SitOnChair chairs) && canStand == false)
                {
                    _currentOutline = outline;

                    _currentTextTips = _textTips;

                    StartCoroutine(FastOutlineAndTips());

                    if (Input.GetKeyDown(raycastKeyCode) && _currentOutline.OutlineWidth >= 1)
                    {
                        chair = chairs;

                        chair.Sit();

                        _currentTextTips.ChangeActiveText();
                    }
                }
                else if (hit.collider.TryGetComponent(out SitOnPC sitOnPC) && canStand == true)
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode) && _currentTextTips._use == true)
                    {
                        pc = sitOnPC;

                        pc.Sit();
                    }
                    else if (Input.GetKeyDown(raycastKeyCode) && _currentTextTips._use == false)
                    {
                        pc.Stand();
                    }
                }
                else if (hit.collider.TryGetComponent(out StandOnTable tables) && canStand == true && usePc == false)
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode))
                    {
                        tables.StandUp(chair._lastPositionPlayer, chair);

                        _currentTextTips.ChangeActiveText();
                    }
                }
                else if (hit.collider.TryGetComponent(out Door doors))
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode))
                    {
                        doors.OpenorClose();
                    }
                }
                else if (hit.collider.TryGetComponent(out BedSideTable box))
                {
                    _currentOutline = outline;

                    //_currentTextTips = _textTips;

                    textOnTips.text = _currentTextTips._textForFirstAction;

                    StartCoroutine(FastOutlineAndTips());

                    if (Input.GetKeyDown(raycastKeyCode) && _currentOutline.OutlineWidth >= 1)
                    {
                        box.OpenOrClose();

                        _currentTextTips.ChangeActiveText();
                    }
                }
                //else if (hit.collider.CompareTag("sasa"))
                //{
                //    OutlineAndTips(outline, _textTips);

                //    wireLength.SetActive(true);

                //    //textWireLength.text = "Длина кабеля " + createMesh._maxDistance.ToString() + "м.";

                //    if (Input.GetKeyDown(raycastKeyCode))
                //    {
                //        currentObject = null;

                //        currentObject = _currentTextTips.transform;

                //        rigidbodyOdject = hit.rigidbody;

                //        rigidbodyOdject.constraints = RigidbodyConstraints.None;

                //        positionHit = hit.transform;

                //        grab = !grab;

                //        useWire = !useWire;

                //        wireLength.SetActive(false);

                //        //_currentTextTips.ChangeActiveText();
                //    }
                //}
                else if (hit.collider.TryGetComponent(out MovingObject movingObject))
                {
                    _currentOutline = outline;

                    _currentTextTips = _textTips;

                    StartCoroutine(FastOutlineAndTips());

                    if (Input.GetKeyDown(raycastKeyCode) && _currentOutline.OutlineWidth >= 1)
                    {
                        mov = movingObject;

                        currentObject = _currentTextTips.transform;

                        movingObject.ChangeGrab();

                        useWire = !useWire;
                    }
                }
                else if (_textTips._typeObject == TextTips.TypeObject.Input && useWire == true)
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode))
                    {
                        grab = !grab;

                        useWire = !useWire;

                        currentObject.gameObject.layer = 2;

                        _currentTextTips.SetWire(currentObject);

                        rigidbodyOdject.constraints = RigidbodyConstraints.FreezeAll;

                        currentObject.rotation = Quaternion.identity;

                        currentObject.position = hit.collider.transform.position;

                        _currentTextTips.ChangeActiveText();

                        currentObject = null;
                    }
                }
                else if (_textTips._typeObject == TextTips.TypeObject.Input && useWire == false)
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode))
                    {
                        currentObject = _currentTextTips._isWire;
                        currentObject.gameObject.layer = 0;
                        _currentTextTips.SetWire(null);

                        rigidbodyOdject = currentObject.GetComponent<Rigidbody>();

                        rigidbodyOdject.constraints = RigidbodyConstraints.None;

                        positionHit = currentObject;

                        grab = !grab;

                        useWire = !useWire;

                        _currentTextTips.ChangeActiveText();
                    }
                }                 
                else if (hit.collider.TryGetComponent(out Test board))
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode))
                    {
                        board.QuestionGenerator();
                    }
                }
                else if (hit.collider.TryGetComponent(out TurnObject turnObject))
                {
                    OutlineAndTips(outline, _textTips);

                    if (Input.GetKeyDown(raycastKeyCode))
                    {

                        //_currentOutline.enabled = false;

                        //textOnTips.text = null;

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
                    }

                    if (Input.GetMouseButton(1) && currentTurnObject != null)
                    {
                        currentTurnObject.RotateAroundObject();

                        CanMoveCamera(RigidbodyConstraints.FreezePosition, false, true, 0, CursorLockMode.Confined);

                        rotateObject = true;
                    }
                    else if (Input.GetMouseButtonUp(1) && rotateObject == true)
                    {
                        CanMoveCamera(RigidbodyConstraints.None, true, false, 100, CursorLockMode.Locked);

                        rotateObject = false;
                    }
                }
                else if (hit.collider.CompareTag("Door"))
                {
                    OutlineAndTips(outline, _textTips);
                }
            }
            else
            {
                wireLength.SetActive(false);

                enable = true;

                if (_currentOutline != null)
                {
                    _currentOutline.enabled = false;

                    textOnTips.text = null;

                    if (Input.GetKeyDown(raycastKeyCode))
                    {
                        _currentOutline.enabled = false;

                        textOnTips.text = null;
                    }
                }
                else if (_currentOutline == null)
                {
                    textOnTips.text = null;

                    _currentOutline = null;
                }
            }
        }

        profilerMarker.End();

    }
    private void FixedUpdate()
    {
        if (grab == true && rotateObject == false)
        {
            if (Input.GetKey(KeyCode.F))
            {
                currentTurnObject.ReferenceTurn2();
            }

            offsetCamera.position = offsetCamera.position + (mainCamera.transform.forward) * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

            rigidbodyOdject.velocity = (offsetCamera.position - (positionHit.position + rigidbodyOdject.centerOfMass)) * grabPower;

            if (offsetCamera.localPosition.z >= 0.9f)
            {
                offsetCamera.localPosition = new Vector3(offsetCamera.localPosition.x, offsetCamera.localPosition.y, 0.9f);
            }
            else if (offsetCamera.localPosition.z <= 0.4f)
            {
                offsetCamera.localPosition = new Vector3(offsetCamera.localPosition.x, offsetCamera.localPosition.y, 0.4f);
            }
        }
    }

    public IEnumerator CanStand(float delay, bool stand, bool pcUser)
    {
        yield return new WaitForSeconds(delay);

        canStand = stand;

        usePc = pcUser;
    }

    private void CanMoveCamera(RigidbodyConstraints rigidbodyConstraints, bool enabledCamera, bool cursor, int numberMultiple, CursorLockMode cursorLockMode)
    {
        //virtualCamera.GetCinemachineComponent<CinemachineHardLockToTarget>().enabled = enabledCamera;

        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = numberMultiple * changeSensivitityMouse.value;
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = numberMultiple * changeSensivitityMouse.value;

        hit.rigidbody.velocity = Vector3.zero;

        hit.rigidbody.angularVelocity = Vector3.zero;

        rigidbodyOdject.constraints = rigidbodyConstraints;

        StartCoroutine(CursorControl.Control(0f, cursorLockMode, cursor, 1f));
        //Cursor.lockState = cursorLockMode;
        //Cursor.visible = !enabledCamera;

        player.enabled = enabledCamera;

        animator.enabled = enabledCamera;

        rotateObject = !enabledCamera;
    }

    private void UseDescription(bool setActive)
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

    public void TimeBlendCinemachine(float timeBlend)
    {
        cinemachineBrain.m_DefaultBlend.m_Time = timeBlend;

        timeBlendCinemachine = cinemachineBrain.m_DefaultBlend.m_Time;
    }

    private void OutlineAndTips(Outline outlines, TextTips _textTips)
    {
        _currentOutline = outlines;

        _currentTextTips = _textTips;

        if (enable == true)
        {
            enable = false;

            _currentOutline.enabled = true;

            if (_currentTextTips._typeObject == TextTips.TypeObject.ObjectWithOneAction)
            {
                textOnTips.text = _currentTextTips._textForFirstAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.ObjectWithTwoAction && _currentTextTips._isActiveText == TextTips.IsActiveText.First)
            {
                textOnTips.text = _currentTextTips._textForFirstAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.ObjectWithTwoAction && _currentTextTips._isActiveText == TextTips.IsActiveText.Second)
            {
                textOnTips.text = _currentTextTips._textForSecondAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.Input && _currentTextTips._isActiveText == TextTips.IsActiveText.First)
            {
                textOnTips.text = _currentTextTips._textForSecondAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.Input && _currentTextTips._isActiveText == TextTips.IsActiveText.Second)
            {
                textOnTips.text = _currentTextTips._textForSecondAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
        }
        else
        {
            StartCoroutine(FastOutlineAndTips());

            if (_currentTextTips._typeObject == TextTips.TypeObject.ObjectWithOneAction)
            {
                textOnTips.text = _currentTextTips._textForFirstAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.ObjectWithTwoAction && _currentTextTips._isActiveText == TextTips.IsActiveText.First)
            {
                textOnTips.text = _currentTextTips._textForFirstAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.ObjectWithTwoAction && _currentTextTips._isActiveText == TextTips.IsActiveText.Second)
            {
                textOnTips.text = _currentTextTips._textForSecondAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.Input && _currentTextTips._isActiveText == TextTips.IsActiveText.First)
            {
                textOnTips.text = _currentTextTips._textForFirstAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
            else if (_currentTextTips._typeObject == TextTips.TypeObject.Input && _currentTextTips._isActiveText == TextTips.IsActiveText.Second)
            {
                textOnTips.text = _currentTextTips._textForSecondAction + "\n" + "(" + raycastKeyCode.ToString() + ")";
            }
        }
    }

    //private IEnumerator FastOutlineAndTips()
    //{
    //    _currentOutline.OutlineWidth = 30;

    //    yield return new WaitForEndOfFrame();

    //    _currentOutline.OutlineWidth = 0;
    //}

    private IEnumerator FastOutlineAndTips()
    {
        _currentOutline.enabled = true;

        yield return new WaitForEndOfFrame();

        if (_currentOutline != null)
        {
            _currentOutline.enabled = false;

            _currentOutline = null;
        }
    }
}