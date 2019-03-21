using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 聊天泡
/// </summary>
public class RoleChatBubble
{
    private const string OBJECT_CHATBUBBLE_NAME = "RoleChatBubble";
    private const string SPRITE_CHATBUBBLE_NAME = "Cmn_bg_qipao";
    private const float MIN_WIDTH = 44;//最小宽度//
    private const float MIN_HEIGHT = 34;//最小高度//
    private const float OFFSET_POS_Y = 16;//高度调整值//


    public GameObject Obj_RoleChatBubble;
    public UISprite Spt_ChatBackGround;
    public UILabel Lbl_ChatText;


    private EChatTextType textType;//聊天文本类型//
    private int showTime;//文本显示时间//
    private bool isLive;//是否存在//
    private Scheduler.OnScheduler callBackFunction;
    private RoleAttribute parentObj;
    private ERoleDirection roleDirection;
    private int initRoleOrder;


    public bool Get_IsLive
    {
        get {
            return isLive;
        }
    }


    /// <summary>
    /// 创建聊天泡
    /// </summary>
    /// <param name="vParent">聊天泡父级组件</param>
    public RoleChatBubble(RoleAttribute vParent, string vText, EChatTextType vTextType, int vShowTime, Scheduler.OnScheduler vCallFunction = null)
    {
        parentObj = vParent;
        if (parentObj == null)
            return;
        parentObj.chatChangeEulerAngles = ChangeEulerAngles;
        roleDirection = parentObj.Get_Direction;
        isLive = true;
        float tmpScaleValue = 1 / parentObj.transform.localScale.x;

        GameObject tmpPrefab = ResourceLoadManager.Instance.LoadView(OBJECT_CHATBUBBLE_NAME);
        if (tmpPrefab == null)
            return;
        GameObject tmpObj = GameObject.Instantiate(tmpPrefab) as GameObject;
        Obj_RoleChatBubble = tmpObj;
        Spt_ChatBackGround = Obj_RoleChatBubble.transform.FindChild("ChatBackGround").GetComponent<UISprite>();
        Lbl_ChatText = Obj_RoleChatBubble.transform.FindChild("ChatBackGround/ChatText").GetComponent<UILabel>();
        Obj_RoleChatBubble.transform.parent = parentObj.transform;
        Obj_RoleChatBubble.transform.localPosition = GetChatBubblePosByRoleType(parentObj, parentObj.Get_RoleType);
        Obj_RoleChatBubble.transform.localScale = new Vector3(tmpScaleValue, tmpScaleValue, tmpScaleValue);
        Obj_RoleChatBubble.GetComponent<UIPanel>().sortingOrder = 91;
        CommonFunction.SetSpriteName(Spt_ChatBackGround, SPRITE_CHATBUBBLE_NAME);
        CommonFunction.SetObjLayer(Obj_RoleChatBubble.transform, parentObj.gameObject.layer);
        Obj_RoleChatBubble.SetActive(false);
        ShowChatText(vText, vTextType, vShowTime, vCallFunction);
    }

    /// <summary>
    /// 清空操作
    /// </summary>
    public void ClearOperate()
    {
        if (parentObj != null)
        {
            CommonFunction.SetObjLayer(parentObj.transform, (int)LayerMaskEnum.UI);
            if (parentObj.Get_MainSpine != null)
            {
                parentObj.Get_MainSpine.setSortingOrder(initRoleOrder);
            }
        }
        if (textType == EChatTextType.ecttCommon)
            GameObject.Destroy(Obj_RoleChatBubble);
        isLive = false;
    }
        
    /// <summary>
    /// 显示聊天泡
    /// </summary>
    /// <param name="vText">聊天信息</param>
    /// <param name="vTextType">文本框类型</param>
    /// <param name="vShowTime">显示时间</param>
    public void ShowChatText(string vText, EChatTextType vTextType, int vShowTime, Scheduler.OnScheduler vCallFunction = null)
    {
        callBackFunction = null;
        if (Obj_RoleChatBubble != null)
            Obj_RoleChatBubble.SetActive(false);
        if (string.IsNullOrEmpty(vText) || (vText.Equals("0")))
        {
            if (vCallFunction != null)
                Scheduler.Instance.AddTimer(CommonFunction.GetSecondTimeByMilliSecond(vShowTime), false, vCallFunction);
            return;
        }
        if (Lbl_ChatText == null)
            return;
        if (parentObj != null)
        {
            CommonFunction.SetObjLayer(parentObj.transform, (int)LayerMaskEnum.NGUI);
            if (parentObj.Get_MainSpine != null)
            {
                //if (parentObj.Get_MainSpine.GetMinSort() < GlobalConst.TEMPORARY_SORT_ORDER)
                //    initRoleOrder = parentObj.Get_MainSpine.GetMinSort();
                //parentObj.Get_MainSpine.setSortingOrder(GlobalConst.TEMPORARY_SORT_ORDER);
                if (parentObj.Get_MainSpine.GetMinSort() < GlobalConst.TEMPORARY_SORT_ORDER)
                    initRoleOrder = parentObj.Get_MainSpine.GetMinSort();
                parentObj.Get_MainSpine.setSortingOrder(TalkManager.Instance.Get_CurChatSortOrder);
                TalkManager.Instance.Get_CurChatSortOrder = TalkManager.Instance.Get_CurChatSortOrder + 5;
            }
        }
        callBackFunction = vCallFunction;
        Scheduler.Instance.RemoveTimer(ChangeChatActiveStatus);
        Lbl_ChatText.text = vText;
        textType = vTextType;
        showTime = vShowTime;
        Scheduler.Instance.AddTimer(0.5f, false, DelayShowChatBubble);
    }

