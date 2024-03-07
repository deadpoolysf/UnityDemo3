using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class TowerObject : MonoBehaviour
{
    //可旋转头部
    public Transform head;
    //开火点
    public Transform gunPoint;

    //旋转速度
    private float roundSpeed = 50f;

    //关联的数据
    private TowerInfo info;

    //当前攻击目标
    private MonsterObject targetObj;
    private List<MonsterObject> targetObjs;
    //记录怪物位置
    private Vector3 monsterPos;

    //用于计时，判断攻击间隔时间
    private float atkTime;

    /// <summary>
    /// 初始化炮台数据
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    private void Update()
    {
        //攻击模式
        //单体攻击
        if(info.atkType == 1)
        {
            //开始寻找目标条件：无目标||目标死亡||目标远离范围
            if (targetObj == null ||
                targetObj.isDead ||
                Vector3.Distance(this.transform.position, targetObj.transform.position) > info.atkRange)
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);
            }

            if (targetObj == null)
                return;

            //设置炮台头部高度
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            //旋转炮台
            //head.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(monsterPos - head.position), roundSpeed * Time.deltaTime);
            head.rotation = Quaternion.LookRotation(monsterPos - head.position);
            //当夹角小于5时开火，并且按攻击间隔攻击
            if (Vector3.Angle(head.forward, monsterPos - head.position) < 5 && atkTime > info.offsetTime)
            {
                atkTime = 0;
                //让目标受伤
                targetObj.Wound(info.atk);
                //播放音效
                GameDataMgr.Instance.PlaySound(info.sound);
                //播放特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff),gunPoint.position,gunPoint.rotation);
                Destroy(effObj, 0.2f);
            }
        }   
        //群体攻击
        else
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);
            //如果目标不为0，按间隔时间攻击
            if (targetObjs.Count > 0 && atkTime > info.offsetTime) 
            {
                atkTime = 0;
                //让所有目标受伤
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(info.atk);
                }
                //播放音效
                GameDataMgr.Instance.PlaySound(info.sound);
                //播放特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                Destroy(effObj, 0.2f);
            }
        }
        atkTime += Time.deltaTime;
    }
}
