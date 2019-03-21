using UnityEngine;
using System.Collections.Generic;
/* File: GateEnemyInfoComponent.cs
 * Desc: 关卡信息界面敌方信息
 * Date: 2015-07-08 15:03
 * add by taiwei
 */
public class GateEnemyInfoComponent : ItemBaseComponent
{
    private MonsterAttributeInfo monsterInfo;
    public MonsterAttributeInfo MonsterInfo 
    {
        get 
        {
            return monsterInfo;
        }
    }

    public UILabel Lbl_NameLabel;
    //public Transform Gobj_BossGroup;
    //public Transform Gobj_BgGroup;
    public UISprite Spt_NormalBg;
    public UISprite Spt_BG;
    public delegate void PressDelegate(GateEnemyInfoComponent comp, bool isPress);
    public PressDelegate pressDelegate;
    public GateEnemyInfoComponent(GameObject root) 
    {
        base.MyStart(root);
        //Gobj_BgGroup = mRootObject.transform.FindChild("BgGroup").gameObject.transform;
        Spt_QualitySprite = mRootObject.transform.FindChild("BaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("BaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Lbl_NameLabel = mRootObject.transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        //Gobj_BossGroup = mRootObject.transform.FindChild("BossGroup").gameObject.transform;
        Spt_ItemBgSprite = mRootObject.transform.FindChild("BaseComp/Bg").gameObject.GetComponent<UISprite>();
        Spt_NormalBg = mRootObject.transform.FindChild("NormalBg").gameObject.GetComponent<UISprite>();
        Spt_BG = mRootObject.transform.FindChild("BgGroup/Bg").gameObject.GetComponent<UISprite>();
        //base.AutoSetGoProperty<GateEnemyInfoComponent>(this, root);
    }

    public void AddPressLisetener(PressDelegate callBack)
    {
        pressDelegate = callBack;
        UIEventListener.Get(mRootObject).onPress = PressHandle;
    }

    private void PressHandle(GameObject go, bool IsPress)
    {
        if (pressDelegate != null)
        {
            pressDelegate(this, IsPress);
        }
    }

    public void UpdateInfo(MonsterAttributeInfo info,bool isBoss) 
    {
        monsterInfo = info;
        if (monsterInfo == null) return;
        UpdateCompInfo(monsterInfo.HeadID, monsterInfo.Star, isBoss,monsterInfo.Name);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="icon">头像</param>
    /// <param name="quality">品质</param>
    /// <param name="isBoss">是否为boss</param>
    private void UpdateCompInfo(string icon,int quality,bool isBoss,string name) 
    {
        base.UpdateInfo(icon, quality);
        if (isBoss)
        {
            Lbl_NameLabel.text = string.Format(ConstString.GATE_ENEMY_BOSS, name);
            Lbl_NameLabel.effectStyle = UILabel.Effect.Shadow;
            CommonFunction.SetSpriteName(Spt_BG, GlobalConst.SpriteName.GTAE_INFO_BOSSBG);
            Spt_NormalBg.enabled = false;
            Lbl_NameLabel.effectDistance = Vector2.one * 2;
            //Lbl_NameLabel.spacingY = 2;
        }
        else
        {
            Lbl_NameLabel.text = string.Format(ConstString.GATE_ENEMY_NOMRAL, name);
            Lbl_NameLabel.effectStyle = UILabel.Effect.None;
            //Lbl_NameLabel.spacingX = 2;
            //Lbl_NameLabel.spacingY = 2;
            Spt_NormalBg.enabled = true;
            CommonFunction.SetSpriteName(Spt_BG, GlobalConst.SpriteName.GTAE_INFO_NORMALBG);
        }
        //Gobj_BgGroup.gameObject.SetActive(!isBoss);
        //Gobj_BossGroup.gameObject.SetActive(isBoss);
    }

    public override void Clear()
    {
        base.Clear();
        Lbl_NameLabel.text = string.Empty;
        //Gobj_BossGroup.gameObject.SetActive(false);
        //Gobj_BgGroup.gameObject.SetActive(true);
    }
}


