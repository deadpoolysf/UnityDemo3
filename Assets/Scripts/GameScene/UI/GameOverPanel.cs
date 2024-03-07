using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//：
public class GameOverPanel : BasePanel
{
    public Text txtWin;
    public Text txtInfo;
    public Text txtMoney;
    public Button btnSure;
    public override void Init()
    {
        //点击确定
        btnSure.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //隐藏面板
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.HidePanel<GameOverPanel>();
            //清空管理器数据
            GameLevelMgr.Instance.ClearInfo();
            //切换场景
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int money,bool isWin)
    {
        txtWin.text = isWin ? "胜利" : "失败";
        txtInfo.text = isWin ? "获得通关奖励" : "获得失败奖励";
        txtMoney.text = "￥"+money.ToString();

        //改变玩家数据
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        Time.timeScale = 1;
    }
}
