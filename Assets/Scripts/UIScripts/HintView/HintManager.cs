using System;
using UnityEngine;
using System.Collections.Generic;
public class HintManager :Singleton<HintManager>
{

    public void Initialize() 
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HINT);
        PlayerData.Instance.LevelUpEvent += ListenLevelUp;
    }

    public void Uninitialize()
    {
        PlayerData.Instance.LevelUpEvent -= ListenLevelUp;
    }

    public void ListenLevelUp(int oldLv,int newLv)
    {
        UISystem.Instance.ShowGameUI(LevelUPView.UIName);
        UISystem.Instance.LevelUPView.ShowLevelUp(oldLv,newLv);
    }

    public void SeeDetail(GameObject go, bool press ,ItemInfo info)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);

        UISystem.Instance.SeeDetailView.SeeDetail(go, press, info);
    }
    public void SeeDetail(GameObject go, bool press, EquipAttributeInfo info)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);

        UISystem.Instance.SeeDetailView.SeeDetail(go, press, info);
    }
    public void SeeDetail(GameObject go, bool press, CommonItemData info)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);

        UISystem.Instance.SeeDetailView.SeeDetail(go, press, info);
    }

    public void SeeDetail(GameObject go, bool press, MonsterAttributeInfo info)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);
        UISystem.Instance.SeeDetailView.SeeDetail(go, press, info);
    }

    public void SeeDetail(GameObject go, bool press, CombatPetInfo info)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);
        UISystem.Instance.SeeDetailView.SeeDetail(go, press, info);
    }

    public void SeeDetail(GameObject go, bool press, uint id)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);
        UISystem.Instance.SeeDetailView.SeeDetail(go, press, id);
    }
    public void SeeDetail(GameObject go, bool press, string content)
    {
        if (!UISystem.Instance.UIIsOpen(SeeDetailView.UIName))
            UISystem.Instance.ShowGameUI(SeeDetailView.UIName);

        UISystem.Instance.SeeDetailView.SeeDetail(go, press, content);
    }

}
