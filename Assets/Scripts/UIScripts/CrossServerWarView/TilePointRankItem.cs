using UnityEngine;
using System.Collections;

public class TilePointRankItem : MonoBehaviour {

    private UILabel Lbl_Rank;
    private UILabel Lbl_Name;
    private UILabel Lbl_Score;

    public void Initialize()
    {
        Lbl_Rank = this.transform.FindChild("Rank").gameObject.GetComponent<UILabel>();
        Lbl_Name = this.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_Score = this.transform.FindChild("Score").gameObject.GetComponent<UILabel>();
    }

    public void SetLabel(TerritoryPointRank t)
    {
        Lbl_Rank.text = t.order.ToString();
        Lbl_Name.text = t.name;
        Lbl_Score.text = t.point.ToString();
        this.gameObject.SetActive(true);
    }
}
