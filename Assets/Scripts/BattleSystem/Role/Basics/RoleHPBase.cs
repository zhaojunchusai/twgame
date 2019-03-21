using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 血条基类
/// </summary>
public class RoleHPBase {

    protected RoleAttribute pRoleAttribute;
    protected ERoleType roleType;
    protected GameObject HPobj;
    private ERoleDirection curRoleDirection;
    private List<KeyValuePair<string, EffectPromtType> > MessageList = new List<KeyValuePair<string,EffectPromtType>>();
    private bool IsEndPromt = true;

    public int curHPValue = 0;
    public int maxHpValue = 0;

    /// <summary>
    /// 角色血条
    /// </summary>
    public UISlider UISlider_HP;
    public UILabel value;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vParent">父物件[数据]</param>
    /// <param name="vRoleType">父物件类型</param>
    /// <param name="vPos">位置[城堡类型需要]</param>
    /// <param name="vIsShow">是否显示</param>
    public virtual void Instance(RoleAttribute vParent, ERoleType vRoleType, Vector3 vPos)
    {
        if (vParent == null)
            return;
        if (vParent.transform.localScale.x == 0)
            return;
        pRoleAttribute = vParent;
        roleType = vRoleType;
        curRoleDirection = vParent.Get_Direction;

        float tmpScaleValue = 1 / vParent.transform.localScale.x;

        Vector3 tmpPosition = new Vector3();
        if (!vPos.Equals(Vector3.zero))
            tmpPosition = vPos;
        else
        {
            tmpPosition = SkillTool.GetBonePosition(vParent, 1);
        }

        //获取并设置数据//
        //ResourceLoadManager.Instance.LoadAssetAlone(this.GetPrefabName(), (obj) =>
        //{
        //    if (vParent == null)
        //    {
        //        return;
        //    }
        //    AloneObjectCache.Instance.LoadGameObject(obj, (go) =>
        //    {
        //        GameObject tmpObj = go;
        //        this.HPobj = tmpObj;

        //        if (this.HPobj == null)
        //            return;

        //        tmpObj.transform.parent = vParent.transform;
        //        tmpObj.transform.localPosition = tmpPosition;
        //        tmpObj.transform.localRotation = vParent.transform.localRotation;
        //        tmpObj.transform.localScale = new Vector3(tmpScaleValue, tmpScaleValue, tmpScaleValue);

        //        UISlider_HP = this.HPobj.GetComponent<UISlider>();

        //        if (UISlider_HP == null)
        //            return;

        //        this.SetSlider();
        //        this.SetHpActive();

        //        if (this.maxHpValue <= 0)
        //            UISlider_HP.value = 1.0f;
        //        else
        //            UISlider_HP.value = (float)this.curHPValue / (float)this.maxHpValue;

        //        this.RefreshSlider();
        //    });
        //}, (error) =>
        //{
        //    Debug.LogWarning(string.Format("RoleHPManager: [{0}, {1}]", error, vParent.name));
        //});


        GameObject tmpPrefab = ResourceLoadManager.Instance.LoadView(this.GetPrefabName());
        if (tmpPrefab == null)
            return;
        AloneObjectCache.Instance.LoadGameObject(tmpPrefab, (go) =>
        {
            if (vParent != null)
            {
                GameObject tmpObj = go;
                this.HPobj = tmpObj;

                if (this.HPobj == null)
                    return;

                tmpObj.transform.parent = vParent.transform;
                tmpObj.transform.localPosition = tmpPosition;
                tmpObj.transform.localRotation = vParent.transform.localRotation;
                tmpObj.transform.localScale = new Vector3(tmpScaleValue, tmpScaleValue, tmpScaleValue);

                UISlider_HP = this.HPobj.GetComponent<UISlider>();

                if (UISlider_HP == null)
                    return;

                this.SetSlider();
                this.SetHpActive();

                if (this.maxHpValue <= 0)
                    UISlider_HP.value = 1.0f;
                else
                    UISlider_HP.value = (float)this.curHPValue / (float)this.maxHpValue;

                this.RefreshSlider();
            }
        }, this.GetPrefabName());
    }
    public virtual void RefreshHp(int vCurChangeValue, HurtType vType, bool vIsShowSlider = true)
    {
        this.RefreshHurt(vCurChangeValue, vType);
        this.RefreshPromt(vType);
        if (vIsShowSlider)
        {
            this.RefreshSlider();
        }
    }
    public virtual void RefreshHurt(int vCurChangeValue, HurtType vType)
    {
        HpShow hpShow;
        if (vCurChangeValue != 0)
        {
            if (vType == HurtType.Crite)
                hpShow = new CriteHurt();
            else
            {
                if (vCurChangeValue > 0)
                    hpShow = new AddBlood();
                else
                    hpShow = new NormalHurt();
            }
            hpShow.Instance(this.HPobj.transform, vCurChangeValue.ToString());
            this.SetHpActive();
        }

        this.curHPValue += vCurChangeValue;
        if (this.curHPValue <= 0)
            this.curHPValue = 0;
    }
    public virtual void RefreshPromt(HurtType vType)
    {
        if (this.HPobj == null)
            return;
        HpShow hpShow = new BuffSelf();
        switch (vType)
        {
            case HurtType.Immune: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_IMMUEN); break;
            case HurtType.Resist: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_Resist); break;
            case HurtType.Shield: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_Shield); break;
            case HurtType.UnAccuracyRate: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_UnAccuracyRate); break;
        }
    }
    public virtual void RfreshEffectPromt(string message,EffectPromtType vType)
    {
        if (this.HPobj == null || this.MessageList == null)
            return;

        MessageList.Add(new KeyValuePair<string, EffectPromtType>(message, vType));
        if (this.IsEndPromt)
        {
            this.IsEndPromt = false;
            if (this.MessageList.Count > 0)
            {
                HpShow hpShow = null;
                switch (MessageList[0].Value)
                {
                    case EffectPromtType.Buff: hpShow = new EffectBuff(); hpShow.Instance(this.HPobj.transform, MessageList[0].Key); break;
                    case EffectPromtType.DeBuff: hpShow = new EffectDeBuff(); hpShow.Instance(this.HPobj.transform, MessageList[0].Key); break;
                }
            }
            this.MessageList.RemoveAt(0);
            Scheduler.Instance.AddTimer(0.5f, true, RfreshEffectPromt);
        }
    }
    void RfreshEffectPromt()
    {
        if (this == null || this.HPobj == null || this.MessageList == null)
        {
            Scheduler.Instance.RemoveTimer(RfreshEffectPromt);
            IsEndPromt = true;
            return;
        }

        if (this.MessageList.Count > 0)
        {
            HpShow hpShow = null;
            switch (MessageList[0].Value)
            {
                case EffectPromtType.Buff: hpShow = new EffectBuff(); hpShow.Instance(this.HPobj.transform, MessageList[0].Key); break;
                case EffectPromtType.DeBuff: hpShow = new EffectDeBuff(); hpShow.Instance(this.HPobj.transform, MessageList[0].Key); break;
            }
            this.MessageList.RemoveAt(0);
        }
        if (this.MessageList.Count <= 0)
        {
            Scheduler.Instance.RemoveTimer(RfreshEffectPromt);
            IsEndPromt = true;
        }
    }
    public void SetCurAndMaxValue(int vCur,int vMax)
    {
        this.curHPValue = vCur;
        this.maxHpValue = vMax;
        if (this.UISlider_HP != null)
            this.SetSlider();
        RefreshSlider();
    }
    public virtual void SetMaxHp(int vMaxHp)
    {
        this.maxHpValue = vMaxHp;
        if(this.UISlider_HP)
            this.UISlider_HP.value = (float)this.curHPValue / (float)this.maxHpValue;
        return;
    }
    /// <summary>
    /// 血条渐隐渐现
    /// </summary>
    public virtual void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
        Scheduler.Instance.RemoveTimer(this.SetHpSpriteUnvisible);
        Scheduler.Instance.AddTimer(4.0f, false, this.SetHpSpriteUnvisible);
    }
    public void SetHpSpriteVisible()
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

        if (Background != null && Foreground != null)
        {
            Background.SetActive(false);
            Foreground.SetActive(false);
        }
    }
    public virtual void SetSlider()
    {
        return;
    }
    public virtual void RefreshSlider()
    {
        if(this.UISlider_HP != null)
            this.UISlider_HP.value = (float)this.curHPValue / (float)this.maxHpValue;
        return;
    }
    public virtual string GetPrefabName()
    {
        return HpSliderName.HP_OBJECT_NAME;
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
        UISlider_HP.transform.localEulerAngles += new Vector3(0, 180, 0); ;
    }

}
/// <summary>
/// 传送门血条
/// </summary>
public class TransferHp : RoleHPBase
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_Build.assetbundle";
        return "HP_Build";
    }
    public override void SetSlider()
    {
        Transform HpValue = this.HPobj.transform.FindChild("HPValue");
        if (HpValue == null)
            return;

        this.value = HpValue.GetComponent<UILabel>();
        if (this.value == null)
            return;

        this.value.text = string.Format("{0}/{1}",this.curHPValue,this.maxHpValue);
    }
    public override void RefreshSlider()
    {
        base.RefreshSlider();
        if(this.value != null)
            this.value.text = string.Format("{0}/{1}", this.curHPValue, this.maxHpValue);
        return;
    }
    public override void RefreshHp(int vCurChangeValue, HurtType vType, bool vIsShowSlider = true)
    {
        this.curHPValue += vCurChangeValue;
        if (this.curHPValue < 0)
            this.curHPValue = 0;
        if (vIsShowSlider)
        {
            this.RefreshSlider();
        }
    }
    public override void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
    }

}
/// <summary>
/// 城堡血条
/// </summary>
public class BarracksHp : RoleHPBase
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_Build.assetbundle";
        return "HP_Build";
    }
    public override void RefreshHp(int vCurChangeValue, HurtType vType, bool vIsShowSlider = true)
    {
        this.curHPValue += vCurChangeValue;
        if (this.curHPValue < 0)
            this.curHPValue = 0;
        if (vIsShowSlider)
        {
            this.RefreshSlider();
        }
    }
    public override void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
    }
    public override void SetSlider()
    {
        Transform HpValue = this.HPobj.transform.FindChild("HPValue");
        if (HpValue == null)
            return;

        this.value = HpValue.GetComponent<UILabel>();
        if (this.value == null)
            return;

        this.value.text = "";
    }
}
/// <summary>
/// 我方武将血条
/// </summary>
public class SoldierHp : RoleHPBase
{
    public override void SetSlider()
    {
        base.SetSlider();
        UISprite Spt_Foreground = UISlider_HP.transform.FindChild("Foreground").gameObject.GetComponent<UISprite>();
        UISprite Spt_Background = UISlider_HP.transform.FindChild("Background").gameObject.GetComponent<UISprite>();

        CommonFunction.SetSpriteName(Spt_Background, HpSliderName.HPBACK_COMMON);
        Spt_Background.width = 83;
        Spt_Background.height = 11;
        CommonFunction.SetSpriteName(Spt_Foreground, HpSliderName.HP_SPRITE_GREEN);
        Spt_Foreground.width = 79;
        Spt_Foreground.height = 7;

        UIPanel Hp_Panel = this.HPobj.GetComponent<UIPanel>();
        if (Hp_Panel != null)
             Hp_Panel.sortingOrder = 40;
    }
}
/// <summary>
/// 护送目标血条
/// </summary>
public class EscortHp : SoldierHp
{
    public override void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
    }
}
/// <summary>
/// 敌方武将血条
/// </summary>
public class EnemySoldierHp : RoleHPBase
{
    public override void SetSlider()
    {
        base.SetSlider();
        UISprite Spt_Foreground = UISlider_HP.transform.FindChild("Foreground").gameObject.GetComponent<UISprite>();
        UISprite Spt_Background = UISlider_HP.transform.FindChild("Background").gameObject.GetComponent<UISprite>();

        CommonFunction.SetSpriteName(Spt_Background, HpSliderName.HPBACK_COMMON);
        Spt_Background.width = 83;
        Spt_Background.height = 11;
        CommonFunction.SetSpriteName(Spt_Foreground, HpSliderName.HP_SPRITE_RED);
        Spt_Foreground.width = 79;
        Spt_Foreground.height = 7;

        UIPanel Hp_Panel = this.HPobj.GetComponent<UIPanel>();
        if (Hp_Panel != null)
            Hp_Panel.sortingOrder = 40;
    }
    public override void RefreshPromt(HurtType vType)
    {
        if (this.HPobj == null)
            return;

        HpShow hpShow = new BuffEnemy();
        switch (vType)
        {
            case HurtType.Immune: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_IMMUEN); break;
            case HurtType.Resist: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_Resist); break;
            case HurtType.Shield: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_Shield); break;
            case HurtType.UnAccuracyRate: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_UnAccuracyRate); break;
        }
    }

}
/// <summary>
/// 我方英雄血条
/// </summary>
public class HeroHp : RoleHPBase
{
    public override void SetSlider()
    {
        base.SetSlider();
        UISprite Spt_Foreground = UISlider_HP.transform.FindChild("Foreground").gameObject.GetComponent<UISprite>();
        UISprite Spt_Background = UISlider_HP.transform.FindChild("Background").gameObject.GetComponent<UISprite>();

        CommonFunction.SetSpriteName(Spt_Background, HpSliderName.HPBACK_HERO);
        Spt_Background.width = 96;
        Spt_Background.height = 20;
        CommonFunction.SetSpriteName(Spt_Foreground, HpSliderName.HP_SPRITE_GREEN_HERO);
        Spt_Foreground.width = 84;
        Spt_Foreground.height = 8;

        UIPanel Hp_Panel = this.HPobj.GetComponent<UIPanel>();
        if (Hp_Panel != null)
            Hp_Panel.sortingOrder = 90;

    }
    public override void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
    }

}
/// <summary>
/// 敌方英雄血条
/// </summary>
public class EnemyHeroHp : RoleHPBase
{
    public override void SetSlider()
    {
        base.SetSlider();
        UISprite Spt_Foreground = UISlider_HP.transform.FindChild("Foreground").gameObject.GetComponent<UISprite>();
        UISprite Spt_Background = UISlider_HP.transform.FindChild("Background").gameObject.GetComponent<UISprite>();

        CommonFunction.SetSpriteName(Spt_Background, HpSliderName.HPBACK_HERO);
        Spt_Background.width = 96;
        Spt_Background.height = 20;
        CommonFunction.SetSpriteName(Spt_Foreground, HpSliderName.HP_SPRITE_RED_HERO);
        Spt_Foreground.width = 84;
        Spt_Foreground.height = 8;

        UIPanel Hp_Panel = this.HPobj.GetComponent<UIPanel>();
        if (Hp_Panel != null)
            Hp_Panel.sortingOrder = 90;
    }
    public override void SetHpActive()
    {
        if (this.HPobj == null)
            return;
        this.HPobj.SetActive(true);
        this.SetHpSpriteVisible();
    }
    public override void RefreshPromt(HurtType vType)
    {
        if (this.HPobj == null)
            return;

        HpShow hpShow = new BuffEnemy();
        switch (vType)
        {
            case HurtType.Immune: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_IMMUEN); break;
            case HurtType.Resist: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_Resist); break;
            case HurtType.Shield: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_Shield); break;
            case HurtType.UnAccuracyRate: hpShow.Instance(this.HPobj.transform, ConstString.ATTACTSCENCE_UnAccuracyRate); break;
        }
    }

}

