using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LimitWrite : MonoBehaviour
{
    [SerializeField] 
    private string @regularExpressions = "[]";

    private InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();

        inputField.onValueChanged.AddListener(delegate { WriteLimit(); });
    }

    private void WriteLimit()
    {
        if (Regex.IsMatch(inputField.text, @regularExpressions))
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
        }
    }
}