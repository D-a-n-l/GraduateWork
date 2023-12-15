using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class MovingObject : MonoBehaviour
{
    [SerializeField, Space(5)]
    private Transform offsetCamera;

    [SerializeField, Space(5)]
    private float grabPower;

    [SerializeField, Space(5)]
    private bool scrollObject = false;

    [SerializeField, Space(5)]
    [ShowIf(nameof(scrollObject))]
    private float scrollSpeed;

    [SerializeField, Space(5)]
    [Range(0f, 1f), 
    ShowIf(nameof(scrollObject))]
    private float rangeScroll;

    private Camera mainCamera;

    private Transform currentObject;

    private Rigidbody rigidbodyObject;

    private float positiveLocalPosition;

    private float negativeLocalPosition;

    private bool grab = false;

    private void Start()
    {
        mainCamera = Camera.main;

        currentObject = GetComponent<Transform>();

        rigidbodyObject = GetComponent<Rigidbody>();

        rigidbodyObject.interpolation = RigidbodyInterpolation.Interpolate;

        positiveLocalPosition = offsetCamera.localPosition.z + rangeScroll;

        negativeLocalPosition = offsetCamera.localPosition.z - rangeScroll;
    }

    private void Update()
    {
        if (grab == true && Input.GetKeyDown(KeyCode.E))
        {
            ChangeGrab();
        }
    }

    private void FixedUpdate()
    {
        if (grab == true)
        {
            rigidbodyObject.velocity = (offsetCamera.position - (currentObject.position + rigidbodyObject.centerOfMass)) * grabPower;

            if(scrollObject == true)
            {
                offsetCamera.position += mainCamera.transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

                if (offsetCamera.localPosition.z >= positiveLocalPosition)
                {
                    ChangePosition(positiveLocalPosition);
                }
                else if (offsetCamera.localPosition.z <= negativeLocalPosition)
                {
                    ChangePosition(negativeLocalPosition);
                }
            }
        }
    }

    private void ChangePosition(float newPosition)
    {
        offsetCamera.localPosition = new Vector3(offsetCamera.localPosition.x, offsetCamera.localPosition.y, newPosition);
    }

    public void ChangeGrab()
    {
        grab = !grab;
    }
}