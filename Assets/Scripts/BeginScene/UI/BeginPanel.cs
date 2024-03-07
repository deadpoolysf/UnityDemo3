using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;

    private ZBAnimator ZB1;
    private ZBAnimator ZB2;
    public override void Init()
    {
        ZB1 = GameObject.Find("ZB1").GetComponent<ZBAnimator>();
        ZB2 = GameObject.Find("ZB2").GetComponent<ZBAnimator>();
        btnStart.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            if (!ZB1.isPlay)
            {
                ZB1.WakeUp();
                ZB2.WakeUp();
            }
            else
            {
                Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
                {
                    UIManager.Instance.ShowPanel<ChooseHeroPanel>();
                });
            }

            //隐藏当前面板
            UIManager.Instance.HidePanel<BeginPanel>();
        });

        btnSetting.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnAbout.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
        });

        btnQuit.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            Application.Quit();
        });
    }
}
