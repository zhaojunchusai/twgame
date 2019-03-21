using System.Collections.Generic;
using UnityEngine;
public class ActivitiesDifficultComponent : BaseComponent
{
    public UISprite Spt_BGSprite;
    public UISprite Spt_IconSprite;
    public UILabel Lb_IndexLabel;
    public UISprite Spt_FGSprite;

    private StageData stageData = null;
    public StageData StageData
    {
        get
        {
            return stageData;
        }
    }
    
    public bool IsLock
    {
        get
        {
            if (stageData == null) return true;
            if (PlayerData.Instance._Level < stageData.stageinfo.UnlockLV)
            {
                return true;
            }
            return false;
        }
    }

    public ActivitiesDifficultComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_BGSprite = mRootObject.transform.FindChild("BGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = mRootObject.transform.FindChild("IconSprite").gameObject.GetComponent<UISprite>();
        Lb_IndexLabel = mRootObject.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Spt_FGSprite = mRootObject.transform.FindChild("FGSprite").gameObject.GetComponent<UISprite>();
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateInfo(StageData data)
    {
        if (data == null)
        {
            Clear();
            return;
        }
        stageData = data;
        UpdateDifficult();
        UpdateStatus();
    }

    private void UpdateDifficult()
    {
        string spriteName = string.Format(GlobalConst.SpriteName.ActivityDiffcultIcon, stageData.stageinfo.SmallGateID);
        string indexName = string.Format(GlobalConst.SpriteName.ActivityDiffcultIndex, stageData.stageinfo.SmallGateID);
        CommonFunction.SetSpriteName(Spt_IconSprite,spriteName);
        Spt_IconSprite.MakePixelPerfect();
        Lb_IndexLabel.text = string.Format(ConstString.ACITIVITIES_DIFFICULTY,ConstString.ACITIVITIES_DIFFICULTY_NAME[stageData.stageinfo.SmallGateID - 1]);
    }

    /// <summary>
    /// 更新功能状态
    /// </summary>
    private void UpdateStatus()
    {
        bool isLock = false;
        if (stageData == null) isLock = true;
        if (PlayerData.Instance._Level < stageData.stageinfo.UnlockLV)
        {
            isLock = true;
        }
        Spt_BGSprite.color = isLock ? Color.black : Color.white;
        Spt_FGSprite.color = isLock ? Color.black : Color.white;
        Spt_IconSprite.color = isLock ? Color.black : Color.white;
        Lb_IndexLabel.color = isLock ? CommonFunction.ToColor(170, 170, 170) : CommonFunction.ToColor(253, 234, 145);
    }

    public override void Clear()
    {
        base.Clear();
        stageData = null;
    }
}
