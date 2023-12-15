using UnityEngine;
using UnityEngine.UI;

public class ChangeViewPassword : MonoBehaviour
{
    [SerializeField] 
    private InputField inputField;

    [Space(10)]
    [SerializeField] 
    private Image checkmarkToggle;

    [Space(10)]
    [SerializeField] 
    private Sprite checkmarkOn;

    [SerializeField] 
    private Sprite checkmarkOff;

    public void ChangeContentType(bool value)
    {
        if (value)
        {
            ChangeType(InputField.ContentType.Password, checkmarkOn);
        }
        else
        {
            ChangeType(InputField.ContentType.Standard, checkmarkOff);
        }
    }

    private void ChangeType(InputField.ContentType contentType, Sprite sprite)
    {
        inputField.contentType = contentType;

        inputField.ActivateInputField();

        checkmarkToggle.sprite = sprite;
    }
}