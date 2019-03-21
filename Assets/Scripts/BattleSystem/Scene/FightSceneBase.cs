using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 场景基类
/// </summary>
public class FightSceneBase : MonoBehaviour
{

    /// <summary>
    /// 场景摄像机
    /// </summary>
    [HideInInspector]
    public Camera sceneCamera;
    /// <summary>
    /// 震屏摄像机
    /// </summary>
    public Camera shakeCamera;
    /// <summary>
    /// 场景总物件
    /// </summary>
    [HideInInspector]
    public Transform transContent;
    /// <summary>
    /// 其它物体
    /// </summary>
    [HideInInspector]
    public Transform transOther;
    /// <summary>
    /// 英雄刷新点
    /// </summary>
    [HideInInspector]
    public Transform transHero;
    /// <summary>
    /// 自己队列刷新点
    /// </summary>
    [HideInInspector]
    public Transform transSelf;
    /// <summary>
    /// 敌方队列刷新点
    /// </summary>
    [HideInInspector]
    public Transform transEnemy;
    /// <summary>
    /// 场景左边缘限制
    /// </summary>
    [HideInInspector]
    public float limitLeft;
    /// <summary>
    /// 场景右边缘限制
    /// </summary>
    [HideInInspector]
    public float limitRight;
    /// <summary>
    /// 战斗场景背景图
    /// </summary>
    protected UITexture fightBackGround;
    /// <summary>
    /// 背景图
    /// </summary>
    protected List<UITexture> ListTexture = new List<UITexture>();



    /// <summary>
    /// 标志物件
    /// </summary>
    public Dictionary<ESceneMarkType, RoleAttribute> _DicMarkObj = new Dictionary<ESceneMarkType, RoleAttribute>();
    /// <summary>
    /// 场景对应关卡数据
    /// </summary>
    protected CreateSceneInfo sceneInfo;
    /// <summary>
    /// 屏幕宽
    /// </summary>
    protected float _CenterScreenWidth;
    /// <summary>
    /// 角色行走路径Y坐标
    /// </summary>
    protected List<float> _ListRolePath = new List<float>();
    /// <summary>
    /// 角色行走路径数量
    /// </summary>
    protected int _RolePathCount;
    /// <summary>
    /// 英雄初始位置
    /// </summary>
    protected Vector3 _HeroInitPos;
    /// <summary>
    /// 角色管理器索引
    /// </summary>
    protected RoleManager pRoleManager;
    /// <summary>
    /// 屏幕X坐标比例
    /// </summary>
    private float _ScreenProportion_X;
    /// <summary>
    /// 屏幕Y坐标比例
    /// </summary>
    private float _ScreenProportion_Y;
    /// <summary>
    /// 战斗场景状态
    /// </summary>
    public ESceneStatus _SceneStatus;
    /// <summary>
    /// 英雄行走路径索引
    /// </summary>
    protected int pathIndex_Hero;
    /// <summary>
    /// 战斗失败原因
    /// </summary>
    protected EPVEFinishStatus fightFailReason;
    /// <summary>
    /// 场景移动前位置
    /// </summary>
    protected Vector3 initPos;
    /// <summary>
    /// 实际背景图宽
    /// </summary>
    protected float actualWidth;
    /// <summary>
    /// 实际背景图高
    /// </summary>
    protected float actualHeight;
    /// <summary>
    /// 场景比例-宽
    /// </summary>
    public float SceneWidthSize;
    public float MaxGroundWidth = 2048;
        



