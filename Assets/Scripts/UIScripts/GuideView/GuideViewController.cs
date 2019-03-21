using UnityEngine;
using System;
using System.Collections.Generic;

public class GuideViewController : UIBase 
{
    public GuideView _ui;
    private BoxCollider[] _colliders;
    private GuideMaskType _curMaskType = GuideMaskType.None;
    private Texture2D _maskCircle;
    private Texture2D _maskRect;
    public override void Initialize()
    {
        if (_ui == null)
            _ui = new GuideView();
        _ui.Initialize();

        InitParam();
        BtnEventBinding();
        CloseAllGuideItem();
    }
    
    public void CloseAllGuideItem()
    {
        if (_ui == null) return;
        if (_ui.Spt_HighLight01 == null) return;
        _ui.Spt_HighLight01.gameObject.SetActive(false);
        _ui.Spt_HighLight02.gameObject.SetActive(false);
        _ui.Tex_GuideMask.gameObject.SetActive(false);
        _ui.Gobj_Colliders.gameObject.SetActive(false);
        _ui.Trans_Finger.gameObject.SetActive(false);
        _ui.Trans_Arrow01.gameObject.SetActive(false);
        _ui.Trans_Arrow02.gameObject.SetActive(false);
        _ui.Gobj_Desc.SetActive(false);
        _ui.Gobj_DragArrow.SetActive(false);
    }

    public bool AllGuideItemIsClose()
    {
        return !_ui.Spt_HighLight01.gameObject.activeSelf ||
               !_ui.Spt_HighLight02.gameObject.activeSelf ||
               !_ui.Tex_GuideMask.gameObject.activeSelf ||
               !_ui.Gobj_Colliders.gameObject.activeSelf ||
               !_ui.Trans_Finger.gameObject.activeSelf ||
               !_ui.WhiteCanvas.gameObject.activeSelf ||
               !_ui.Trans_Arrow01.gameObject.activeSelf ||
               !_ui.Trans_Arrow02.gameObject.activeSelf ||
               !_ui.Gobj_Desc.gameObject.activeSelf ||
               !_ui.Gobj_DragArrow.gameObject.activeSelf;
    }

    public void SetWhiteScreen(bool forward)
    {
        _ui.WhiteCanvas.gameObject.SetActive(true);
        _ui.WhiteCanvas.OnFinishEvent = OnWhiteScreedFinish;
        _ui.WhiteCanvas.PlayAni(forward);
    }

    public void OnWhiteScreedFinish()
    {

    }

    public void SetDragArrow(bool right)
    {
        _ui.Gobj_DragArrow.SetActive(true);
        if (right)
        {
             _ui.Gobj_DragArrow.transform.localRotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            _ui.Gobj_DragArrow.transform.localRotation = Quaternion.Euler(0, 180, 0);            
        }
    }

    public void SetDesc(Vector2 pos,string content)
    {
        _ui.Gobj_Desc.SetActive(true);
        _ui.Gobj_Desc.transform.localPosition = pos;
        _ui.Lbl_Desc.text = content;
        _ui.Lbl_Desc.transform.localPosition = new Vector3(-(float)_ui.Lbl_Desc.width/2,0,0);
        _ui.Spt_DescBG.width = _ui.Lbl_Desc.width + 82;
        _ui.Spt_DescBG.height = _ui.Lbl_Desc.height + 50;
    }

    public void SetHighLight(List<Vector4> posSize)
    {
        if (posSize == null)
            return;
        for (int i = 0; i < posSize.Count; i++)
        {
            if(i > 1)
            {
                break;
            }
            SetHighLight(i + 1, posSize[i]);
        }
    }
    private void SetHighLight(int index, Vector4 posSize) { SetHighLight(index, new Vector3(posSize.x, posSize.y, 0), new Vector2(posSize.z, posSize.w)); }
    private void SetHighLight(int index, Vector3 position, Vector2 size)
    {
        if(index == 1)
        {
            _ui.Spt_HighLight01.gameObject.SetActive(true);
            _ui.Spt_HighLight01.transform.localPosition = position;
            _ui.Spt_HighLight01.width = (int)size.x /*+ 30*/;
            _ui.Spt_HighLight01.height = (int)size.y /*+ 30*/;
        }
        else
        {
            _ui.Spt_HighLight02.gameObject.SetActive(true);
            _ui.Spt_HighLight02.transform.localPosition = position;
            _ui.Spt_HighLight02.width = (int)size.x /*+ 30*/;
            _ui.Spt_HighLight02.height = (int)size.y /*+ 30*/;
        }
    }

