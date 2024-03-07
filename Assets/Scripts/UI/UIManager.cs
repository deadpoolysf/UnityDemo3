using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //单例模式
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    //私有构造函数
    private UIManager()
    {
        //得到场景中的canvas父对象（动态加载）
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //过场景不移出，保证整个游戏过程中只有一个canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }

    //用于存储显示的面板的容器
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //获得canvas父对象
    private Transform canvasTrans;

    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //泛型T的类型和面板预设体的名字一样
        string panelName = typeof(T).Name;

        //判断字典中是否显示面板
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;   //里氏替换，父类装子类

        //没有显示，根据名字动态创建面板
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvasTrans, false);

        //获取面板脚本，并存储
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        //调用自己的显示逻辑
        panel.ShowMe();

        return panel;

    }
    //隐藏面板
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //判断字典中有无对应的面板
        if (panelDic.ContainsKey(panelName))
        {
            //是否需要淡出后删除
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典中的面板
                    panelDic.Remove(panelName);
                });
            }
            else  //直接删除
            {
                //删除对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除字典中的面板
                panelDic.Remove(panelName);
            }
        }
    }

    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        //如果没有找到
        return null;
    }
}
