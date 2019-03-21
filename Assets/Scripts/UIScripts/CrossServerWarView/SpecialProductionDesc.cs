using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialProductionDesc : MonoBehaviour {

    private UILabel Lbl_TileID;
    private UILabel Lbl_Productions;
    private UILabel Lbl_OwnerState;

    public void Initialize()
    {
        Lbl_TileID = transform.FindChild("TileID").gameObject.GetComponent<UILabel>();
        Lbl_Productions = transform.FindChild("Productions").gameObject.GetComponent<UILabel>();
        Lbl_OwnerState = transform.FindChild("OwnerState").gameObject.GetComponent<UILabel>();
        Lbl_Productions.text = "";
    }

    public void SetSpecialProductionDesc(uint id,string desc,string name)
    {
        //List<CommonItemData> datas = CommonFunction.GetCommonItemDataList(production);
        //if (datas == null || datas.Count < 1)
        //{
        //    Debug.LogError("special production id is wrong id = " + production);
        //    return;
        //}
        Lbl_TileID.text = name;
        Lbl_Productions.text = desc;
        //Lbl_Productions.text = "";
        //foreach(CommonItemData item in datas)
        //{
        //    if (Lbl_Productions.text == "")
        //        Lbl_Productions.text = item.Name + "x" + item.Num;
        //    else
        //        Lbl_Productions.text += "\n" + item.Name + "x" + item.Num;
        //}
        if (UISystem.Instance.CrossServerWarView.tileDic[id].owner_type == 1)
            Lbl_OwnerState.text = ConstString.CROSSSERVER_OCCUPPIED;
        else
            Lbl_OwnerState.text = ConstString.CROSSSERVER_UNOCCUPPIED;
    }
}
