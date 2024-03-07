using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//：
public abstract class BasePanel : MonoBehaviour
{
    //控制面板淡入淡出组件
    private CanvasGroup canvasGroup;
    //淡入淡出速度
    private float alphaSpeed = 10;
    //当前是否显示
    public bool isShow = false;

    //当隐藏完毕后的回调
    private UnityAction hideCallback = null;

    protected virtual void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        //如果当前处于显示状态，但透明度不为1，淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //淡出
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //面板隐藏后调用回调
                hideCallback.Invoke();
            }
        }
    }
    /// <summary>
    /// 注册控件事件的方法 所有的子面板都需要注册一些控件事件
    /// 抽象方法，让子类必须去实现
    /// </summary>
    public abstract void Init();

    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hideCallback = callBack;
    }
}