public class HpShow
{
    public virtual void Instance(Transform vParent, string vMessage)
    {
    }
    public  virtual string GetPrefabName()
    {
        return "";
    }
}
public class LabelShow : HpShow
{

    public override void Instance(Transform vParent,string vMessage)
    {
        if (vParent == null)
            return;

        //ResourceLoadManager.Instance.LoadAssetAlone(this.GetPrefabName(), (obj) =>
        //{
        //    if (obj == null)
        //        return;

        //    AloneObjectCache.Instance.LoadGameObject(obj, (GO_HP) =>
        //    {
        //        GameObject tmpObj = GO_HP;
        //        tmpObj.SetActive(true);
        //        tmpObj.transform.parent = vParent;
        //        tmpObj.transform.localPosition = vParent.localPosition;
        //        tmpObj.transform.localRotation = new Quaternion(0,0,0,0);

        //        TweenPosition tweenPos = tmpObj.GetComponent<TweenPosition>();
        //        if (tweenPos != null)
        //        {
        //            tweenPos.ResetToBeginning();
        //            tweenPos.PlayForward();
        //        }
        //        TweenAlpha tweenAlp = tmpObj.GetComponent<TweenAlpha>();
        //        if (tweenAlp)
        //        {
        //            tweenAlp.Restart();
        //            tweenAlp.PlayForward();
        //        }

        //        this.SetLabel(tmpObj,vMessage);

        //        EventDelegate.Add(tmpObj.GetComponent<TweenAlpha>().onFinished, () => { AloneObjectCache.Instance.FreeObject(tmpObj); }, true);
        //    });
        //}, (error) =>
        //{
        //    Debug.LogWarning(string.Format("RoleHPManager: [{0}]", error));
        //});


        GameObject go = ResourceLoadManager.Instance.LoadView(this.GetPrefabName());
        if (go == null)
            return;
        AloneObjectCache.Instance.LoadGameObject(go, (GO_HP) =>
        {
            if (vParent != null)
            {
                GameObject tmpObj = GO_HP;
                tmpObj.SetActive(true);
                tmpObj.transform.parent = vParent;
                tmpObj.transform.localPosition = vParent.localPosition;
                tmpObj.transform.localRotation = new Quaternion(0, 0, 0, 0);

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

                this.SetLabel(tmpObj, vMessage);

                EventDelegate.Add(tmpObj.GetComponent<TweenAlpha>().onFinished, () => { AloneObjectCache.Instance.FreeObject(tmpObj); }, true);
            }
        });
    }

