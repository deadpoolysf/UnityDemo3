using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class GameLevelMgr 
{
    //单例模式
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    private GameLevelMgr()
    {

    }

    //玩家对象脚本
    public PlayerObject player;
    //所有出怪点
    private List<MonsterPoint> points = new List<MonsterPoint>();
    //当前剩余多少波
    private int nowWaveNum = 0;
    //一共多少波
    private int maxWaveNum = 0;
    //记录当前场景的怪物数
    //private int nowMonsterNum = 0;
    //记录当前场景上所有怪物的列表
    private List<MonsterObject> monsterList = new List<MonsterObject>();

    /// <summary>
    /// 切换场景时的初始化
    /// </summary>
    public void InitInfo(SceneInfo sceneInfo)
    {
        //显示游戏界面
        UIManager.Instance.ShowPanel<GamePanel>();

        //玩家的创建
        //获取当前选中的玩家数据
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        //获取玩家出生点
        Transform heroPoint = GameObject.Find("HeroBornPoint").transform;

        //实例化玩家预设体
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),heroPoint.position,heroPoint.rotation);
        //对玩家进行初始化
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, sceneInfo.money);

        //让摄像机看向玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化保护区血量
        MainTowerObject.Instance.UpdateHp(sceneInfo.towerHp, sceneInfo.towerHp);
    }

    //添加出怪点
    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }
    /// <summary>
    /// 更新最大波数
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public void ChangeWaveNum(int num)
    {
        nowWaveNum -= num;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    /// <summary>
    /// 记录场景中的怪物数
    /// </summary>
    /// <param name="num"></param>
    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }

    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }

    /// <summary>
    /// 寻找满足条件的怪物对象
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos,int range)
    {
        for(int i = 0;i<monsterList.Count;i++)
        {
            //如果该怪物没有死亡，且在范围内
            if(!monsterList[i].isDead && Vector3.Distance(pos,monsterList[i].transform.position)<=range)
            {
                return monsterList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 寻找满足条件的所有怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos,int range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }

    /// <summary>
    /// 胜利检测,怪物死亡时检测
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        //是否剩余波数
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver())
                return false;
        }
        //是否剩余怪物
        if (monsterList.Count > 0)
            return false;
        Debug.Log("游戏胜利");
        return true;
    }

    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}
