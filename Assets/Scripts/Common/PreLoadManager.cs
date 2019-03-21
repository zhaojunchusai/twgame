using System.Collections;
using System.Collections.Generic;

public class AssetResourceData
{
    public string assetName;
    public string resType;
}

public class PreLoadManager : MonoSingleton<PreLoadManager>
{
    public List<AssetResourceData> GetPreAssetList()
    {
        List<AssetResourceData> preAssetList = new List<AssetResourceData>();
        preAssetList.Add(new AssetResourceData() { assetName = "font_FZHTJW.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_White.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_Green.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_Vip.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_AddBlood.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_FT_HeroFont.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_CritFont.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_LvUnLock.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_Orange.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_SuitractBlood.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_LevelUP.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "nguifont_DHJB.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_HighDivisionAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_FirstPay_Square.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_BadgeIconAtlas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_10.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_15.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_7.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_13.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_18.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_9.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_14.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_11.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_19.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_17.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_2.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_12.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_4.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_6.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_8.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_16.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_3.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_5.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAtlas_New_1.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_EquipIconAtlas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_GameActivityAltas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_HeadPortraitAtlas_2.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_HeadPortraitAtlas_1.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_HeadPortraitMaxAtlas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_LifeSpiritAtlas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_ItemsIconAtlas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_SkillIconAtlas.assetbundle", resType = ResourceType.UIWindow });
        //preAssetList.Add(new AssetResourceData() { assetName = "atlas_TaskIconAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_BoxLight_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LifeSpiritLockBtnAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_ring_gongchengliedi.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_xuanzhuan_Atlas_1.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_WZP_Juntuanzhengba_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_AutoFighting.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_SignEffect_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_Battleground_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LevelUpEffect_Square.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LL_JuanZeng1.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_Recruitment_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LL_HuoPen.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_tiaoxi.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_FirstPay_Circle.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_WZP_Yan.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_HintViewAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_Refresh_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LL_JuanZeng2.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LowDivisionAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LifeSpiritCollectAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_PreyLifeSpiritAtlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_LPZ_pics_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_xianglu.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_WZP_Huo.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_Booom_Atlas.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas0.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas1.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas2.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas3.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas4.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas5.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas6.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas7.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas8.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas9.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas10.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas11.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas12.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas13.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas14.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas15.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas16.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas17.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas18.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas19.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas20.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas21.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas22.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas23.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas24.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas25.assetbundle", resType = ResourceType.UIWindow });
        preAssetList.Add(new AssetResourceData() { assetName = "atlas_CommonAltlas26.assetbundle", resType = ResourceType.UIWindow });
        return preAssetList;
    }
}
