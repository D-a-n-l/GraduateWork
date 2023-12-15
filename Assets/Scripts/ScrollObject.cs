using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingObject))]
public class ScrollObject : MonoBehaviour
{
    [SerializeField]
    private Transform offsetCamera;

    [SerializeField]
    private float scrollSpeed;

    [SerializeField, Range(0f, 1f)]
    private float rangeScroll;

    private Camera mainCamera;

    private MovingObject movingObject;

    private float defaultLocalPosition;

    private void Start()
    {
        mainCamera = Camera.main;

        movingObject = GetComponent<MovingObject>();

        defaultLocalPosition = offsetCamera.localPosition.z;
    }

    private void FixedUpdate()
    {

            offsetCamera.position = offsetCamera.position + mainCamera.transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

            if (offsetCamera.localPosition.z >= defaultLocalPosition + rangeScroll)
            {
                offsetCamera.localPosition = new Vector3(offsetCamera.localPosition.x, offsetCamera.localPosition.y, defaultLocalPosition + rangeScroll);
            }
            else if (offsetCamera.localPosition.z <= defaultLocalPosition - rangeScroll)
            {
                offsetCamera.localPosition = new Vector3(offsetCamera.localPosition.x, offsetCamera.localPosition.y, defaultLocalPosition - rangeScroll);
            }
    }
}