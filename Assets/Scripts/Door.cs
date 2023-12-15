using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class Door : MonoBehaviour
{
    [SerializeField]
    private string textOpen;

    [SerializeField]
    private string textClose;

    [Space(10)]
    [SerializeField] 
    private float eulerOpen;

    [SerializeField] 
    private float eulerClose;

    [Space(10)]
    [SerializeField] 
    private float timeOpenClose;

    private TextTips textTips;

    private bool isOpen;

    private void Start()
    {
        textTips = GetComponent<TextTips>();

        StartCoroutine(textTips.ChangeText(0f, textOpen));
    }

    private void Update()
    {
        if (isOpen) { OpenOrCloseDoor(eulerOpen); }
        else { OpenOrCloseDoor(eulerClose); }
    }

    private void OpenOrCloseDoor(float newEulerAngles)
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(transform.localEulerAngles.x, newEulerAngles, transform.localEulerAngles.z), timeOpenClose * Time.deltaTime);
    }

    public void OpenorClose()
    {
        isOpen = !isOpen;

        if (isOpen) { StartCoroutine(textTips.ChangeText(0f, textClose)); }
        else { StartCoroutine(textTips.ChangeText(0f, textOpen)); }
    }
}