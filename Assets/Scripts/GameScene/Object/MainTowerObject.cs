using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//：
public class MainTowerObject : MonoBehaviour
{
    private int hp;
    private int maxHp;

    private bool isDead = false;

    //单例模式
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;
    private void Awake()
    {
        instance = this;
    }

    //更新血量
    public void UpdateHp(int hp,int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;

        //更新界面上的显示
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHp);
    }

    //自己收到伤害
    public void Wound(int dmg)
    {
        if (isDead)
            return;
        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
            //游戏结束
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(10, false);
        }
        //更新血量
        UpdateHp(hp, maxHp);
    }

    //过场景删除单例引用
    private void OnDestroy()
    {
        instance = null;
    }
}
