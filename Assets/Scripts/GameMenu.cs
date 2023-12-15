using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameMenu : MonoBehaviour
{
    [Space(10)]
    [SerializeField] 
    private Canvas mainPage;

    [Space(5)]
    [SerializeField] 
    private Canvas settingsPage;

    [Space(10)]
    [SerializeField] 
    private CinemachineVirtualCamera supportVirtualCamera;

    [Space(10)]
    [SerializeField]
    private KeyCode buttonKeyboard;

    private void Start()
    {
        mainPage.enabled = false;
        settingsPage.enabled = false;

        StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
    }

    private void Update()
    {
        if (settingsPage.enabled == false)
        {
            if (Input.GetKeyDown(buttonKeyboard))
            {
                mainPage.enabled = !mainPage.enabled;

                if (mainPage.enabled == true)
                {
                    StartCoroutine(CursorControl.Control(0f, CursorLockMode.Confined, true, 0));
                }
                else
                {
                    StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));

                    if (supportVirtualCamera.gameObject.activeSelf == true)
                    {
                        StartCoroutine(CursorControl.Control(0f, CursorLockMode.Confined, true, 1));
                    }
                    else
                    {
                        StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
                    }
                }
            }
        }
        else
        {
            mainPage.enabled = true;
        }
    }

    public void TransitionOnScene(int numberScene)
    {
        SceneManager.LoadScene(numberScene);

        StartCoroutine(CursorControl.Control(0f, CursorLockMode.Confined, true, 1));
    }
}