    public uint Get_CurrentSceneID
    {
        get {
            if (sceneInfo == null)
                return 0;
            else
                return sceneInfo.sceneID;
        }
    }
    /// <summary>
    /// 屏幕宽
    /// </summary>
    public float Get_CenterScreenWidth
    {
        get {
            return _CenterScreenWidth;
        }
    }
    public float Get_ActualWidth
    {
        get {
            return actualWidth;
        }
    }
    public float Get_ActualHeight
    {
        get {
            return actualHeight;
        }
    }
    /// <summary>
    /// 路径数
    /// </summary>
    public int Get_RolePathCount
    {
        get {
            return _RolePathCount;
        }
    }
    /// <summary>
    /// 英雄初始位置
    /// </summary>
    public Vector3 Get_HeroInitPos
    {
        get {
            return _HeroInitPos;
        }
    }
    /// <summary>
    /// 屏幕X坐标比例
    /// </summary>
    public float Get_ScreenProportion_X
    {
        get {
            return _ScreenProportion_X;
        }
    }
    /// <summary>
    /// 屏幕Y坐标比例
    /// </summary>
    public float Get_ScreenProportion_Y
    {
        get {
            return _ScreenProportion_Y;
        }
    }
    /// <summary>
    /// 战斗场景状态
    /// </summary>
    public ESceneStatus Get_SceneStatus
    {
        get {
            return _SceneStatus;
        }
    }
    /// <summary>
    /// 获取英雄行走路径索引
    /// </summary>
    public int Get_PathIndex_Hero
    {
        get {
            return pathIndex_Hero;
        }
    }
    /// <summary>
    /// 获取其余行走路径
    /// </summary>
    public int Get_OtherIndex
    {
        get {
            int tmpPathIndex = Random.Range(0, _RolePathCount);
            if (tmpPathIndex == pathIndex_Hero)
                tmpPathIndex += 1;
            return tmpPathIndex;
        }
    }
    /// <summary>
    /// 获取战斗失败原因
    /// </summary>
    public EPVEFinishStatus Get_FightFailReason
    {
        get {
            return fightFailReason;
        }
    }



    /// <summary>
    /// 初始化场景
    /// </summary>
    public virtual void Initialize(CreateSceneInfo vInfo)
    {
        pRoleManager = RoleManager.Instance;
        SetLocalUseComponentsAndDatas();
        if (sceneCamera != null)
            _CenterScreenWidth = sceneCamera.GetScreenWidth();
        //if (transContent != null)
        //    transContent.transform.gameObject.AddComponent<ShakeGameObj>();

        if (vInfo != null)
            sceneInfo = new CreateSceneInfo(vInfo.sceneType, vInfo.sceneID, vInfo.sceneBackGround);

        _ScreenProportion_X = 1 / this.transform.localScale.x;
        _ScreenProportion_Y = 1 / this.transform.localScale.y;
        AddRolePath();
        SetFightSceneBackGround();
    }

