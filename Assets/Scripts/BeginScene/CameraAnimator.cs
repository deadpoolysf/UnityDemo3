using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//：
public class CameraAnimator : MonoBehaviour
{
    private Animator anim;
    //委托
    private UnityAction overAction;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TurnLeft(UnityAction action)
    {
        overAction = action;
        anim.SetTrigger("Left");
    }

    public void TurnRight(UnityAction action)
    {
        overAction = action;
        anim.SetTrigger("Right");
    }

    public void PlayOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
