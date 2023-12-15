using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class TextTips : MonoBehaviour
{
    [SerializeField]
    private bool use = true;

    //private bool canUseWireTypeInput = true;

    //private bool canUseWireTypeOutput = true;

    [Min(0.01f), Space(5)]
    [SerializeField]
    private float distanceToUse = 1f;

    [Space(5)]
    [SerializeField]
    private TypeObject typeObject;

    [Space(5)]
    [SerializeField, HideIf(nameof(typeObject), TypeObject.ObjectWithOneAction), ReadOnly]
    private IsActiveText isActiveText;

    [Space(5)]
    [Multiline]
    [SerializeField, ShowIf(nameof(typeObject), TypeObject.ObjectWithOneAction)]
    private string textAction;

    [Space(5)]
    [Multiline]
    [SerializeField, ShowIf(nameof(typeObject), TypeObject.ObjectWithTwoAction)]
    private string textFirstAction;

    [Space(5)]
    [Multiline]
    [SerializeField, ShowIf(nameof(typeObject), TypeObject.ObjectWithTwoAction)]
    private string textSecondAction; 

    [Space(5)]
    [Multiline]
    [SerializeField, ShowIf(nameof(typeObject), TypeObject.Input)]
    private string textInputIsNotBusy;

    [Space(5)]
    [Multiline]
    [SerializeField, ShowIf(nameof(typeObject), TypeObject.Input)]
    private string textInputIsBusy;

    [Space(5)]
    [Multiline]
    [SerializeField, ShowIf(nameof(typeObject), TypeObject.Output)]
    private string textOutputIsNotBusy;[Space(5)]


    [SerializeField]
    private bool isBusy = false;

    [SerializeField, ReadOnly]
    private Transform isWire;



    //private CreateMesh isWire;

    private string textForFirstAction;

    private string textForSecondAction;

    public bool _use { get { return use; } }

    //public bool _canUseWireTypeInput { get { return canUseWireTypeInput; } }

    //public bool _canUseWireTypeOutput { get { return canUseWireTypeOutput; } }

    public float _distanceToUse { get { return distanceToUse; } }

    public bool _isBusy { get { return isBusy; } }

    public string _textForFirstAction 
    { 
        get 
        {
            switch (typeObject)
            {
                case TypeObject.ObjectWithOneAction:
                    textForFirstAction = textAction;
                    return textForFirstAction;
                case TypeObject.ObjectWithTwoAction:
                    textForFirstAction = textFirstAction;
                    textForSecondAction = textSecondAction;
                    return textForFirstAction;
                case TypeObject.Input:
                    textForFirstAction = textInputIsNotBusy;
                    textForSecondAction = textInputIsBusy;
                    return textForFirstAction;
                case TypeObject.Output:
                    textForFirstAction = textOutputIsNotBusy;
                    return textForFirstAction;
                default:
                    return null;
            }
        } 
    }

    public string _textForSecondAction { get { return textForSecondAction; } }

    //public CreateMesh _isWire { get { return isWire; } }

    public Transform _isWire { get { return isWire; } }

    public TypeObject _typeObject { get { return typeObject; } }

    public enum TypeObject { ObjectWithOneAction, ObjectWithTwoAction, Input, Output, }

    public IsActiveText _isActiveText { get { return isActiveText; } }

    public enum IsActiveText { First, Second }

    public void ChangeActiveText()
    {
        switch (isActiveText)
        {
            case IsActiveText.First:
                isActiveText = IsActiveText.Second;
                break;
            case IsActiveText.Second:
                isActiveText = IsActiveText.First;
                break;
        }
    }

    public void ChangeIsBusy(bool value)
    {
        isBusy = value;
    }

    public void SetWire(Transform gameObject)
    {
        isWire = gameObject;
    }

    public IEnumerator ChangeUse(float delay, bool Use)
    {
        yield return new WaitForSeconds(delay);

        use = Use;
    }

    //public IEnumerator ChangeCanUseWire(float delay, bool CanUseWireTypeA, bool CanUseWireTypeB)
    //{
    //    yield return new WaitForSeconds(delay);

    //    canUseWireTypeInput = CanUseWireTypeA;

    //    canUseWireTypeOutput = CanUseWireTypeB;
    //}

    public IEnumerator ChangeText(float delay, string Text)
    {
        yield return new WaitForSeconds(delay);

        textForFirstAction = Text;
    }

    //public void ChangeWire(CreateMesh IsWire)
    //{
    //    isWire = IsWire;
    //}

    public void ChangeDistance(float Distance)
    {
        distanceToUse = Distance;
    }
}