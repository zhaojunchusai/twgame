using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PrisonMarkViewController : UIBase 
{
    public PrisonMarkView view;
    private List<fogs.proto.msg.EnslaveRecord> tmpList;
    private List<PrisonMarkComponent> PrisonMarkItemList = new List<PrisonMarkComponent>();

    private List<fogs.proto.msg.AltarRecord> tmpAltarList;
    public override void Initialize()
    {
        if (view == null)
            view = new PrisonMarkView();
        view.Initialize();
        BtnEventBinding();
        this.view.Title.text = ConstString.RISONTITLE;
    }
    public void SetInfo()
    {

    }
    #region 奴役系统
    public void InitPrisonMarkItem()
    {
        this.view.UIWrapContent_Grid.onInitializeItem = SetPrisonMarkInfo;
        this.view.Title.text = ConstString.RISONTITLE;
        tmpList = PlayerData.Instance._Prison.GetEnslaveRecord();
        if (this.tmpList == null) return;
        Main.Instance.StartCoroutine(CreatPrisonMarkItem(tmpList));
    }
    private IEnumerator CreatPrisonMarkItem(List<fogs.proto.msg.EnslaveRecord> _data)
    {
        this.view.ScrView_MaterialScroll.ResetPosition();
        yield return 0;
        int MaxCount = 6;
        int count = _data.Count;
        int itemCount = PrisonMarkItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_Grid.minIndex = -index;
        this.view.UIWrapContent_Grid.maxIndex = 0;
        if (count % 2 != 0)
        {
            this.view.UIWrapContent_Grid.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_Grid.cullContent = true;
        }

        if (count > MaxCount)
        {
            this.view.UIWrapContent_Grid.enabled = true;
            if (count % 2 != 0)
            {
                this.view.UIWrapContent_Grid.cullContent = false;
            }
            else
            {
                this.view.UIWrapContent_Grid.cullContent = true;
            }

            count = MaxCount;
        }
        else
        {
            this.view.UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                PrisonMarkItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.Item, this.view.Grd_Grid.transform);
                PrisonMarkComponent item = vGo.GetComponent<PrisonMarkComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<PrisonMarkComponent>();
                    item.MyStart(vGo);
                }
                PrisonMarkItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
            }
            else
            {
                PrisonMarkItemList[i].gameObject.SetActive(true);
            }
            PrisonMarkItemList[i].SetInfo(_data[i]);
        }
        if (count > MaxCount - 2)
            this.view.UIWrapContent_Grid.ReGetChild();
        this.view.Grd_Grid.repositionNow = true;
        yield return 0;
        this.view.ScrView_MaterialScroll.ResetPosition();
    }
    public void SetPrisonMarkInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        PrisonMarkComponent item = PrisonMarkItemList[wrapIndex];
        item.SetInfo(tmpList[realIndex]);
    }
    #endregion
    #region 军团奴役系统
    public void InitUnionPrisonMarkItem()
    {
        this.view.UIWrapContent_Grid.onInitializeItem = SetUnionPrisonMarkInfo;
        this.tmpAltarList = UnionPrisonModule.Instance.altar_records;
        this.view.Title.text = ConstString.UNIONPRISONTITLE;
        if (this.tmpAltarList == null) return;
        Main.Instance.StartCoroutine(CreatUnionPrisonMarkItem(tmpAltarList));
    }
    private IEnumerator CreatUnionPrisonMarkItem(List<fogs.proto.msg.AltarRecord> _data)
    {
        this.view.ScrView_MaterialScroll.ResetPosition();
        yield return 0;
        int MaxCount = 6;
        int count = _data.Count;
        int itemCount = PrisonMarkItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_Grid.minIndex = -index;
        this.view.UIWrapContent_Grid.maxIndex = 0;
        if (count % 2 != 0)
        {
            this.view.UIWrapContent_Grid.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_Grid.cullContent = true;
        }

        if (count > MaxCount)
        {
            this.view.UIWrapContent_Grid.enabled = true;
            if (count % 2 != 0)
            {
                this.view.UIWrapContent_Grid.cullContent = false;
            }
            else
            {
                this.view.UIWrapContent_Grid.cullContent = true;
            }

            count = MaxCount;
        }
        else
        {
            this.view.UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                PrisonMarkItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.Item, this.view.Grd_Grid.transform);
                PrisonMarkComponent item = vGo.GetComponent<PrisonMarkComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<PrisonMarkComponent>();
                    item.MyStart(vGo);
                }
                PrisonMarkItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
            }
            else
            {
                PrisonMarkItemList[i].gameObject.SetActive(true);
            }
            PrisonMarkItemList[i].SetInfo(_data[i]);
        }
        if (count > MaxCount - 2)
            this.view.UIWrapContent_Grid.ReGetChild();
        this.view.Grd_Grid.repositionNow = true;
        yield return 0;
        this.view.ScrView_MaterialScroll.ResetPosition();
    }
    public void SetUnionPrisonMarkInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpAltarList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        PrisonMarkComponent item = PrisonMarkItemList[wrapIndex];
        item.SetInfo(tmpAltarList[realIndex]);
    }
#endregion
    public void ButtonEvent_Button_close(GameObject btn)
    {
        this.Close(null,null);
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        this.view = null;

        if (this.PrisonMarkItemList != null)
            this.PrisonMarkItemList.Clear();
        if (this.tmpList != null)
            this.tmpList = null;
        if(this.tmpAltarList != null)
        {
            this.tmpAltarList.Clear();
            this.tmpAltarList = null;
        }
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Mark.gameObject).onClick = ButtonEvent_Button_close;
    }


}
