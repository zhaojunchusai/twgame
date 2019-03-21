using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 单行奖励
/// </summary>
public class SingleRankItem {

    private const int INIT_POSY = 30;
    private const int DIS_POSY = 90;
    private const int LIMIT_LINECOUNT = 6;


    public GameObject Obj_Root;
    public UILabel Lbl_Rank;
    public Transform Trans_Award;
    private List<SingleAwardItem> listAwardItems;


    public SingleRankItem(Transform vSourceTrans)
    {
        if (vSourceTrans != null)
        {
            Obj_Root = CommonFunction.InstantiateObject(vSourceTrans.gameObject, vSourceTrans.parent);
            if (Obj_Root != null)
            {
                Lbl_Rank = Obj_Root.transform.FindChild("Rank").gameObject.GetComponent<UILabel>();
                Trans_Award = Obj_Root.transform.FindChild("AwardItem");
                InitInfo();
            }
        }
        if (listAwardItems == null)
        {
            listAwardItems = new List<SingleAwardItem>();
        }
        listAwardItems.Clear();
    }

    public void InitInfo()
    {
        if (Obj_Root != null)
        {
            if (Lbl_Rank != null)
            {
                Lbl_Rank.text = string.Empty;
            }
            if (Trans_Award != null)
            {
                Trans_Award.gameObject.SetActive(false);
            }
            if (listAwardItems != null)
            {
                foreach (SingleAwardItem tmpInfo in listAwardItems)
                {
                    if (tmpInfo != null)
                    {
                        tmpInfo.InitInfo();
                    }
                }
            }
            Obj_Root.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vIndex">索引</param>
    /// <param name="vID">掉落包ID</param>
    public int ReSetValue(int vIndex, uint vID, int vInitPosY)
    {
        int tmpResult = INIT_POSY;
        if (Obj_Root != null)
        {
            if (Lbl_Rank != null)
            {
                if (vIndex == -1)
                {
                    Lbl_Rank.text = ConstString.SUPERMACY_OTHERRANK;
                }
                else
                {
                    Lbl_Rank.text = string.Format(ConstString.SUPERMACY_RANK, vIndex + 1);
                }
            }

            //計算整個獎勵所佔用的像素高度//
            List<CommonItemData> tmpDropList = CommonFunction.GetCommonItemDataList(vID);
            if (tmpDropList != null)
            {
                int tmpCount = 0;
                foreach (CommonItemData tmpInfo in tmpDropList)
                {
                    if (tmpInfo != null)
                    {
                        if (tmpCount < listAwardItems.Count)
                        {
                            if (listAwardItems[tmpCount] == null)
                            {
                                listAwardItems[tmpCount] = new SingleAwardItem(Trans_Award);
                            }
                        }
                        else
                        {
                            listAwardItems.Add(new SingleAwardItem(Trans_Award));
                        }
                        listAwardItems[tmpCount].ReSetValue(tmpCount, tmpInfo);
                        tmpCount += 1;
                    }
                }

                tmpResult += (tmpCount / LIMIT_LINECOUNT) * DIS_POSY;
                if (tmpCount % LIMIT_LINECOUNT != 0)
                {
                    tmpResult += DIS_POSY;
                }
            }

            Obj_Root.transform.localPosition = new Vector3(0, vInitPosY, 0);
            Obj_Root.SetActive(true);
        }
        return tmpResult;
    }
}