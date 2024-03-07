using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//：
public class TowerBtn : MonoBehaviour
{
    public Image imgTower;

    public Text txtTip;

    public Text txtMoney;
    /// <summary>
    /// 外部初始化按钮信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="inputStr"></param>
    public void InitInfo(int id,string inputStr)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        imgTower.sprite = Resources.Load<Sprite>(info.imgRes);
        txtMoney.text = "￥"+info.money;
        txtTip.text = inputStr;

        if(info.money>GameLevelMgr.Instance.player.money)
        {
            txtMoney.text = "金钱不足";
        }
    }
}
