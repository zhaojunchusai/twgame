using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
/// <summary>
/// 角色血量管理
/// </summary>
public class RoleHPManager
{
    /// <summary>
    /// 血条物件名字
    /// </summary>
    private const string HP_OBJECT_NAME = "aloneres_RoleProgress.assetbundle";
    /// <summary>
    /// 红血条
    /// </summary>
    private const string HP_SPRITE_RED = "CMN_Slider_hongxuetiao";
    /// <summary>
    /// 绿血条
    /// </summary>
    private const string HP_SPRITE_GREEN = "CMN_Slider_lvxuetiao";
    /// <summary>
    /// 普通血条背景
    /// </summary>
    private const string HPBACK_COMMON = "CMN_Slider_xuetiaodi";
    /// <summary>
    /// 英雄红血条
    /// </summary>
    private const string HP_SPRITE_RED_HERO = "CMN_BG_yxxt02";
    /// <summary>
    /// 英雄绿血条
    /// </summary>
    private const string HP_SPRITE_GREEN_HERO = "CMN_BG_yxxt01";
    /// <summary>
    /// 英雄血条背景
    /// </summary>
    private const string HPBACK_HERO = "CMN_BG_yxxt";
    

    /// <summary>
    /// 角色血条
    /// </summary>
    private UISlider UISlider_HP;
    /// <summary>
    /// 血条图片
    /// </summary>
    private UISprite Spt_Foreground;
    /// <summary>
    /// 血条背景
    /// </summary>
    private UISprite Spt_Background;
    /// <summary>
    /// 角色血量修改显示
    /// </summary>
    private GameObject Lbl_HP;
    /// <summary>
    /// Miss图片
    /// </summary>
    private GameObject Spt_Miss;
    

    /// <summary>
    /// 角色当前血量
    /// </summary>
    private int mCurHP;
    /// <summary>
    /// 角色最大血量
    /// </summary>
    private int mMaxHP;
    /// <summary>
    /// 角色当前方向
    /// </summary>
    private ERoleDirection curRoleDirection;
    /// <summary>
    /// 修改角度
    /// </summary>
    private Vector3 changeEulerAngles = new Vector3(0, 180, 0);
    /// <summary>
    /// 文字信息列表
    /// </summary>
    private List<GameObject> labelList = new List<GameObject>();
    /// <summary>
    /// 图片信息列表
    /// </summary>
    private List<UISprite> spriteList = new List<UISprite>();

    private GameObject HPobj = null;

    private Keyframe CriteKeyframe = new Keyframe(0.5F, 2.5F);
    /// <summary>
    /// 角色类型
    /// </summary>
    private ERoleType roleType;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="vParent">父物件-角色</param>
    /// <param name="vIsShow">是否显示</param>
    public RoleHPManager(RoleAttribute vParent, ERoleType vRoleType, bool vIsShow = true)
    {
        //判断父物件是否存在//
        if (vParent == null)
            return;
        roleType = vRoleType;
        float tmpScaleValue = 1 / vParent.transform.localScale.x;
        //获取血条位置//
        Vector3 tmpPos = SkillTool.GetBonePosition(vParent, 1);
        mCurHP = 0;
        mMaxHP = 0;
        curRoleDirection = vParent.Get_Direction;

        //获取并设置数据//
        ResourceLoadManager.Instance.LoadAssetAlone(HP_OBJECT_NAME, (obj) =>
        {
            if (vParent == null)
            {
                return;
            }
            AloneObjectCache.Instance.LoadGameObject(obj, (go) => 
            {
                GameObject tmpObj = go;
                this.HPobj = tmpObj;
                
                tmpObj.transform.parent = vParent.transform;
                tmpObj.transform.localPosition = tmpPos;
                tmpObj.transform.localScale = new Vector3(tmpScaleValue, tmpScaleValue, tmpScaleValue);

                UISlider_HP = this.HPobj.GetComponent<UISlider>();
                this.Spt_Foreground = UISlider_HP.transform.FindChild("Foreground").gameObject.GetComponent<UISprite>();
                this.Spt_Background = UISlider_HP.transform.FindChild("Background").gameObject.GetComponent<UISprite>();
                if (vParent.Get_RoleType == ERoleType.ertHero)
                {
                    CommonFunction.SetSpriteName(this.Spt_Background, HPBACK_HERO);
                    this.Spt_Background.width = 96;
                    this.Spt_Background.height = 20;
                    if (vParent.Get_RoleCamp == EFightCamp.efcSelf)
                        CommonFunction.SetSpriteName(this.Spt_Foreground, HP_SPRITE_GREEN_HERO);
                    else
                        CommonFunction.SetSpriteName(this.Spt_Foreground, HP_SPRITE_RED_HERO);
                    this.Spt_Foreground.width = 84;
                    this.Spt_Foreground.height = 8;
                }
                else
                {
                    CommonFunction.SetSpriteName(this.Spt_Background, HPBACK_COMMON);
                    this.Spt_Background.width = 83;
                    this.Spt_Background.height = 11;
                    if (vParent.Get_RoleCamp == EFightCamp.efcSelf)
                        CommonFunction.SetSpriteName(this.Spt_Foreground, HP_SPRITE_GREEN);
                    else
                        CommonFunction.SetSpriteName(this.Spt_Foreground, HP_SPRITE_RED);
                    this.Spt_Foreground.width = 79;
                    this.Spt_Foreground.height = 7;
                }

                GameObject GO_HP = ((GameObject)obj).transform.FindChild("HP").gameObject;
                this.Lbl_HP = GO_HP;

                GameObject GO_MISS = ((GameObject)obj).transform.FindChild("Miss").gameObject;
                this.Spt_Miss = GO_MISS;

                UIPanel Hp_Panel = this.HPobj.GetComponent<UIPanel>();
                if(Hp_Panel != null)
                {
                    if (vRoleType == ERoleType.ertHero)
                        Hp_Panel.sortingOrder = 90;
                    else
                        Hp_Panel.sortingOrder = 40;
                }

                UISlider_HP.gameObject.SetActive(vIsShow);

                if ((roleType == ERoleType.ertHero) || (roleType == ERoleType.ertEscort))
                {
                    this.HPobj.SetActive(true);
                    this.SetHpSpriteVisible();
                }
                else
                    this.SetHpActive();
            }, HP_OBJECT_NAME);
        }, (error) => {
            Debug.LogWarning(string.Format("RoleHPManager: [{0}, {1}]", error, vParent.name));
        });
    }

