using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    private AudioSource bkSource;
    private void Awake()
    {
        instance = this;
        bkSource = GetComponent<AudioSource>();

        //初始化背景音乐
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
    }

    //开关背景音乐
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    //调节背景音乐大小
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }
}
