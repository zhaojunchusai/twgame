using System.Collections.Generic;
using Assets.Script.Common;
using UnityEngine;
using System.Collections;

public class MultiCreateAcc : MonoBehaviour
{
    public string PreFix;
    public int Count;
    public int StartIndex;
    private List<string> list = new List<string>();
    private int index = 0;
    void Start()
    {
        list.Clear();
    }
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 100), "Creat"))
        {
            for (int i = 0; i < Count; i++)
            {
                list.Add(string.Format("{0}{1:D2}", PreFix, i + StartIndex));
            }
            CreateAcc();
        }
    }
    void CreateAcc()
    {
        Debug.LogWarning(index);
        Scheduler.Instance.RemoveTimer(CreateAcc);
        RegisterAccount();
        //Scheduler.Instance.AddTimer(1, false, EnterGameServer);
        Scheduler.Instance.AddTimer(3, false, CreateRole);
        Scheduler.Instance.AddTimer(5, false, LevelUp);
        Scheduler.Instance.AddTimer(7, false, Main.Instance.LoginOut);
        Scheduler.Instance.AddTimer(7.1f, false, IndexFix);
        if (index < list.Count - 1)
            Scheduler.Instance.AddTimer(7.2f, false, CreateAcc);
    }

    void IndexFix()
    {
        index++;
    }

    void RegisterAccount()
    {
        LoginModule.Instance.RegisterAccountReq(list[index], "123456");
    }

    void EnterGameServer()
    {
        LoginModule.Instance.SetGameNetWork(LoginModule.Instance.CurServer);
        LoginModule.Instance.SendEnterGameServer();
    }

    void CreateRole()
    {
        LoginModule.Instance.SendCreateCharacter(1, list[index], 0);
    }

    void LevelUp()
    {
        int lv = UnityEngine.Random.Range(36, 80);
        GMModule.Instance.SendGMCommandReq(string.Format("level {0}", lv));
    }
}
