using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//：
public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        //根据本地存储数据初始化
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;
        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;

        //UI相关数据
        btnClose.onClick.AddListener(()=>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //设置完后关闭界面才存储数据
            GameDataMgr.Instance.SaveMusicData();
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((v)=>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //让背景音乐进行开关
            BKMusic.Instance.SetIsOpen(v);
            //记录开关数据
            GameDataMgr.Instance.musicData.musicOpen = v;
        });
        togSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //记录开关数据
            GameDataMgr.Instance.musicData.soundOpen = v;
        });
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.ChangeValue(v);
            //记录开关数据
            GameDataMgr.Instance.musicData.musicValue = v;
        });
        sliderSound.onValueChanged.AddListener((v) =>
        {
            //记录开关数据
            GameDataMgr.Instance.musicData.soundValue = v;
        });
    }
}
