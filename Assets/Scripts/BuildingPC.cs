using System.Collections;
using UnityEngine;

public class BuildingPC : MonoBehaviour
{
    [SerializeField] private string textTag;

    [SerializeField] private Vector3[] fromVectorBeforeVector;

    private GrabbingObject grabbingObject;

    private BoxCollider collision;

    private void Start()
    {
        grabbingObject = FindObjectOfType<GrabbingObject>().GetComponent<GrabbingObject>();

        collision = GetComponent<BoxCollider>();
    }

    public void OnCollision(bool trueOrFalse)
    {
        collision.enabled = trueOrFalse;
    }

    private void OnTriggerEnter(Collider col) => Abbreviation(col);

    private void OnTriggerStay(Collider col) => Abbreviation(col);

    private void OnTriggerExit(Collider col) => Abbreviation(col);

    private void Abbreviation(Collider col)
    {
        if (col.CompareTag(textTag) &&
        (col.transform.eulerAngles.x >= fromVectorBeforeVector[0].x && col.transform.eulerAngles.x <= fromVectorBeforeVector[1].x) &&
        (col.transform.eulerAngles.y >= fromVectorBeforeVector[0].y && col.transform.eulerAngles.y <= fromVectorBeforeVector[1].y) &&
        (col.transform.eulerAngles.z >= fromVectorBeforeVector[0].z && col.transform.eulerAngles.z <= fromVectorBeforeVector[1].z))
        {
            grabbingObject.SetGrab(false);

            grabbingObject.SetUse(true);

            OnCollision(false);
        }
    }
}