using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
class Guide : MonoBehaviour
{
    GameObject mark;
    GameObject ui;
    GameObject HighLight;
    private void init()
    {
        ui = Resources.Load("Prefabs/UI/GuidePanel") as GameObject;

        GameObject father = GameObject.Find("Panel");

        GameObject clone = GameObject.Instantiate(ui) as GameObject;
        Transform parent = father.transform;
        if (clone != null && parent != null)
        {
            clone.transform.parent = parent;
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = new Vector3(1, 1, 1);
        }
        HighLight = clone;
    }
    public void Active()
    {
        Scheduler.Instance.AddTimer(0.2f, true, this.Find);
    }
    public void Find()
    {
        mark = GameObject.Find("SoldierIntensifyPanel");
        if (mark != null)
        {
            if (mark.activeSelf)
            {
                init();

                GameObject result = mark.transform.FindChild("FastSelectButton").gameObject;
                UISprite BoundingBox = this.HighLight.transform.FindChild("ShowPanel/BoundingBox").gameObject.GetComponent<UISprite>();
                BoxCollider box = result.GetComponent<BoxCollider>();

                if (BoundingBox != null)
                {
                    BoundingBox.gameObject.SetActive(true);
                    BoundingBox.transform.position = result.transform.position;
                    BoundingBox.width = (int)box.size.x + (int)(20);
                    BoundingBox.height = (int)box.size.y + (int)(20);
                }
                this.mark.SetActive(false);

                result.AddComponent<UIPanel>();
                result.layer = LayerMask.NameToLayer("Guide");
                this.mark.SetActive(true);
                GuideTouchListener.Get(result.gameObject).onClick = (go) =>
                {
                    go.layer = LayerMask.NameToLayer("NGUI");
                    Destroy(go.GetComponent<UIPanel>());
                    GameObject.Destroy(this.HighLight);
                };
                //Destroy(result.GetComponent<UIPanel>());
                Scheduler.Instance.RemoveTimer(this.Find);
            }
        }
        return;
    }
}
public class BaseCondition
{
    public bool isComplete = false;
    public virtual void Initialize(int num)
    {
        return;
    }
    public virtual bool IsCompelete()
    {
        return this.isComplete;
    }
}
public class PlayerLvCondition : BaseCondition
{
    int LevelCondition = 0;
    public override void Initialize(int num)
    {
        this.LevelCondition = num;
        if (PlayerData.Instance._Level >= num)
            this.isComplete = true;
        else
            PlayerData.Instance.LevelUpEvent += this.Instance_LevelUpEvent;
    }

    void Instance_LevelUpEvent(int old, int now)
    {
        this.isComplete = now >= LevelCondition;
        if (isComplete)
            PlayerData.Instance.LevelUpEvent -= this.Instance_LevelUpEvent;
    }
}
public class IsHadMaxSoldier : BaseCondition
{
    public override void Initialize(int num)
    {
        base.Initialize(num);
        foreach(Soldier sd in PlayerData.Instance._SoldierDepot._soldierList)
        {
            if(sd.isMaxLevel())
            {
                this.isComplete = true;
                break;
            }
        }
        if (!this.isComplete)
            PlayerData.Instance._SoldierDepot.SoldierDepotEvent += _SoldierDepot_SoldierDepotEvent;
    }
    void _SoldierDepot_SoldierDepotEvent(SoldierChange change, int Slot = -1, ulong uID = 0)
    {
        if(uID != 0)
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(uID);
            if(sd.isMaxLevel())
            {
                this.isComplete = true;
                PlayerData.Instance._SoldierDepot.SoldierDepotEvent -= this._SoldierDepot_SoldierDepotEvent;
            }
        }
    }
}
public class FightViewCondition : BaseCondition
{
    public enum FightViewType
    {
        /// <summary>
        /// 进攻城堡
        /// </summary>
        FightTower = 1,
        /// <summary>
        /// 护送模式
        /// </summary>
        Protect = 2,
        /// <summary>
        /// 保护传送门模式
        /// </summary>
        ProtectDoor = 3
    }
    public FightViewType type;
    public override void Initialize(int num)
    {
        this.type = (FightViewType)num;
    }
}