using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Space(10)]
    [SerializeField] 
    private Slider changeSensivitityMouse;

    [Space(10)]
    [SerializeField] 
    private Text textSensivitityMouse;

    [Space(10)]
    [SerializeField] 
    private Dropdown changeResolutionScreen;

    [Space(10)]
    [SerializeField] 
    private Dropdown changeGraphicsSettings;

    [Space(10)]
    [SerializeField] 
    private CinemachineVirtualCamera mainVirtualCamera;

    private Resolution[] resolutions;

    private void Awake()
    {
        //changeGraphicsSettings.value = QualitySettings.GetQualityLevel();

        changeSensivitityMouse.value = PlayerPrefs.GetFloat("sensivitity", 0);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        string[] textResolution = new string[resolutions.Length];

        for (int i = 0; i < resolutions.Length; i++)
        {
            textResolution[i] = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
        }

        changeResolutionScreen.ClearOptions();
        changeResolutionScreen.AddOptions(textResolution.ToList());

        if(PlayerPrefs.HasKey("resolutions"))
        {
            changeResolutionScreen.value = PlayerPrefs.GetInt("resolutions");
            Screen.SetResolution(resolutions[PlayerPrefs.GetInt("resolutions", changeResolutionScreen.value)].width, resolutions[PlayerPrefs.GetInt("res", changeResolutionScreen.value)].height, true);
        }
        else
        {
            changeResolutionScreen.value = resolutions.Length - 1;
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
        }
    }

    public void EnabledVSync(bool value) => QualitySettings.vSyncCount = value ? 1 : 0;

    public void ChangeGraphics(int value) => QualitySettings.SetQualityLevel(value);

    public void ChangeResolution()
    {
        Screen.SetResolution(resolutions[changeResolutionScreen.value].width, resolutions[changeResolutionScreen.value].height, true);

        PlayerPrefs.SetInt("resolutions", changeResolutionScreen.value);
    }

    public void ChangeSensetive(float valueSensetiveMouse)
    {
        mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 100 * valueSensetiveMouse;
        mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 100 * valueSensetiveMouse;

        textSensivitityMouse.text = changeSensivitityMouse.value.ToString();

        PlayerPrefs.SetFloat("sensivitity", valueSensetiveMouse);
    }
}