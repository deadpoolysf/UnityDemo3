using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class TowerPoint : MonoBehaviour
{
    //该造塔点关联的塔对象
    private GameObject towerObj = null;
    //当前塔的数据
    public TowerInfo nowTowerInfo = null;
    //可以建造的三个塔的id
    public List<int> chooseIds;

    private void OnTriggerEnter(Collider other)
    {
        //如果最高级，不显示
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //如果不希望显示下方UI，传空
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }

    /// <summary>
    /// 根据id造塔
    /// </summary>
    /// <param name="id"></param>
    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        //如果钱不够，不能造塔
        if (info.money > GameLevelMgr.Instance.player.money)
            return;
        GameDataMgr.Instance.PlaySound("Music/Tower/Create");
        //扣钱
        GameLevelMgr.Instance.player.AddMoney(-info.money);
        //如果有塔(升级)
        //先删除再创建
        if(towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        //实例化
        towerObj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        //初始化
        towerObj.GetComponent<TowerObject>().InitInfo(info);
        //记录当前塔信息
        nowTowerInfo = info;

        //更新游戏界面
        if(nowTowerInfo.nextLev != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }
}
