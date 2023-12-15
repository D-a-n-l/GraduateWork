using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlayer : MonoBehaviour
{
    private Animator animator;

    private float xMouse;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float mouse = Input.GetAxis("Mouse X");

        xMouse = Mathf.Lerp(xMouse, mouse, 10 * Time.deltaTime);

        animator.SetFloat("MouseX", xMouse);
    }
}