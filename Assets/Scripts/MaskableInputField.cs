using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

[RequireComponent(typeof(InputField))]
public class MaskableInputField : MonoBehaviour
{
    [SerializeField] 
    private InputField inputField;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(delegate { OnValueChanged(); });
        inputField.ActivateInputField();
    }

    private void OnValidate()
    {
        inputField = GetComponent<InputField>();
    }

    private void OnValueChanged()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = string.Empty;
        }
        else
        {
            string input = inputField.text;
            string MatchPattern = @"^((\d{3}\.){0,3}(\d{1,2})?)$";
            string ReplacementPattern = "$1.$3";
            string ToReplacePattern = @"((\.?\d{3})+)(\d)";

            input = Regex.Replace(input, ToReplacePattern, ReplacementPattern);
            Match result = Regex.Match(input, MatchPattern);
            
            if (result.Success)
            {
                inputField.text = input;
                inputField.caretPosition++;
            }
        }
    }
}