    /// <summary>
    /// 刷新角色血量信息
    /// </summary>
    /// <param name="vCurChangeValue">当前血量修改值</param>
    /// <param name="vMaxValue">最大血量</param>
    public void RefreshRoleHP(int vCurChangeValue, int vMaxValue)
    {
        //设置数值//
        mCurHP += vCurChangeValue;
        if (vMaxValue != 0)
            mMaxHP = vMaxValue;
        //修正数值//
        if (mMaxHP <= 0)
        {
            mMaxHP = 1;
            mCurHP = 0;
        }
        else
        {
            if (mCurHP < 0)
                mCurHP = 0;
            else if (mCurHP > mMaxHP)
                mCurHP = mMaxHP;
        }
        //显示血条//
        RefreshSliderInfo();
    }
    /// <summary>
    /// 刷新角色血量信息
    /// </summary>
    /// <param name="vCurChangeValue">当前血量修改值</param>
    /// <param name="vIsShowChange">是否显示变化值</param>
    public void RefreshRoleHP(int vCurChangeValue, HurtType vType, bool vIsShowChange = true)
    {
        //设置数值//
        mCurHP += vCurChangeValue;

        //修正数值//
        if (mMaxHP <= 0)
            mMaxHP = 1;
        if (mCurHP < 0)
            mCurHP = 0;
        if (mCurHP > mMaxHP)
            mCurHP = mMaxHP;

        //显示血条//
        RefreshSliderInfo();

        //显示修改信息//
        if (vIsShowChange)
        {
            if (vCurChangeValue > 0)
                RefreshLabelInfo(true, vType, vCurChangeValue);
            else
                RefreshLabelInfo(false, vType, vCurChangeValue);
        }
    }

    /// <summary>
    /// 修改角度
    /// </summary>
    public void ChangeEulerAngles(ERoleDirection vRoleDirection)
    {
        if (UISlider_HP == null)
            return;
        if (curRoleDirection == vRoleDirection)
            return;
        curRoleDirection = vRoleDirection;
        UISlider_HP.transform.localEulerAngles += changeEulerAngles;
    }