    public virtual void SetLabel(GameObject vLabel,string vMessage)
    {
        if (vLabel == null)
            return;
        vLabel.transform.localScale = Vector3.one;

        GameObject HP = vLabel.transform.FindChild("HP").gameObject;
        if (HP == null)
            return;

        UILabel tmpLabel = HP.GetComponent<UILabel>();
        
        if (tmpLabel == null)
            return;
        
        tmpLabel.text = vMessage;
        tmpLabel.gameObject.SetActive(true);
    }
}
/// <summary>
/// 普通伤害显示
/// </summary>
public class NormalHurt : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_NormalHurt.assetbundle";
        return "HP_NormalHurt";
    }
}
/// <summary>
/// 暴击伤害显示
/// </summary>
public class CriteHurt : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_CriteHurt.assetbundle";
        return "HP_CriteHurt";
    }
    public override void SetLabel(GameObject vLabel, string vMessage)
    {
        if (vLabel == null)
            return;

        vLabel.transform.localScale = Vector3.zero;
        TweenScale com = TweenScale.Begin(vLabel, 0.3F, new Vector3(1.0F, 1.0F, 1.0F));
        com.animationCurve.AddKey(new Keyframe(0.5F, 2.5F));

        GameObject HP = vLabel.transform.FindChild("HP").gameObject;
        if (HP == null)
            return;

        UILabel tmpLabel = HP.GetComponent<UILabel>();

        if (tmpLabel == null)
            return;

        tmpLabel.text = vMessage;
        tmpLabel.gameObject.SetActive(true);
    }
}
/// <summary>
/// 加血显示
/// </summary>
public class AddBlood : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_AddBlood.assetbundle";
        return "HP_AddBlood";
    }
}
/// <summary>
/// 我方BUFF显示
/// </summary>
public class BuffSelf : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_BuffSelf.assetbundle";
        return "HP_BuffSelf";
    }
}
/// <summary>
/// 敌方BUFF显示
/// </summary>
public class BuffEnemy : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_BuffEnemy.assetbundle";
        return "HP_BuffEnemy";
    }
}
/// <summary>
/// BUFF显示
/// </summary>
public class EffectBuff : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_BuffEnemy.assetbundle";
        return "HP_Buff";
    }
}
/// <summary>
/// BUFF显示
/// </summary>
public class EffectDeBuff : LabelShow
{
    public override string GetPrefabName()
    {
        //return "aloneres_HP_BuffEnemy.assetbundle";
        return "HP_DeBuff";
    }
}