    /// <summary>
    /// 隐藏聊天泡
    /// </summary>
    public void HideChatText()
    {
        if (Obj_RoleChatBubble != null)
            Obj_RoleChatBubble.SetActive(false);
    }

    /// <summary>
    /// 修改聊天泡角度
    /// </summary>
    public void ChangeEulerAngles()
    {
        if (parentObj == null)
            return;
        if (roleDirection == parentObj.Get_Direction)
            return;
        roleDirection = parentObj.Get_Direction;
        if (Obj_RoleChatBubble == null)
            return;
        Obj_RoleChatBubble.transform.localEulerAngles += new Vector3(0, 180, 0); ;
    }

    /// <summary>
    /// 延迟显示聊天泡
    /// </summary>
    private void DelayShowChatBubble()
    {
        if (Lbl_ChatText == null)
            return;
        Vector2 tmpRect = Lbl_ChatText.printedSize;
        if (Spt_ChatBackGround == null)
            return;
        Spt_ChatBackGround.width = (int)(tmpRect.x + MIN_WIDTH);
        Spt_ChatBackGround.height = (int)(tmpRect.y + MIN_HEIGHT);
        Lbl_ChatText.transform.localPosition = new Vector3(-Lbl_ChatText.printedSize.x / 2, Lbl_ChatText.printedSize.y + OFFSET_POS_Y, 0);
        if (Obj_RoleChatBubble != null)
            Obj_RoleChatBubble.SetActive(true);

        if (textType == EChatTextType.ecttCommon)
        {
            Scheduler.Instance.AddTimer(CommonFunction.GetSecondTimeByMilliSecond(showTime), false, ChangeChatActiveStatus);
        }
        else if (textType == EChatTextType.ecttInterval)
        {
            Scheduler.Instance.AddTimer(CommonFunction.GetSecondTimeByMilliSecond(showTime), true, ChangeChatActiveStatus);
        }

        if (callBackFunction != null)
            Scheduler.Instance.AddTimer(CommonFunction.GetSecondTimeByMilliSecond(showTime), false, callBackFunction);
    }

    /// <summary>
    /// 修改聊天泡显示状态
    /// </summary>
    private void ChangeChatActiveStatus()
    {
        if (Obj_RoleChatBubble != null)
            Obj_RoleChatBubble.SetActive(!Obj_RoleChatBubble.activeSelf);
    }

    /// <summary>
    /// 通过角色类型获取聊天泡位置
    /// </summary>
    /// <param name="vParent"></param>
    /// <param name="vRoleType"></param>
    /// <returns></returns>
    private Vector3 GetChatBubblePosByRoleType(RoleAttribute vParent, ERoleType vRoleType)
    {
        Vector3 tmpResult = Vector3.zero;
        if (vParent == null)
            return tmpResult;

        if ((vRoleType == ERoleType.ertHero) || (vRoleType == ERoleType.ertSoldier) || 
            (vRoleType == ERoleType.ertShooter) || (vRoleType == ERoleType.ertMonster) || (vRoleType == ERoleType.ertEscort))
        {
            tmpResult = SkillTool.GetBonePosition(vParent, 1) + new Vector3(0, 40, 0);
        }
        else if (vRoleType == ERoleType.ertTransfer)
        {
            tmpResult = new Vector3(0, 120, 0);
        }
        else if (vRoleType == ERoleType.ertBarracks)
        {
            if (vParent.Get_RoleCamp == EFightCamp.efcSelf)
                tmpResult = new Vector3(-100, 220, 0);
            else
                tmpResult = new Vector3(100, 220, 0);
        }

        return tmpResult;
    }

}