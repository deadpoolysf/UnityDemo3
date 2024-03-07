using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//：
public class ChooseHeroPanel : BasePanel
{
    //左右键
    public Button btnLeft;
    public Button btnRight;

    //购买按钮
    public Button btnUnlock;
    public Text txtUnlock;

    //开始和返回
    public Button btnStart;
    public Button btnBack;

    //拥有的钱
    public Text txtMoney;

    //角色名
    public Text txtName;

    //预制体初始化位置
    private Transform heroPos;
    //当前场景显示的对象
    private GameObject heroObj;
    //当前角色数据
    private RoleInfo nowRoleData;
    //当前角色索引
    private int nowIndex;

    public override void Init()
    {
        //找到初始化位置
        heroPos = GameObject.Find("HeroPos").transform;
        //初始化玩家拥有的钱
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        //左右键
        btnLeft.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            //模型的更新
            ChangeHero();
        });
        btnRight.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            ++nowIndex;
            if (nowIndex > GameDataMgr.Instance.roleInfoList.Count - 1)
                nowIndex = 0;
            //模型的更新
            ChangeHero();
        });

        //购买
        btnUnlock.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            PlayerData data = GameDataMgr.Instance.playerData;
            //有钱购买时
            if(data.haveMoney >= nowRoleData.lockMoney)
            {
                //扣钱
                data.haveMoney -= nowRoleData.lockMoney;
                //更新显示
                txtMoney.text = data.haveMoney.ToString();
                //记录购买id
                data.buyHero.Add(nowRoleData.id);
                //保存当前数据
                GameDataMgr.Instance.SavePlayerData();
                //更新解锁按钮
                UpdateLockBtn();

                //提示面板，购买成功
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功！");
            }
            else
            {
                //提示面板，购买失败
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足！");
            }
        });

        //开始和返回
        btnStart.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //记录当前选择角色
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            //隐藏当前面板，显示选择场景面板
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });
        btnBack.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //切换摄像机动画，返回开始面板
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
            //隐藏自己
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
        });

        //更新模型
        ChangeHero();
    }

    private void ChangeHero()
    {
        //先删除，再创建
        if(heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        //获取当前索引值指向的角色数据
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        //加载到场景，并记录下，用于下次删除
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        //移除脚本PlayerObject
        Destroy(heroObj.GetComponent<PlayerObject>());

        txtName.text = nowRoleData.tips;

        //根据解锁信息，来决定是否显示解锁按钮
        UpdateLockBtn();
    }

    private void UpdateLockBtn()
    {
        //如果该角色需要解锁并且没有解锁的话，就应该显示解锁按钮，并隐藏开始按钮
        if (nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            btnUnlock.gameObject.SetActive(true);
            txtUnlock.text = "￥:" + nowRoleData.lockMoney;
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnlock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        //每次关闭面板时删除当前人物模型
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