    public void SetCollider(Vector4 posSize){SetCollider(new Vector3(posSize.x, posSize.y, 0), new Vector2(posSize.z, posSize.w));}
    private void SetCollider(Vector3 position, Vector2 size)
    {
        //Debug.LogError(size.x + "   " + size.y);
        if (_ui.Gobj_Colliders == null)
        {
            Debug.LogError("_ui.Gobj_Colliders == null");
            return;
        }
        _ui.Gobj_Colliders.gameObject.SetActive(true);
        _ui.Gobj_Colliders.transform.localPosition = position + Vector3.back;
        if (_colliders == null || _colliders[0] == null) 
        {
            _colliders = null;
            InitParam();
        }
        _colliders[0].center = new Vector3(0, 288 + size.y / 2-15, 0);
        _colliders[1].center = new Vector3(0, -288 - size.y / 2+15, 0);
        _colliders[2].center = new Vector3(512 + size.x / 2-15, 0, 0);
        _colliders[3].center = new Vector3(-512 - size.x / 2+15, 0, 0);
    }

    public void SetMaskPan(Vector4 posSize,GuideMaskType type) { SetMaskPan(new Vector3(posSize.x, posSize.y, 0), new Vector2(posSize.z, posSize.w),type); }
    private void SetMaskPan(Vector3 position, Vector2 size, GuideMaskType type)
    {
        _ui.Tex_GuideMask.gameObject.SetActive(true);
        if (_curMaskType != type)
        {
            if (type == GuideMaskType.Circle)
            {
                if (_maskCircle == null)
                    _maskCircle = ResourceLoadManager.Instance.LoadResources("InternalTexture/GuideMaskCircle") as Texture2D;
                _ui.Tex_GuideMask.mainTexture = _maskCircle;
            }
            else
            {
                if (_maskRect == null)
                    _maskRect = ResourceLoadManager.Instance.LoadResources("InternalTexture/GuideMaskRect") as Texture2D;
                _ui.Tex_GuideMask.mainTexture = _maskRect;
            }
        }
        SetUVRect(_ui.Tex_GuideMask, position, size);
    }

    private Vector3 _maskPos;
    private Vector2 _maskSize;
    private void SetUVRect()
    {
        Rect rect = new Rect();
        rect.width = (float)_ui.Tex_GuideMask.width / 2 / (_maskSize.x - 30);
        rect.height = (float)_ui.Tex_GuideMask.height / 2 / (_maskSize.y - 30);
        rect.x = -rect.width / 2 + 0.5f - _maskPos.x * (512 / (_maskSize.x - 30)) / 1024;
        rect.y = -rect.height / 2 + 0.5f - _maskPos.y * (288 / (_maskSize.y - 30)) / 576;
        _ui.Tex_GuideMask.uvRect = rect;
    }

    private void SetUVRect(UITexture tex,Vector3 position, Vector2 size)
    {
        _maskPos = position;
        _maskSize = size;
        Assets.Script.Common.Scheduler.Instance.AddFrame(2, false, SetUVRect);
    }
    
    public void SetFinger(List<int> paras)
    {
        if(paras.Count <4)
        {
            return;
        }
        SetFinger(new Vector3(paras[0], paras[1], 0), paras[2], paras[3]);
    }
    private void SetFinger(Vector3 position, int angleY,int angleZ)
    {
        _ui.Trans_Finger.gameObject.SetActive(true);
        _ui.Trans_Finger.transform.localPosition = position;
        _ui.Trans_Finger.localRotation = Quaternion.Euler(new Vector3(0, angleY, angleZ));
    }

    public void SetArrow(List<int> paras)
    {
        if (paras == null || paras.Count <= 0)
            return;

        if (paras.Count >= 6)
        {
            SetArrow(1, new Vector3(paras[0], paras[1], 0), paras[2]);
            SetArrow(2, new Vector3(paras[3], paras[4], 0), paras[5]);
        }
        else if (paras.Count >= 3)
        {
            SetArrow(1, new Vector3(paras[0], paras[1], 0), paras[2]);            
        }
    }
    private void SetArrow(int arrowIndex, Vector3 position, int angle)
    {
        if (arrowIndex == 1)
        {
            _ui.Trans_Arrow01.gameObject.SetActive(true);
            _ui.Trans_Arrow01.localPosition = position;
            _ui.Trans_Arrow01.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            _ui.Trans_Arrow02.gameObject.SetActive(true);
            _ui.Trans_Arrow02.localPosition = position;
            _ui.Trans_Arrow02.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        
    }

    /*
    public void SetFinger(Vector4 posSize,int angle) { SetFinger(new Vector3(posSize.x, posSize.y, 0), new Vector2(posSize.z, posSize.w),angle); }
    public void SetFinger(Vector3 position, Vector2 size, int angle)
    {
        _ui.Trans_Finger.gameObject.SetActive(true);
        _ui.Trans_Finger.localPosition = position;
        _ui.Trans_Finger.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _ui.Tex_Finger.transform.localPosition = new Vector3(-size.x / 2, -size.y / 2, 0);
    }
    */
    private void InitParam()
    {
        if (_colliders != null)
            return;

        _colliders = _ui.Gobj_Colliders.GetComponents<BoxCollider>();

    }

    private void ClickMask(GameObject go)
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.ClickGuideMask);
    }

    public override void Uninitialize()
    {
        CloseAllGuideItem();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(_ui.Gobj_Colliders).onClick = ClickMask;
    }

    public override void Destroy()
    {
        CloseAllGuideItem();
       // base.Destroy();
        _colliders = null;
    }
}

