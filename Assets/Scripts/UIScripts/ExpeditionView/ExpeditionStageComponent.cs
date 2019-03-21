using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class ExpeditionStageComponent : BaseComponent {

    public UISprite Spt_CurrentItem;
    public Transform Trans_BoxLight;


    public ExpeditionData mExpeditionData;
    public ExpeditionInfo.GateStatus mGateStatus;
    private bool isStage;


    public override void MyStart(GameObject root)
    {
        mRootObject = root;

        if (Spt_CurrentItem == null)
        {
            if (mRootObject == null)
                return;
            Spt_CurrentItem = mRootObject.transform.GetComponent<UISprite>();
            Trans_BoxLight = Spt_CurrentItem.transform.FindChild("BoxLight");
        }
        Spt_CurrentItem.gameObject.SetActive(false);
        Trans_BoxLight.gameObject.SetActive(false);
    }

    public override void Clear()
    {
        GameObject.Destroy(mRootObject);
        mExpeditionData = null;
    }

    public void InitStatus(ExpeditionData vData, bool vIsStage)
    {
        if (Spt_CurrentItem == null)
            return;
        mExpeditionData = vData;
        isStage = vIsStage;
        if (isStage)
        {
            CommonFunction.SetSpriteName(Spt_CurrentItem, mExpeditionData.stageIcon);
            Spt_CurrentItem.transform.localPosition = mExpeditionData.stagePos;
        }
        else
        {
            if ((mExpeditionData.awardIcon != null) && (mExpeditionData.awardIcon.Count > 0))
                CommonFunction.SetSpriteName(Spt_CurrentItem, mExpeditionData.awardIcon[0]);
            Spt_CurrentItem.transform.localPosition = mExpeditionData.awardPos;
        }
        Spt_CurrentItem.color = Color.black;
        Spt_CurrentItem.MakePixelPerfect();
        Spt_CurrentItem.GetComponent<BoxCollider>().size = new Vector3(Spt_CurrentItem.width, Spt_CurrentItem.height, 0);
        Spt_CurrentItem.gameObject.SetActive(true);
    }

    public void RefreshStatus(ExpeditionInfo.GateStatus vStatus)
    {
        mGateStatus = vStatus;
        Trans_BoxLight.gameObject.SetActive(false);
        if (!isStage)
        {
            if ((mExpeditionData.awardIcon != null) && (mExpeditionData.awardIcon.Count >= 1))
                CommonFunction.SetSpriteName(Spt_CurrentItem, mExpeditionData.awardIcon[0]);
        }
        Spt_CurrentItem.color = Color.white;
        switch (mGateStatus)
        {
            case ExpeditionInfo.GateStatus.LOCKED://未解锁//
                {
                    Spt_CurrentItem.color = Color.black;
                }
                break;
            case ExpeditionInfo.GateStatus.UNLOCKED://解锁-战斗中//
                { }
                break;
            case ExpeditionInfo.GateStatus.PASSED://通关-未奖励//
                {
                    if (!isStage)
                    {
                        Trans_BoxLight.gameObject.SetActive(true);
                    }
                }
                break;
            case ExpeditionInfo.GateStatus.PASSED_REWARED://通关-已奖励//
                {
                    if (!isStage)
                    {
                        if ((mExpeditionData.awardIcon != null) && (mExpeditionData.awardIcon.Count >= 2))
                            CommonFunction.SetSpriteName(Spt_CurrentItem, mExpeditionData.awardIcon[1]);
                    }
                }
                break;
            default:
                { }
                break;
        }
        //for (int i = 0; i < mExpeditionData.awardIcon.Count; i++)
        //{
        //    Debug.LogWarning(string.Format("[{0}, {1}]", i, mExpeditionData.awardIcon[i]));
        //}
        Spt_CurrentItem.MakePixelPerfect();
        Spt_CurrentItem.GetComponent<BoxCollider>().size = new Vector3(Spt_CurrentItem.width, Spt_CurrentItem.height, 0);
    }
}