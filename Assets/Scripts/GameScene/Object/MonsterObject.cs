using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//：
public class MonsterObject : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    //配置的基础数据
    private MonsterInfo monsterInfo;

    //当前血量
    private int hp;
    //是否死亡
    public bool isDead = false;

    //攻击计时器
    private float atkTime = 0f;
    //初始化
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        //添加状态机
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //初始血量
        hp = info.hp;
        //速度初始化
        agent.speed = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
        agent.acceleration = info.moveSpeed;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //攻击，伤害检测
        if (isDead)
            return;
        //根据速度来决定动画播放什么
        //速度为0时停止跑步
        anim.SetBool("Run", agent.velocity != Vector3.zero);
        //靠近目标后攻击,按攻击间隔时间
        if (Vector3.Distance(transform.position, MainTowerObject.Instance.transform.position) < 5 && atkTime>monsterInfo.atkOffset)
        {
            atkTime = 0;
            anim.SetTrigger("Atk");
            AtkEvent();
        }
        atkTime += Time.deltaTime;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="dmg"></param>
    public void Wound(int dmg)
    {
        if (isDead)
            return;
        hp -= dmg;
        anim.SetTrigger("Wound");

        if(hp<=0)
        {
            //死亡
            Dead();
        }
        else
        {
            //受伤音效
            GameDataMgr.Instance.PlaySound("Music/Zonbie/Hurt");
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        isDead = true;
        agent.enabled = false;
        anim.SetBool("Dead", true);
        //播放死亡音效
        GameDataMgr.Instance.PlaySound("Music/Zonbie/Dead");
        //移除碰撞体
        Destroy(this.GetComponent<CapsuleCollider>());
        //玩家获得金币，通过关卡管理类
        GameLevelMgr.Instance.player.AddMoney(25);
    }

    /// <summary>
    /// 死亡事件
    /// </summary>
    public void DeadEvent()
    {
        //死亡动画播完后移除对象
        GameLevelMgr.Instance.RemoveMonster(this);
        Destroy(this.gameObject);
        //胜利检测
        if(GameLevelMgr.Instance.CheckOver())
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(100, true);
        }
    }

    /// <summary>
    /// 出生之后再移动
    /// </summary>
    public void BornOver()
    {
        //朝目标点移动
        if(agent.enabled==true)
            agent.SetDestination(MainTowerObject.Instance.transform.position);
        //播放移动动画
        anim.SetBool("Run", true);
    }
    /// <summary>
    /// 攻击事件，伤害检测
    /// </summary>
    public void AtkEvent()
    {
        //范围检测
        Collider[] colliders =  Physics.OverlapSphere(this.transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("MainTower"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            {
                GameDataMgr.Instance.PlaySound("Music/Zonbie/Eat");
                //让保护区受伤害
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
