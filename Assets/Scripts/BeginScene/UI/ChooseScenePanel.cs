using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//：
public class ChooseScenePanel : BasePanel
{
    //左右键
    public Button btnLeft;
    public Button btnRight;

    //开始和返回
    public Button btnStart;
    public Button btnBack;

    //图片和描述
    public Image imgScene;
    public Text txtInfo;

    //当前索引值
    private int nowIndex;
    //当前选择的数据
    private SceneInfo nowSceneInfo;
    public override void Init()
    {
        //左右键
        btnLeft.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;

            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            ++nowIndex;
            if (nowIndex > GameDataMgr.Instance.sceneInfoList.Count - 1)
                nowIndex = 0;

            ChangeScene();
        });

        //开始和返回
        btnStart.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //关闭当前面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //切换场景(异步加载)
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            ao.completed += (obj) =>
            {
                //场景加载完毕后再初始化，防止同步加载场景未完成就初始化
                //初始化关卡
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            };
        });
        btnBack.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //返回选人面板，关闭当前面板
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            UIManager.Instance.HidePanel<ChooseScenePanel>();
        });

        //刚打开面板时也要初始化
        ChangeScene();
    }

    /// <summary>
    /// 切换界面显示的场景信息
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];
        //更新图片文字信息
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        txtInfo.text = "名称:\n" + nowSceneInfo.name + "\n"+"描述:\n" + nowSceneInfo.tips;
    }
}
