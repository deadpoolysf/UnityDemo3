using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class PlayerObject : MonoBehaviour
{
    //1.玩家属性初始化
    //攻击力
    private int atk;
    //玩家拥有的钱
    public int money;
    //旋转速度
    private float roundSpeed = 100;

    //持枪对象的开火点
    public Transform gunPoint;

    private Animator anim;

    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;

        //初始化时更新界面上钱的数量
        UpdateMoney();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //2.移动变化,动作变化
        anim.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        anim.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        //旋转
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        //蹲下
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetLayerWeight(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetLayerWeight(1, 0);
        }

        //翻滚
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Roll");
        }

        //攻击
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Fire");
        }
    }

    /// <summary>
    /// 处理近战武器
    /// </summary>
    public void KnifeEvent()
    {
        GameDataMgr.Instance.PlaySound(GameDataMgr.Instance.nowSelRole.sound);
        //进行伤害检测（范围检测）
        //得到怪物的碰撞器数组
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));
        for (int i = 0; i < colliders.Length; i++)
        {
            //得到怪物脚本，让其受伤
            MonsterObject monster = colliders[i].GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead) 
            {
                monster.Wound(this.atk);
                break;
            }
        }
    }

    public void ShootEvent()
    {
        GameDataMgr.Instance.PlaySound(GameDataMgr.Instance.nowSelRole.sound);
        //进行射线检测
        //RaycastHit[] hits = Physics.RaycastAll(new Ray(gunPoint.position, gunPoint.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        //for (int i = 0; i < hits.Length; i++)
        //{
        //    //得到怪物脚本，让其受伤
        //    MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
        //    if (monster != null)
        //        monster.Wound(this.atk);
        //}
        Collider[] colliders = Physics.OverlapBox(this.transform.position + this.transform.forward*5 + this.transform.up, new Vector3(1,1,5),Quaternion.identity, 1 << LayerMask.NameToLayer("Monster"));
        for (int i = 0; i < colliders.Length; i++)
        {
            //得到怪物脚本，让其受伤
            MonsterObject monster = colliders[i].GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                //创建特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = colliders[i].transform.position + Vector3.up;
                effObj.transform.rotation = Quaternion.LookRotation(colliders[i].transform.forward);
                Destroy(effObj, 1);
                monster.Wound(this.atk);
                break;
            }
        }
    }

    //钱变化逻辑
    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    //击杀怪物得钱
    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
