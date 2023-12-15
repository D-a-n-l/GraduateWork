using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnHead : MonoBehaviour
{
    [SerializeField] private Transform lookObject;

    [SerializeField] private bool ikActive = true;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (_animator)
        {
            if (ikActive == true)
            {
                _animator.SetLookAtWeight(1f);
                _animator.SetLookAtPosition(lookObject.position);
            }
            else if(ikActive == false)
            {
                _animator.SetLookAtWeight(0f);
            }
        }
    }
}