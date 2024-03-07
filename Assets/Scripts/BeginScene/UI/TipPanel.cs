using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//：
public class TipPanel : BasePanel
{
    public Text txtInfo;
    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }

    public void ChangeInfo(string str)
    {
        txtInfo.text = str;
    }
}