    /// <summary>
    /// 销毁场景
    /// </summary>
    public virtual void Uninitialize()
    {
        //清空引用//
        pRoleManager = null;
        if (this._DicMarkObj != null)
            this._DicMarkObj.Clear();
        //清空角色信息//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        if (ListTexture != null)
        {
            for (int i = 0; i < ListTexture.Count; i++)
            {
                if (ListTexture[i] == null)
                    continue;
                ListTexture[i].mainTexture = null;
            }
        }
        if (fightBackGround != null)
        {
            fightBackGround.mainTexture = null;
        }

        ClearRolePath();
        //System.GC.Collect();
        Destroy(this.gameObject);

       // Resources.UnloadAsset(this.gameObject);
       // Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 设置本地端组件
    /// </summary>
    protected virtual void SetLocalUseComponentsAndDatas()
    {
        actualWidth = GlobalConst.DEFAULT_SCREEN_SIZE_X;
        actualHeight = GlobalConst.DEFAULT_SCREEN_SIZE_Y;
        SceneWidthSize = 1;
        if (fightBackGround != null)
        {
            UIWidget mWidget = fightBackGround.GetComponent<UIWidget>();
            if (mWidget != null)
            {
                Vector3 size = (mWidget != null) ? new Vector3(mWidget.width, mWidget.height) : fightBackGround.transform.localScale;
                UIRoot mRoot = NGUITools.FindInParents<UIRoot>(gameObject);
                actualWidth = Mathf.RoundToInt(sceneCamera.pixelWidth * mRoot.pixelSizeAdjustment);
                actualHeight = Mathf.RoundToInt(sceneCamera.pixelHeight * mRoot.pixelSizeAdjustment);
            }
        }
        float tmpSizeWidth = GlobalConst.DEFAULT_SCREEN_SIZE_X / actualWidth;
        float tmpSizeHeight = GlobalConst.DEFAULT_SCREEN_SIZE_Y / actualHeight;
        float tmpSizeMin = (tmpSizeWidth < tmpSizeHeight) ? tmpSizeWidth : tmpSizeHeight;
        if (sceneCamera != null)
        {
            if ((GlobalConst.DEFAULT_SCREEN_SIZE_X / GlobalConst.DEFAULT_SCREEN_SIZE_Y) == (actualWidth / actualHeight))
                sceneCamera.orthographicSize = 1;
            else
                sceneCamera.orthographicSize = tmpSizeMin;
        }
        shakeCamera.orthographicSize = sceneCamera.orthographicSize;
        //SceneWidthSize = sceneCamera.orthographicSize;
        SceneWidthSize = sceneCamera.pixelWidth / GlobalConst.DEFAULT_SCREEN_SIZE_X;
        if (transOther != null)
        {
            limitLeft = limitLeft + (GlobalConst.DEFAULT_SCREEN_SIZE_X - actualWidth * sceneCamera.orthographicSize) / 2;
            limitRight = limitRight - (GlobalConst.DEFAULT_SCREEN_SIZE_X - actualWidth * sceneCamera.orthographicSize) / 2;
            transOther.transform.localPosition = new Vector3(limitLeft, 0, 0);
        }
    }

    /// <summary>
    /// 设置战斗场景背景图
    /// </summary>
    protected virtual void SetFightSceneBackGround() { }

    /// <summary>
    /// 重置场景状态
    /// </summary>
    public virtual void ReSetStatus(object vData)
    {
        SetFightSceneBackGround();
    }

    /// <summary>
    /// 胜利操作
    /// </summary>
    public virtual bool Operate_Victory()
    {
        if (_SceneStatus != ESceneStatus.essNormal)
            return false;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Voice_Win, this.transform));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_ReduceVolume_Music);
        return true;
    }

    /// <summary>
    /// 失败操作
    /// </summary>
    public virtual bool Operate_Failure()
    {
        if (_SceneStatus != ESceneStatus.essNormal)
            return false;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Voice_Lose, this.transform));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_ReduceVolume_Music);
        return true;
    }

    /// <summary>
    /// 添加角色行走路线
    /// </summary>
    protected virtual void AddRolePath() { }

    /// <summary>
    /// 修改战斗速度操作
    /// </summary>
    /// <param name="vFightSpeed"></param>
    public virtual void ChangeFightSpeedOperate(float vFightSpeed)
    {
        if (pRoleManager == null)
            return;
        if (pRoleManager.Get_RoleDic == null)
            return;
        if (pRoleManager.Get_RoleDic.Count <= 0)
            return;
        foreach (KeyValuePair<int, RoleBase> tmpInfo in pRoleManager.Get_RoleDic)
        {
            if (tmpInfo.Value == null)
                continue;
            tmpInfo.Value.ReSetShowSpeed(vFightSpeed);
        }
    }

    /// <summary>
    /// 资源加载完毕
    /// </summary>
    protected virtual void CheckResourceIsReady(RoleBase vRole)
    {
        UISystem.Instance.HintView.FightIsReady();
    }

    /// <summary>
    /// 预加载资源
    /// </summary>
    protected virtual void PreLoadResource() { }

    /// <summary>
    /// 添加一条行走路径
    /// </summary>
    /// <param name="vPathValue">Y值</param>
    protected void AddSingleRolePath(float vPathValue)
    {
        if (_ListRolePath.Contains(vPathValue))
            return;
        _ListRolePath.Add(vPathValue);
        _RolePathCount = _ListRolePath.Count;
    }
    /// <summary>
    /// 删除指定路线行走路径
    /// </summary>
    /// <param name="vIndex">路径索引</param>
    protected void DelSingleRolePath(int vIndex)
    {
        if ((vIndex < 0) || (vIndex >= _ListRolePath.Count))
            return;
        _ListRolePath.RemoveAt(vIndex);
        _RolePathCount = _ListRolePath.Count;
    }
    /// <summary>
    /// 清空所有行走路径
    /// </summary>
    protected void ClearRolePath()
    {
        _ListRolePath.Clear();
        _RolePathCount = 0;
    }

    /// <summary>
    /// 获取指定路线的角色坐标Y值
    /// </summary>
    /// <param name="vIndex">路径索引</param>
    /// <returns></returns>
    public float GetSingleRolePath(int vIndex)
    {
        if ((vIndex < 0) || (vIndex >= _ListRolePath.Count))
            return 0;
        return _ListRolePath[vIndex];
    }

    /// <summary>
    /// 重置场景到移动前位置
    /// </summary>
    public void ReSetScenePos()
    {
        if (transContent == null)
            return;
        transContent.localPosition = initPos;
    }

}
