using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class Test : MonoBehaviour
{
    [SerializeField]
    private MoveController player;

    [Space(10)]
    [SerializeField]
    private RaycastControl raycast;

    [Space(10)]
    [SerializeField]
    private GameObject crosshair;

    [Space(10)]
    [SerializeField]
    private Animation animations;

    [Space(10)]
    [SerializeField]
    private AnimationClip startClip;

    [Space(5)]
    [SerializeField]
    private AnimationClip continueClip;

    [Space(5)]
    [SerializeField]
    private AnimationClip endClip;

    [Space(10)]
    [SerializeField]
    private float timeBlendWhenChangeVCamera;

    [Space(10)]
    [SerializeField] 
    private CinemachineVirtualCamera mainVirtualCamera;

    [Space(10)]
    [SerializeField]
    private CinemachineVirtualCamera supportVirtualCamera;

    [Space(5)]
    [SerializeField]
    private Vector3 supportVirtualCameraPosition;

    [Space(5)]
    [SerializeField]
    private Vector3 supportVirtualCameraRotation;

    [Space(10)]
    [SerializeField]
    private Text textScore;

    [Space(5)]
    [SerializeField]
    private Text textQuestion;

    [Space(5)]
    [SerializeField]
    private Text[] buttonAnswer;

    [Space(10)]
    [SerializeField]
    private QuestionList[] listQuestion;

    private QuestionList currentQuestion;

    private List<object> list;

    private TextTips textTips;

    private Animator animatorPlayer;

    private CinemachinePOV mainPOV;

    public float mainValueX;

    public float mainValueY;

    public float mainValueHorizontalMin;

    public float mainValueHorizontalMax;

    private float mainValueVerticalMin;

    private float mainValueVerticalMax;



    private int randomQuestion;

    private int randomAnswer;

    private int score = 0;

    private bool yes = true;

    private void Start()
    {
        list = new List<object>(listQuestion);

        animations.clip = startClip;

        animatorPlayer = player.GetComponent<Animator>();

        textTips = GetComponent<TextTips>();

        mainPOV = mainVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void QuestionGenerator()
    {
        if(list.Count >= 1)
        {
            raycast.TimeBlendCinemachine(timeBlendWhenChangeVCamera);

            StartCoroutine(CursorControl.Control(timeBlendWhenChangeVCamera, CursorLockMode.Confined, true, 1));
            //StartCoroutine(raycast.CursorManagment(timeBlendWhenChangeVCamera, CursorLockMode.Confined, true));

            ChangeVirtualCamera(false, true);

            StartCoroutine(UseCan(false, "", timeBlendWhenChangeVCamera));

            crosshair.SetActive(false);

            //player.enabled = false;

            //animatorPlayer.enabled = false;

            animations.Play();

            randomQuestion = Random.Range(0, list.Count);

            currentQuestion = list[randomQuestion] as QuestionList;

            textQuestion.text = currentQuestion.question;

            List<string> currentAnswers = new List<string>(currentQuestion.answer);

            for (int i = 0; i < currentQuestion.answer.Length; i++)
            {
                randomAnswer = Random.Range(0, currentAnswers.Count);

                buttonAnswer[i].text = currentAnswers[randomAnswer];

                currentAnswers.RemoveAt(randomAnswer);
            }
        }
        else
        {
            Debug.Log("Кончились вопросы");
        }
    }

    public void Click(int index)
    {
        if (list.Count >= 1)
        {
            if (buttonAnswer[index].text.ToString() == currentQuestion.answer[0]) score += 1;
            else Debug.Log("Не правильный ответ");

            textScore.text = score.ToString();

            animations.clip = continueClip;

            animations.Play();

            list.RemoveAt(randomQuestion);

            QuestionGenerator();
        }
        else
        {
            Debug.Log("Нету вопросов");

            animations.clip = endClip;

            animations.Play();

            crosshair.SetActive(true);

            ChangeVirtualCamera(true, false);

            StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
            //StartCoroutine(raycast.CursorManagment(0f, CursorLockMode.Locked, false));

            StartCoroutine(UseCan(true, "", timeBlendWhenChangeVCamera));
        }
    }

    private IEnumerator UseCan(bool use, string textTip, float timeOfDelay)
    {
        //textTips.text = null;

        if (use == true)
        {
            yield return new WaitForSeconds(timeOfDelay);

            player.enabled = use;

            animatorPlayer.enabled = use;

            yield return new WaitForSeconds(.01f);

            mainPOV.m_HorizontalAxis.m_MinValue = mainValueHorizontalMin;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueHorizontalMax;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueVerticalMin;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueVerticalMax;

            //textTips.use = use;

            //textTips.text = textTip;
            StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
            //StartCoroutine(raycast.CursorManagment(0f, CursorLockMode.Locked, false));
        }
        else
        {
            player.enabled = use;

            animatorPlayer.enabled = use;

            mainPOV.m_HorizontalAxis.m_MinValue = mainValueX;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueX;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueY;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueY;

            yield return new WaitForSeconds(timeOfDelay);

            mainPOV.m_HorizontalAxis.m_MinValue = mainValueHorizontalMin;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueHorizontalMax;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueVerticalMin;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueVerticalMax;

            //textTips.use = use;

            //textTips.text = textTip;
        }
    }

    //private IEnumerator CinemachineDefault(float timeOfDelay)
    //{
    //    yield return new WaitForSeconds(timeBlendWhenChangeVCamera);

    //    player.enabled = true;

    //    animatorPlayer.enabled = true;

    //    mainPOV.m_HorizontalAxis.m_MinValue = mainValueX;
    //    mainPOV.m_HorizontalAxis.m_MaxValue = mainValueX;

    //    mainPOV.m_VerticalAxis.m_MinValue = mainValueY;
    //    mainPOV.m_VerticalAxis.m_MaxValue = mainValueY;

    //    yield return new WaitForSeconds(timeOfDelay);

    //    mainPOV.m_HorizontalAxis.m_MinValue = mainValueHorizontalMin;
    //    mainPOV.m_HorizontalAxis.m_MaxValue = mainValueHorizontalMax;

    //    mainPOV.m_VerticalAxis.m_MinValue = mainValueVerticalMin;
    //    mainPOV.m_VerticalAxis.m_MaxValue = mainValueVerticalMax;
    //}

    private void ChangeVirtualCamera(bool mainVCam, bool sit)
    {
        mainVirtualCamera.gameObject.SetActive(mainVCam);

        supportVirtualCamera.gameObject.SetActive(!mainVCam);

        supportVirtualCamera.transform.SetPositionAndRotation(supportVirtualCameraPosition,
            Quaternion.Euler(supportVirtualCameraRotation));

        mainValueX = mainPOV.m_HorizontalAxis.Value;
        mainValueY = mainPOV.m_VerticalAxis.Value;

        if (sit == true)
        {
            if (yes == true)
            {
                mainValueHorizontalMin = mainPOV.m_HorizontalAxis.m_MinValue;
                mainValueHorizontalMax = mainPOV.m_HorizontalAxis.m_MaxValue;

                mainValueVerticalMin = mainPOV.m_VerticalAxis.m_MinValue;
                mainValueVerticalMax = mainPOV.m_VerticalAxis.m_MaxValue;
            }

            yes = false;

            raycast.TimeBlendCinemachine(timeBlendWhenChangeVCamera);

            StartCoroutine(raycast.CanStand(0f, true, true));

            StartCoroutine(CursorControl.Control(timeBlendWhenChangeVCamera, CursorLockMode.Confined, true, 1));
            //StartCoroutine(raycast.CursorManagment(timeBlendWhenChangeVCamera, CursorLockMode.Confined, true));
        }
        else
        {
            //mainValueMin = mainPOV.m_HorizontalAxis.m_MinValue;
            //mainValueMax = mainPOV.m_HorizontalAxis.m_MaxValue;

            mainPOV.m_HorizontalAxis.m_MinValue = mainValueX;
            mainPOV.m_HorizontalAxis.m_MaxValue = mainValueX;

            mainPOV.m_VerticalAxis.m_MinValue = mainValueY;
            mainPOV.m_VerticalAxis.m_MaxValue = mainValueY;

            mainPOV.m_HorizontalAxis.Value = mainValueX;
            mainPOV.m_VerticalAxis.Value = mainValueY;

            mainPOV.m_HorizontalAxis.m_Wrap = false;

            StartCoroutine(raycast.CanStand(timeBlendWhenChangeVCamera, true, false));

            StartCoroutine(CursorControl.Control(0f, CursorLockMode.Locked, false, 1));
            //StartCoroutine(raycast.CursorManagment(0f, CursorLockMode.Locked, false));
        }

        crosshair.SetActive(mainVCam);
    }
}

[System.Serializable]
public class QuestionList
{
    public string question;

    public string[] answer;
}