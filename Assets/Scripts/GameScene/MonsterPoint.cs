using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class MonsterPoint : MonoBehaviour
{
    //怪物有多少波
    public int maxWave;
    //每波怪物有多少只
    public int monsterNumOneWave;
    private int nowNum;

    //怪物ID 随机创建
    public List<int> monsterIds;
    private int nowId;

    //单只怪物创建的间隔时间
    public float createOffsetTime;
    //波与波之间的间隔时间
    public float delayTime;
    //第一波怪物的创建间隔时间
    public float firstDelayTime;

    private void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        //添加出怪点
        GameLevelMgr.Instance.AddMonsterPoint(this);
        //更新最大波数
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }

    /// <summary>
    /// 创建一波怪物
    /// </summary>
    private void CreateWave()
    {
        //得到当前波怪物的Id是什么,最后1波出精英怪
        if (maxWave == 1)
            nowId = monsterIds[monsterIds.Count-1];
        else
            nowId = monsterIds[Random.Range(0, monsterIds.Count-1)];

        //当前波怪物有多少只
        nowNum = monsterNumOneWave;
        if (nowId == monsterIds[monsterIds.Count - 1])
        {
            //精英怪少一只
            nowNum--;
        }

        //创建怪物
        CreateMonster();

        //减少波数
        maxWave--;
        GameLevelMgr.Instance.ChangeWaveNum(1);
    }
    /// <summary>
    /// 创建怪物
    /// </summary>
    private void CreateMonster()
    {
        //取出怪物数据
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowId - 1];

        //创建怪物预设体
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();
        monsterObj.InitInfo(info);

        //管理器怪物数+1
        GameLevelMgr.Instance.AddMonster(monsterObj);

        //减去创建的怪物数
        nowNum--;

        //怪物创建完成
        if(nowNum == 0)
        {
            if(maxWave > 0)
            {
                //延迟创建下一波
                Invoke("CreateWave", delayTime);
            }
        }
        else
        {
            //延迟创建下一只
            Invoke("CreateMonster", createOffsetTime);
        }
    }
    /// <summary>
    /// 是否完成所有出怪
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
