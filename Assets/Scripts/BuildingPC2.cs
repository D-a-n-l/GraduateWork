using UnityEngine;

public class BuildingPC2 : MonoBehaviour
{
    [SerializeField] private string textTag;

    [SerializeField] private Vector3 vectorPosition, vectorRotation;

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

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag(textTag))
        {
            if (grabbingObject._grab == false && grabbingObject.CurrentTurnObjectUse == true)
            {
                if (col.CompareTag("CPU"))
                {
                    transform.parent.GetChild(0).gameObject.SetActive(true);

                    col.gameObject.SetActive(false);
                }

                col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                col.transform.localPosition = vectorPosition;
                col.transform.eulerAngles = vectorRotation;

                OnCollision(false);

                grabbingObject.SetPlacePlant(false);
                grabbingObject.UseDescription(false);
            }
        }
    }
}