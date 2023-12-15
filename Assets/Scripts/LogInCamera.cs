using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class LogInCamera : MonoBehaviour
{
    [Space(5)]
    [SerializeField]
    private MeshRenderer displayForOutputCamera;

    [Space(10)]
    [SerializeField]
    private PasswordPool passwordPool;

    [Space(10)]
    [SerializeField]
    private CanvasManagment canvasManagment;

    [Space(10)]
    [SerializeField]
    private InputField inputfieldIp;

    [SerializeField]
    private InputField inputfieldPassword;

    [Space(10)]
    [SerializeField]
    private Animator animatorLaptop;

    [SerializeField]
    private string nameAnimationError = "error";

    [Space(10)]
    [SerializeField]
    private Text objectTextForNameCamera;

    [Space(10)]
    [SerializeField]
    private Text objectTextForError;

    [Space(10)]
    [SerializeField]
    private string textForErrorIncludedCamera = "камера уже включена";

    [SerializeField]
    private string textForErrorFalseIpOrPassword = "не правильный пароль или ip";

    private Material materialDisplayForOutputCamera;

    private int numberCamera;

    private void Awake()
    {
        materialDisplayForOutputCamera = displayForOutputCamera.material;

        if (animatorLaptop == null || nameAnimationError == null || objectTextForNameCamera == null || objectTextForError == null || textForErrorIncludedCamera == null ||
            textForErrorFalseIpOrPassword == null || canvasManagment == null || inputfieldIp == null || inputfieldPassword == null || displayForOutputCamera == null || passwordPool == null)
            throw new UnassignedReferenceException();
    }

    public void LogIn()
    {
        string ipL = inputfieldIp.text.ToLower();
        string passwordL = inputfieldPassword.text.ToLower();

        for (int i = 0; i < passwordPool._pool.Length; i++)
        {
            if (ipL == passwordPool._pool[i].ip && passwordL == passwordPool._pool[i].password)
            {
                if (passwordPool._pool[i].camera.gameObject.activeSelf == false)
                    EnableCamera(i);
                else
                    Error(nameAnimationError, textForErrorIncludedCamera);
            }
            else
            {
                Error(nameAnimationError, textForErrorFalseIpOrPassword);
            }
        }
    }

    public void LogOut()
    {
        EnableCamera(numberCamera, false);
    }

    private void Error(string animatorTrigger, string textError)
    {
        animatorLaptop.SetTrigger(animatorTrigger);

        objectTextForError.text = textError;
    }

    private void EnableCamera(int value, bool active = true)
    {
        numberCamera = value;

        canvasManagment.Enabled(!active, active, false);

        displayForOutputCamera.gameObject.SetActive(active);

        passwordPool._pool[value].camera.gameObject.SetActive(active);

        if (active == true)
        {
            materialDisplayForOutputCamera.mainTexture = passwordPool._pool[value].camera.targetTexture;

            objectTextForNameCamera.text = passwordPool._pool[value].nameCamera;
        }
        else
            materialDisplayForOutputCamera.mainTexture = null;
    }
}