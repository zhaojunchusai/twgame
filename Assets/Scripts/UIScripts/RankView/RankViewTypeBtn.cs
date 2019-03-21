using UnityEngine;
using System.Collections;
using System;

public class RankViewTypeBtn : RankViewTypeBase
{
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UILabel Lbl_BtnLbl;
    private bool _initialized = false;
    private Color _lblUnSelectColor = new Color(35 / 255f, 19 / 255f, 0f);
    private Color _lblSelectColor = new Color(1f, 1f, 197 / 255f);
    public RankViewSubTypeObj SubObj;
    private void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        Spt_BG = transform.FindChild("ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnLbl = transform.FindChild("Label").gameObject.GetComponent<UILabel>();
        UIEventListener.Get(gameObject).onClick = ClickTypeBtn;
    }

    public void Init(RankviewData data, Action<uint> clickType)
    {
        Initialize();
        _data = data;
        _clickType = clickType;
        gameObject.name = _data.ID.ToString();
        Lbl_BtnLbl.text = data.Name;
    }

    public void SetBtnState(bool selected)
    {
        CommonFunction.SetSpriteName(Spt_BG, selected ? GlobalConst.SpriteName.RANK_BTN_SELECT : GlobalConst.SpriteName.RANK_BTN_NOTSELECT);
        Lbl_BtnLbl.color = selected ? _lblSelectColor : _lblUnSelectColor;
    }

    protected override void ClickTypeBtn(GameObject go)
    {
        if (_clickType != null)
        {
            _clickType(uint.Parse(go.name));
        }
    }
}
