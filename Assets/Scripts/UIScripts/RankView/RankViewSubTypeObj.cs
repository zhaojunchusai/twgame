using UnityEngine;
using System;
using System.Collections.Generic;

public class RankViewSubTypeObj : RankViewTypeBase
{
    private enum EFromUI
    {
        Rank = 1,
        Recycle = 2,
    }

    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UIGrid Grd_Grid;
    private bool _initialized = false;

    private Transform _selectedSpt;
    private GameObject _subTypeBtn;
    private List<GameObject> btns = new List<GameObject>();
    public bool IsOpen = false;

    private void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Grd_Grid = transform.FindChild("Grid").gameObject.GetComponent<UIGrid>();
    }

    public void Init(Transform selected,GameObject subBtn,RankviewData data,Action<uint> clickType,int fromUI = 1)
    {
        Initialize();
        _selectedSpt = selected;
        _subTypeBtn = subBtn;
        _data = data;
        _clickType = clickType;

        if (_data.HasChild)
        {
            foreach (uint item in _data.ChildIDs )
	        {
                GameObject go = CommonFunction.InstantiateObject(subBtn, Grd_Grid.transform);
                UIEventListener.Get(go).onClick = ClickTypeBtn;
                go.name = item.ToString();
                RankviewData tmp = (EFromUI)fromUI == EFromUI.Rank
                    ? ConfigManager.Instance.mRankviewConfig.GetRankviewDataByID(item)
                    : ConfigManager.Instance.mRecycleViewConfig.GetRankviewDataByID(item);
                go.GetComponentInChildren<UILabel>().text = tmp.Name;
                btns.Add(go);
	        }

            Grd_Grid.Reposition();
            SetBGHeight(_data.ChildIDs.Count);
        }
    }

    private void SetBGHeight(int count)
    {
        if (count <= 1)
        {
            Spt_BG.height = 66;
        }
        else
        {
            Spt_BG.height = 66 + (count - 1) * 60;
        }
    }

    public void SwitchState()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        IsOpen = true;
        gameObject.SetActive(true);
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(SetActiveFalse);
        iTween.ScaleTo(gameObject, iTween.Hash("time", 0.25f, "y", 1, "easetype", iTween.EaseType.linear));
        if (btns.Count > 0)
            ClickTypeBtn(btns[0]);
    }

    public void Close()
    {
        IsOpen = false;
        iTween.ScaleTo(gameObject, iTween.Hash("time", 0.25f, "y",0.001f,"easetype", iTween.EaseType.linear));
        Assets.Script.Common.Scheduler.Instance.AddTimer(0.28f,false,SetActiveFalse);
    }
    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    protected override void ClickTypeBtn(GameObject go)
    {
        _selectedSpt.parent = go.transform;
        _selectedSpt.localPosition = Vector3.zero;
        _selectedSpt.localScale = Vector3.one;
        if (_clickType != null)
        {
            _clickType(uint.Parse(go.name));
        }
    }
}
