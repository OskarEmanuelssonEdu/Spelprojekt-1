using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimateObject : MonoBehaviour
{
    // Types of movement
    private enum EaseFunction
    {
        Linear,
        Punch,
        Shake,
        Spring,
        InBack,
        InBounce,
        InCirc,
        InCubic,
        InElastic,
        InExpo,
        InQuad,
        InQuart,
        InQuint,
        InSine,
        InOutBack,
        InOutBounce,
        InOutCirc,
        InOutCubic,
        InOutElastic,
        InOutExpo,
        InOutQuad,
        InOutQuart,
        InOutQuint,
        InOutSine,
        OutBack,
        OutBounce,
        OutCirc,
        OutCubic,
        OutElastic,
        OutExpo,
        OutQuad,
        OutQuart,
        OutQuint,
        OutSine,
    }

    private enum Direction
    {
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerCenter,
        LowerRight
    }

    [Header("Information")]
    [SerializeField]
    [Tooltip("This does nothing. Its only purpose is to help organize the editor.")]
    string myHeader;

    //public TextAnchor myAnchor;
    [Header("Objects")]
    [SerializeField]
    [Tooltip("NOT IMPLEMENTED")]
    private Button myButton;
    [SerializeField]
    [Tooltip("The object to animate.")]
    private GameObject myMovableObject;

    [Header("Animation")]
    [SerializeField]
    [Tooltip("NOT IMPLEMENTED")]
    private EaseFunction myEaseFunction;
    [SerializeField]
    [Tooltip("The object to animate.")]
    private Direction myAnimateFrom;
    [SerializeField]
    [Tooltip("The object to animate.")]
    private Direction myAnimateTo;
    [SerializeField]
    [Tooltip("NOT IMPLEMENTED")]
    private float myTime;

    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(() => Animate());
    }

    // Update is called once per frame
    void Animate()
    {
        myMovableObject.SetActive(true);

        Vector3 startPosition = Vector3.zero, endPosition = Vector3.zero;

        switch (myAnimateFrom)
        {
            case Direction.UpperLeft:
                startPosition.x = -Screen.width + (Screen.width >> 1);
                startPosition.y = (Screen.height << 1) - (Screen.height >> 1);
                break;
            case Direction.UpperCenter:
                startPosition.x = Screen.width >> 1;
                startPosition.y = (Screen.height << 1) - (Screen.height >> 1);
                break;
            case Direction.UpperRight:
                startPosition.x = (Screen.width << 1) - (Screen.width >> 1);
                startPosition.y = (Screen.height << 1) - (Screen.height >> 1);
                break;
            case Direction.MiddleLeft:
                startPosition.x = -Screen.width + (Screen.width >> 1);
                startPosition.y = Screen.height >> 1;
                break;
            case Direction.MiddleCenter:
                startPosition.x = Screen.width >> 1;
                startPosition.y = Screen.height >> 1;
                break;
            case Direction.MiddleRight:
                startPosition.x = (Screen.width << 1) - (Screen.width >> 1);
                startPosition.y = Screen.height >> 1;
                break;
            case Direction.LowerLeft:
                startPosition.x = -Screen.width + (Screen.width >> 1);
                startPosition.y = -Screen.height + (Screen.height >> 1);
                break;
            case Direction.LowerCenter:
                startPosition.x = Screen.width >> 1;
                startPosition.y = -Screen.height + (Screen.height >> 1);
                break;
            case Direction.LowerRight:
                startPosition.x = (Screen.width << 1) - (Screen.width >> 1);
                startPosition.y = -Screen.height + (Screen.height >> 1);
                break;
            default:
                break;
        }

        myMovableObject.transform.position = startPosition;

        switch (myAnimateTo)
        {
            case Direction.UpperLeft:
                endPosition.x = -Screen.width + (Screen.width >> 1);
                endPosition.y = (Screen.height << 1) - (Screen.height >> 1);
                break;
            case Direction.UpperCenter:
                endPosition.x = Screen.width >> 1;
                endPosition.y = (Screen.height << 1) - (Screen.height >> 1);
                break;
            case Direction.UpperRight:
                endPosition.x = (Screen.width << 1) - (Screen.width >> 1);
                endPosition.y = (Screen.height << 1) - (Screen.height >> 1);
                break;
            case Direction.MiddleLeft:
                endPosition.x = -Screen.width + (Screen.width >> 1);
                endPosition.y = Screen.height >> 1;
                break;
            case Direction.MiddleCenter:
                endPosition.x = Screen.width >> 1;
                endPosition.y = Screen.height >> 1;
                break;
            case Direction.MiddleRight:
                endPosition.x = (Screen.width << 1) - (Screen.width >> 1);
                endPosition.y = Screen.height >> 1;
                break;
            case Direction.LowerLeft:
                endPosition.x = -Screen.width + (Screen.width >> 1);
                endPosition.y = -Screen.height + (Screen.height >> 1);
                break;
            case Direction.LowerCenter:
                endPosition.x = Screen.width >> 1;
                endPosition.y = -Screen.height + (Screen.height >> 1);
                break;
            case Direction.LowerRight:
                endPosition.x = (Screen.width << 1) - (Screen.width >> 1);
                endPosition.y = -Screen.height + (Screen.height >> 1);
                break;
            default:
                break;
        }

        switch (myEaseFunction)
        {
            case EaseFunction.Linear:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseLinear();
                break;
            case EaseFunction.Punch:
                LeanTween.move(myMovableObject, endPosition, myTime).setEasePunch();
                break;
            case EaseFunction.Shake:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseShake();
                break;
            case EaseFunction.Spring:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseSpring();
                break;
            case EaseFunction.InBack:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInBack();
                break;
            case EaseFunction.InBounce:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInBounce();
                break;
            case EaseFunction.InCirc:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInCirc();
                break;
            case EaseFunction.InCubic:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInCubic();
                break;
            case EaseFunction.InElastic:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInElastic();
                break;
            case EaseFunction.InExpo:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInExpo();
                break;
            case EaseFunction.InQuad:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInQuad();
                break;
            case EaseFunction.InQuart:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInQuart();
                break;
            case EaseFunction.InQuint:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInQuint();
                break;
            case EaseFunction.InSine:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInSine();
                break;
            case EaseFunction.InOutBack:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutBack();
                break;
            case EaseFunction.InOutBounce:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutBounce();
                break;
            case EaseFunction.InOutCirc:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutCirc();
                break;
            case EaseFunction.InOutCubic:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutCubic();
                break;
            case EaseFunction.InOutElastic:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutElastic();
                break;
            case EaseFunction.InOutExpo:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutExpo();
                break;
            case EaseFunction.InOutQuad:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutQuad();
                break;
            case EaseFunction.InOutQuart:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutQuart();
                break;
            case EaseFunction.InOutQuint:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutQuint();
                break;
            case EaseFunction.InOutSine:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseInOutSine();
                break;
            case EaseFunction.OutBack:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutBack();
                break;
            case EaseFunction.OutBounce:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutBounce();
                break;
            case EaseFunction.OutCirc:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutCirc();
                break;
            case EaseFunction.OutCubic:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutCubic();
                break;
            case EaseFunction.OutElastic:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutElastic();
                break;
            case EaseFunction.OutExpo:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutExpo();
                break;
            case EaseFunction.OutQuad:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutQuad();
                break;
            case EaseFunction.OutQuart:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutQuart();
                break;
            case EaseFunction.OutQuint:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutQuint();
                break;
            case EaseFunction.OutSine:
                LeanTween.move(myMovableObject, endPosition, myTime).setEaseOutSine();
                break;
        }
    }
}
