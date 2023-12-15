using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(InverseKinematic))]
public class MoveController : MonoBehaviour
{
    [Min(0.5f)]
    [SerializeField] 
    private float speed;

    [Min(0.5f)]
    [SerializeField] 
    private float smooth;

    private Camera mainCamera;

    private Animator animator;

    private Rigidbody rigidbodyy;

    private InverseKinematic inverseKinematic;

    private Vector3 movingVector;

    private Vector2 currentVelocity;

    private float currentSpeed;

    private bool canMove = true;

    private void Start()
    {
        mainCamera = Camera.main;

        animator = GetComponent<Animator>();

        rigidbodyy = GetComponent<Rigidbody>();

        inverseKinematic = GetComponent<InverseKinematic>();

        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

        rigidbodyy.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbodyy.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            movingVector = Vector3.ClampMagnitude(cameraForward.normalized * Input.GetAxis("Vertical") * currentSpeed + cameraRight.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);

            rigidbodyy.velocity = new Vector3(movingVector.x, rigidbodyy.velocity.y, movingVector.z);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.transform.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            Walk();
        else
            Walk();
    }

    private void Walk()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, moveX * speed, smooth * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, moveY * speed, smooth * Time.fixedDeltaTime);

        animator.SetFloat("x", currentVelocity.x);
        animator.SetFloat("y", currentVelocity.y);

        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 3);
    }

    public IEnumerator ChangeParameters(string nameTriggerAnimation, bool isKinematic, bool ik, float delayCanMove, bool moveCan)
    {
        animator.SetTrigger(nameTriggerAnimation);

        rigidbodyy.isKinematic = isKinematic;

        inverseKinematic.Sit(ik);

        yield return new WaitForSeconds(delayCanMove);

        canMove = moveCan;
    }
}