using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TurnObject : MonoBehaviour
{
    [SerializeField, Min(.1f)]
    private float speedTurnWithKey = .1f;

    [SerializeField, Min(.1f)]
    private float speedTurnWithMouse = .1f;

    [SerializeField]
    private Vector3 referenceRotation;

    [Space(10)]
    [SerializeField]
    private bool usePlacePlant = true;

    public bool _usePlacePlant { get { return usePlacePlant; } }

    [SerializeField]
    private GameObject placePlant;

    [SerializeField]
    private BuildingPC triggerSmall;

    [SerializeField]
    private BuildingPC2 triggerBig;

    [Space(10)]
    [SerializeField]
    private bool useDescription = true;

    public bool _useDescription { get { return useDescription; } }

    [SerializeField, Multiline]
    private string description;

    public string _description { get { return description; } }

    [Space(10)]
    [SerializeField]
    private AxisX asixXRotation;

    [SerializeField]
    private AxisY axisYRotation;

    private Vector3 directionXVector;

    private Vector3 directionYVector;

    private Camera mainCamera;

    private Rigidbody rigidbodyy;

    private bool use = true;

    public bool _use { get { return use; } }

    public enum AxisX
    {
        right,
        up,
        forward,
        left,
        down,
        back,
    }

    public enum AxisY
    {
        right,
        up,
        forward,
        left,
        down,
        back,
    }

    private void Start()
    {
        mainCamera = Camera.main;

        rigidbodyy = GetComponent<Rigidbody>();

        rigidbodyy.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()//Start потом сделать
    {
        DefineDirection();
    }

    public void SetUse(bool set)
    {
        use = set;
    }

    public void SetPlacePlant(bool place)
    {
        placePlant.SetActive(place);
    }

    public IEnumerator DisabledCollision(bool collision)
    {
        yield return new WaitForSeconds(2f);

        triggerSmall.OnCollision(collision);
        triggerBig.OnCollision(collision);
    }

    public void ReferenceTurn2()
    {
        Quaternion translateInQuaternion = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles + referenceRotation);

        rigidbodyy.velocity = Vector3.zero;

        rigidbodyy.angularVelocity = Vector3.zero;

        transform.rotation = Quaternion.Slerp(transform.rotation, translateInQuaternion, speedTurnWithKey * Time.deltaTime);
    }

    public void ReferenceTurn(RaycastHit hit)
    {
        Quaternion translateInQuaternion = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles + referenceRotation);

        hit.rigidbody.velocity = Vector3.zero;

        hit.rigidbody.angularVelocity = Vector3.zero;

        hit.transform.rotation = Quaternion.Slerp(hit.transform.rotation, translateInQuaternion, speedTurnWithKey * Time.deltaTime);
    }

    public void RotateAroundObject()
    {
        float positionMouseX = Input.GetAxis("Mouse X") * speedTurnWithMouse * Time.deltaTime;
        float positionMouseY = Input.GetAxis("Mouse Y") * speedTurnWithMouse * Time.deltaTime;

        transform.Rotate(directionYVector, positionMouseY);
        transform.Rotate(directionXVector, positionMouseX);
    }

    private void DefineDirection()
    {
        switch (asixXRotation)
        {
           case AxisX.right:
                directionXVector = Vector3.right;
                break;
           case AxisX.up:
                directionXVector = Vector3.up;
                break;
           case AxisX.forward:
                directionXVector = Vector3.forward;
                break;
           case AxisX.left:
                directionXVector = Vector3.left;
                break;
           case AxisX.down:
                directionXVector = Vector3.down;
                break;
           case AxisX.back:
                directionXVector = Vector3.back;
                break;
           default:
                break;
        }

        switch (axisYRotation)
        {
            case AxisY.right:
                directionYVector = Vector3.right;
                break;
            case AxisY.up:
                directionYVector = Vector3.up;
                break;
            case AxisY.forward:
                directionYVector = Vector3.forward;
                break;
            case AxisY.left:
                directionYVector = Vector3.left;
                break;
            case AxisY.down:
                directionYVector = Vector3.down;
                break;
            case AxisY.back:
                directionYVector = Vector3.back;
                break;
            default:
                break;
        }
    }
}