using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InverseKinematic: MonoBehaviour
{
    [SerializeField, Min(.0001f)]
    private float lengthRay;

    [SerializeField, Min(.0001f)]
    private float raduisSphere;

    [SerializeField, Space(5f)]
    private string[] tagsOtherObjects;

    [Header("Left Hand (LH)"), Space(5f)]

    [Header("When LH Place On Chair"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float xOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float yOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float zOffsetLH;

    [SerializeField]
    private Vector3 rotationOffsetLH;

    [Space(10f)]

    [Header("When LH Place On Chair Back"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float XOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float YOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float ZOffsetLH;

    [SerializeField]
    private Vector3 RotationOffsetLH;

    [Space(10f)]

    [Header("When LH Place On Table"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float _xOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float _yOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float _zOffsetLH;

    [SerializeField]
    private Vector3 _rotationOffsetLH;

    [Space(10f)]

    [Header("When LH Place On Other Object"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float _XOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float _YOffsetLH;

    [SerializeField, Range(-1f, 1f)]
    private float _ZOffsetLH;

    [SerializeField]
    private Vector3 _RotationOffsetLH;

    [Space(20f)]

    [Header("Right Hand (RH)"), Space(5f)]

    [Header("When RH Place On Chair"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float xOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float yOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float zOffsetRH;

    [SerializeField]
    private Vector3 rotationOffsetRH;

    [Space(10f)]

    [Header("When RH Place On Chair Back"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float XOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float YOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float ZOffsetRH;

    [SerializeField]
    private Vector3 RotationOffsetRH;

    [Space(10f)]

    [Header("When RH Place On Table"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float _xOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float _yOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float _zOffsetRH;

    [SerializeField]
    private Vector3 _rotationOffsetRH;

    [Space(10f)]

    [Header("When RH Place On Other Object"), Space(5f)]
    [SerializeField, Range(-1f, 1f)]
    private float _XOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float _YOffsetRH;

    [SerializeField, Range(-1f, 1f)]
    private float _ZOffsetRH;

    [SerializeField]
    private Vector3 _RotationOffsetRH;

    private Animator animator;

    private Ray rayLH;

    private Ray rayRH;

    private Vector3 LH;

    private Vector3 RH;

    private bool sitPlayer = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Sit(bool trueOrFalse)
    {
        sitPlayer = trueOrFalse;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            RaycastHit hit;

            rayLH = new(animator.GetIKPosition(AvatarIKGoal.LeftHand), Vector3.down);

            rayRH = new(animator.GetIKPosition(AvatarIKGoal.RightHand), Vector3.down);

            LH = animator.GetIKPosition(AvatarIKGoal.LeftHand);

            RH = animator.GetIKPosition(AvatarIKGoal.RightHand);

            if (Physics.SphereCast(LH, raduisSphere, Vector3.down, out hit))
            {
                if (hit.collider.GetComponent<StandOnTable>())
                {
                    Abbrevation(hit, _xOffsetLH, _yOffsetLH, _zOffsetLH, AvatarIKGoal.LeftHand, _rotationOffsetLH);
                }
                else if (CheckTags(hit))
                {
                    Abbrevation(hit, _XOffsetLH, _YOffsetLH, _ZOffsetLH, AvatarIKGoal.LeftHand, _RotationOffsetLH);
                }
            }

            if (Physics.CheckSphere(LH, raduisSphere))
            {
                if (hit.collider.GetComponent<SitOnChair>() && hit.collider.GetComponent<SitOnChair>()._putDownHands == false)
                {
                    Abbrevation(hit, XOffsetLH, YOffsetLH, ZOffsetLH, AvatarIKGoal.LeftHand, RotationOffsetLH);
                }
            }

            if (Physics.Raycast(rayLH, out hit, lengthRay))
            {
                if (hit.collider.GetComponent<SitOnChair>() == false && sitPlayer)
                {
                    Abbrevation(hit, _xOffsetLH, _yOffsetLH, _zOffsetLH, AvatarIKGoal.LeftHand, _rotationOffsetLH);
                }
                else if (hit.collider.GetComponent<SitOnChair>() && hit.collider.GetComponent<SitOnChair>()._putDownHands == true)
                {
                    Abbrevation(hit, xOffsetLH, yOffsetLH, zOffsetLH, AvatarIKGoal.LeftHand, rotationOffsetLH);
                }
            }

            if (Physics.SphereCast(RH, raduisSphere, Vector3.down, out hit))
            {
                if (hit.collider.GetComponent<StandOnTable>())
                {
                    Abbrevation(hit, _xOffsetRH, _yOffsetRH, _zOffsetRH, AvatarIKGoal.RightHand, _rotationOffsetRH);
                }
                else if (CheckTags(hit))
                {
                    Abbrevation(hit, _XOffsetRH, _YOffsetRH, _ZOffsetRH, AvatarIKGoal.RightHand, _RotationOffsetRH);
                }
            }

            if (Physics.CheckSphere(RH, raduisSphere))
            {
                if (hit.collider.GetComponent<SitOnChair>() && hit.collider.GetComponent<SitOnChair>()._putDownHands == false)
                {
                    Abbrevation(hit, XOffsetRH, YOffsetRH, ZOffsetRH, AvatarIKGoal.RightHand, RotationOffsetRH);
                }
            }

            if (Physics.Raycast(rayRH, out hit, lengthRay))
            {
                if (hit.collider.GetComponent<SitOnChair>() == false && sitPlayer)
                {
                    Abbrevation(hit, _xOffsetRH, _yOffsetRH, _zOffsetRH, AvatarIKGoal.RightHand, _rotationOffsetRH);
                }
                else if (hit.collider.GetComponent<SitOnChair>() && hit.collider.GetComponent<SitOnChair>()._putDownHands == true)
                {
                    Abbrevation(hit, xOffsetRH, yOffsetRH, zOffsetRH, AvatarIKGoal.RightHand, rotationOffsetRH);
                }
            }
        }
    }

    private bool CheckTags(RaycastHit hit)
    {
        for (int i = 0; i < tagsOtherObjects.Length; i++)
        {
            if (hit.collider.CompareTag(tagsOtherObjects[i]))
            {
                return true;
            }
        }

        return false;
    }

    private void Abbrevation(RaycastHit hit, float xOffset, float yOffset, float zOffset, AvatarIKGoal avatarIKGoal, Vector3 rotationOffset)
    {
        Vector3 hitPosition = hit.point;
        
        hitPosition.x += xOffset;
        hitPosition.y += yOffset;
        hitPosition.z += zOffset;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

        animator.SetIKPosition(avatarIKGoal, hitPosition);
        animator.SetIKRotation(avatarIKGoal, Quaternion.LookRotation(transform.forward + rotationOffset));
    }
}
