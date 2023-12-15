using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(TextTips))]
public class BedSideTable : MonoBehaviour
{
    [SerializeField]
    private string textOpen;

    [SerializeField]
    private string textClose;

    [Space(10)]
    [SerializeField] 
    private float positionOpen;

    [Min(0.001f)]
    [SerializeField] 
    private float timeOpenClose;

    private TextTips textTips;

    private float positionClose;

    private bool isOpen;

    private void Start()
    {
        textTips = GetComponent<TextTips>();

        positionClose = transform.localPosition.z;

        StartCoroutine(textTips.ChangeText(0f, textOpen));
    }

    private void Update()
    {
        if(isOpen) { OpenOrCloseBox(positionOpen); }
        else { OpenOrCloseBox(positionClose); }
    }

    private void OpenOrCloseBox(float newPosition)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, newPosition), timeOpenClose * Time.deltaTime);
    }

    public void OpenOrClose()
    {
        isOpen = !isOpen;

        if (isOpen) { StartCoroutine(textTips.ChangeText(0f, textClose)); }
        else { StartCoroutine(textTips.ChangeText(0f, textOpen)); }
    }
}