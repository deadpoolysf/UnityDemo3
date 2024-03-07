using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//：
public class GamePanel : BasePanel
{
    public Image imgHp;
    public float hpW = 600f;

    public Text txtHP;
    public Text txtWave;
    public Text txtMoney;

    public Button btnQuit;

    //下方造塔组合控件父对象，用于显隐
    public Transform botTrans;
    //管理三个组合控件的脚本
    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    //当前进入的造塔点
    private TowerPoint nowSelTowerPoint;
    //是否检测造塔输入
    private bool checkInput;

    public override void Init()
    {
        btnQuit.onClick.AddListener(()=> 
        {
            GameDataMgr.Instance.PlaySound("Music/UI/1");
            //返回开始界面
            SceneManager.LoadScene("BeginScene");
            //隐藏游戏界面
            UIManager.Instance.HidePanel<GamePanel>();
            //其他
            //清空管理器数据
            GameLevelMgr.Instance.ClearInfo();

            Cursor.lockState = CursorLockMode.None;
        });

        //一开始隐藏下方UI
        botTrans.gameObject.SetActive(false);

        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void Update()
    {
        base.Update();
        //是否显示鼠标
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (!checkInput)
            return;

        //监听进入造塔点的键盘输入
        //如果没有塔，检测123
        if(nowSelTowerPoint.nowTowerInfo == null)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIds[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIds[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIds[2]);
            }
        }
        //如果有塔，检测E升级
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLev);
            }
        }
    }

    //提供给外部的方法
    public void UpdateTowerHp(int hp,int maxHp)
    {
        txtHP.text = hp + "/" + maxHp;
        //更新血条长度
        (imgHp.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHp * hpW, 40);
    }

    public void UpdateWaveNum(int nowNum,int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }

    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }

    /// <summary>
    /// 更新造塔点界面
    /// </summary>
    /// <param name="point"></param>
    public void UpdateSelTower(TowerPoint point)
    {
        nowSelTowerPoint = point;
        //如果传入为空
        if(nowSelTowerPoint==null)
        {
            //父对象不显示
            botTrans.gameObject.SetActive(false);
            checkInput = false;
        }
        else
        {
            //父对象显示
            botTrans.gameObject.SetActive(true);
            checkInput = true;
            //如果没有塔
            if (nowSelTowerPoint.nowTowerInfo == null)
            {
                //显示三个塔
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIds[i], "按数字键" + (i + 1));
                }
            }
            //有塔就显示塔
            else
            {
                //只显示中间按钮
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                //显示为下一等级的塔
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLev, "按E升级");
            }
        } 
    }
}