    /// <summary>
    /// 销毁文字信息
    /// </summary>
    private void DeleteSingleLabel()
    {
        if (labelList.Count > 0)
        {
            AloneObjectCache.Instance.FreeObject(this.labelList[0].gameObject);
            labelList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 销毁图片信息
    /// </summary>
    private void DeleteSingleSprite()
    {
        if (spriteList.Count > 0 && this.spriteList[0] != null)
        {
            AloneObjectCache.Instance.FreeObject(this.spriteList[0].gameObject);
            spriteList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 刷新血条显示
    /// </summary>
    private void RefreshSliderInfo()
    {
        if (UISlider_HP == null)
            return;
        UISlider_HP.value = (float)mCurHP / (float)mMaxHP;
        if ((roleType == ERoleType.ertHero) || (roleType == ERoleType.ertEscort))
            return;
        this.SetHpActive();
    }

    /// <summary>
    /// 刷新修改信息显示
    /// </summary>
    /// <param name="vIsAdd"></param>
    /// <param name="vValue"></param>
    private void RefreshLabelInfo(bool vIsAdd,HurtType vType, int vValue = 0)
    {
        if (vValue != 0)
        {//显示文字信息//
            if (Lbl_HP == null)
                return;
            AloneObjectCache.Instance.LoadGameObject(this.Lbl_HP, (GO_HP) => 
            {
                GameObject tmpObj = GO_HP;
                tmpObj.SetActive(true);
                tmpObj.transform.parent = this.HPobj.transform;

                TweenPosition tweenPos = tmpObj.GetComponent<TweenPosition>();
                if(tweenPos != null)
                {
                    tweenPos.ResetToBeginning();
                    tweenPos.PlayForward();
                }
                TweenAlpha tweenAlp = tmpObj.GetComponent<TweenAlpha>();
                if(tweenAlp)
                {
                    tweenAlp.Restart();
                    tweenAlp.PlayForward();
                }
                if (vType == HurtType.Crite)
                {
                    tmpObj.transform.localScale = Vector3.zero;
                    TweenScale com = TweenScale.Begin(tmpObj, 0.3F, new Vector3(3.0F, 3.0F, 1.0F));
                    com.animationCurve.AddKey(CriteKeyframe);
                }
                else
                    tmpObj.transform.localScale = Vector3.one;

                tmpObj.transform.localPosition = Lbl_HP.transform.localPosition;
                UILabel tmpLabel = tmpObj.transform.FindChild("HP").gameObject.GetComponent<UILabel>();

                if (tmpLabel == null)
                    return;

                if (vIsAdd)
                    tmpLabel.color = Color.green;
                else
                {
                    if (vType == HurtType.Crite)
                    {
                        tmpLabel.color = Color.yellow;
                    }
                    else
                        tmpLabel.color = Color.red;
                }
                tmpLabel.text = vValue.ToString();
                tmpLabel.gameObject.SetActive(true);

                EventDelegate.Add(tmpObj.GetComponent<TweenAlpha>().onFinished, DeleteSingleLabel, true);
                labelList.Add(tmpObj);
            });
        }
        else
        {//显示图片信息//
            if (vType == HurtType.Normal || vType == HurtType.none)
                return;

            if (Spt_Miss == null)
                return;
            AloneObjectCache.Instance.LoadGameObject(this.Spt_Miss, (GO_MISS) => 
            {
                GameObject tmpObj = GO_MISS;
                tmpObj.SetActive(true);
                tmpObj.transform.parent = this.HPobj.transform;
                tmpObj.transform.localScale = Vector3.one;
                tmpObj.transform.localPosition = Spt_Miss.transform.localPosition;

                TweenPosition tweenPos = tmpObj.GetComponent<TweenPosition>();
                if (tweenPos != null)
                {
                    tweenPos.ResetToBeginning();
                    tweenPos.PlayForward();
                }
                TweenAlpha tweenAlp = tmpObj.GetComponent<TweenAlpha>();
                if (tweenAlp)
                {
                    tweenAlp.Restart();
                    tweenAlp.PlayForward();
                }

                UISprite tmpSprite = tmpObj.GetComponent<UISprite>();
                if (tmpSprite == null)
                    return;
                if (vType == HurtType.UnAccuracyRate)
                    CommonFunction.SetSpriteName(tmpSprite, GlobalConst.SpriteName.FightShanbi);
                else if (vType == HurtType.Shield)
                    CommonFunction.SetSpriteName(tmpSprite, GlobalConst.SpriteName.FightShield);
                else if (vType == HurtType.Immune)
                    CommonFunction.SetSpriteName(tmpSprite, GlobalConst.SpriteName.FightImmune);
                else if (vType == HurtType.Resist)
                    CommonFunction.SetSpriteName(tmpSprite, GlobalConst.SpriteName.FightResist);
                tmpSprite.MakePixelPerfect();
                tmpSprite.gameObject.SetActive(true);

                EventDelegate.Add(tmpSprite.GetComponent<TweenAlpha>().onFinished, DeleteSingleSprite, true);
                spriteList.Add(tmpSprite);
            });
        }
    }


    /// <summary>
    /// 血条渐隐渐现
    /// </summary>
    private void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
        Scheduler.Instance.RemoveTimer(this.SetHpSpriteUnvisible);
        Scheduler.Instance.AddTimer(4.0f, false, this.SetHpSpriteUnvisible);
    }
    private void SetHpSpriteVisible()
    {
        if (this.HPobj == null)
            return;

        GameObject Background = this.HPobj.transform.FindChild("Background").gameObject;
        GameObject Foreground = this.HPobj.transform.FindChild("Foreground").gameObject;

        if (Background != null && Foreground != null)
        {
            Background.SetActive(true);
            Foreground.SetActive(true);
        }
    }
    public void SetHpSpriteUnvisible()
    {
        if (this.HPobj == null)
            return;

        GameObject Background = this.HPobj.transform.FindChild("Background").gameObject;
        GameObject Foreground = this.HPobj.transform.FindChild("Foreground").gameObject;

        if(Background != null && Foreground != null)
        {
            Background.SetActive(false);
            Foreground.SetActive(false);
        }
    }
}
