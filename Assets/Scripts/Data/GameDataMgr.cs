using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class GameDataMgr 
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;
    private GameDataMgr()
    {
        //构造函数初始化音乐数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //初始化玩家数据
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");

        //读取角色数据
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        //读取场景数据
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        //读取怪物数据
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        //读取塔数据
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    //音效相关数据
    public MusicData musicData;
    //玩家相关数据
    public PlayerData playerData;

    //角色数据
    public List<RoleInfo> roleInfoList;
    //记录选择场景选择的角色数据，用于游戏场景动态创建角色
    public RoleInfo nowSelRole;

    //场景数据
    public List<SceneInfo> sceneInfoList;
    //怪物数据
    public List<MonsterInfo> monsterInfoList;
    //塔数据
    public List<TowerInfo> towerInfoList;

    /// <summary>
    /// 存储音乐数据
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData,"MusicData");
    }

    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.soundValue;
        a.mute = !musicData.soundOpen;
        a.Play();

        GameObject.Destroy(musicObj, 1);
    }
}
