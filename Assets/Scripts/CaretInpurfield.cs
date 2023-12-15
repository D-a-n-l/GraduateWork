using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaretInpurfield : MonoBehaviour
{
    private InputField _inputField;

    private void Start()
    {
        _inputField = GetComponent<InputField>();
    }

    private void Update()
    {
        _inputField.caretPosition = _inputField.text.Length;
    }
}