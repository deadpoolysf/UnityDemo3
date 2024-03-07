using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//：
public class ZBAnimator : MonoBehaviour
{
    private Animator anim;
    public bool isPlay = false;
    private UnityAction overAction;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void WakeUp()
    {
        anim.SetTrigger("Start");
        GameDataMgr.Instance.PlaySound("Music/Zonbie/Wake");
        StartCoroutine(TurnLeft());
    }


    private IEnumerator TurnLeft()
    {
        yield return new WaitForSeconds(3.2f);
        Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
        {
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        
        isPlay = true;
    }
}
