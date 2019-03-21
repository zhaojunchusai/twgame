using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Script.Common;
using fogs.proto.msg;
using TdSpine;

public class SpecilEffect
{
    protected SkillSpecilInfo info = null;
    protected List<RoleAttribute> targetList = null;
    protected float ScaleRange = 1.0f;
    //protected SpineBase spine;
    protected RoleAttribute resource = null;
    public virtual void initialize(SkillSpecilInfo info, RoleAttribute role)
    {
        this.info = info;
        this.targetList = new List<RoleAttribute>();
        this.resource = role;
    }
    public void SetTarget(List<RoleAttribute> targetList, float scale)
    {
        if (targetList.Find(
            (RoleAttribute role) => 
            { 
                if (role != null) 
                {
                    if (role.Get_RoleType == ERoleType.ertHero)
                        return true;
                    //RoleHero temp = role as RoleHero;
                    //if (temp != null)
                    //    return true;
                    //RoleAIHero tp = role as RoleAIHero;
                    //if (tp != null)
                    //    return true;
                }
                return false; 
            }) != null)
        {
            this.ScaleRange = scale;
        }
        this.targetList.Clear();
        this.targetList.Capacity = targetList.Count + 1;
        this.targetList.AddRange(targetList);
    }
    public void DoSpecilEffect()
    {
        float roleAttributeScale = 1.0f;
        if (this.resource != null)
        {
            roleAttributeScale = this.resource.Get_SkillTimeScale;
        }

        UpdateTimeTool.Instance.AddTimer((((float)info.timeDelay * PlayerData.Instance._TimeScale) / 1000.0f) / roleAttributeScale, false, this._doSpecilEffect);
    }
    public void OnDestroy()
    {

    }
    ~SpecilEffect()
    {

    }
    private void _doSpecilEffect()
    {
        List<RoleAttribute> destList = this.targetList;
        foreach (RoleAttribute tempRole in destList)
        {
            RoleAttribute myRole = tempRole;
            if (!myRole.IsLive() && this.info.followSpecil == 1)
                continue;
            if (myRole.SpecilDepotManage == null)
                continue;

            if (myRole.transform.localScale.x == 0)
                continue;

            if (this.info != null && info.effectName != "0" && myRole != null)
            {
                float scale = SkillTool.GetLocalScare(myRole);
                EffectObjectCache.Instance.LoadGameObject(info.effectName,
                (GameObject gb) =>
                {
                    if (myRole == null) return;
                    GameObject go = null;
                    if (this.info.followSpecil == 1)
                    {
                        go = CommonFunction.SetParent(gb, myRole.transform);
                        go.transform.localPosition = SkillTool.GetBonePosition(myRole, this.info.effectType);
                        
                        go.transform.localScale = new Vector3
                            (
                            (1.0f / myRole.transform.localScale.x) * this.ScaleRange * scale,
                            (1.0f / myRole.transform.localScale.x) * this.ScaleRange * scale,
                            (1.0f / myRole.transform.localScale.x) * this.ScaleRange
                            );
                    }
                    else
                    {
                        go = CommonFunction.SetParent(gb, SceneManager.Instance.Get_CurScene.transOther);
                        go.transform.position = SkillTool.GetWorldPosition(myRole, this.info.effectType);
                        go.transform.localScale = new Vector3
                            (
                            1.0f * this.ScaleRange,
                            1.0f * this.ScaleRange,
                            1.0f * this.ScaleRange
                            );
                    }
                    SpineBase spine;
                    spine = go.GetComponent<SpineBase>();
                    if (spine == null)
                        spine = go.AddComponent<SpineBase>();
                    if(spine.skeletonAnimation == null)
                        spine.InitSkeletonAnimation();
                    else
                        spine.skeletonAnimation.Reset();
                    spine.Clear();
                    spine.setTimeScare(1.0f);
                    if (myRole.Get_MainSpine)
                    {
                        if (this.info.sortLayer != 2)
                            spine.setSortingOrder(myRole.Get_MainSpine.GetMaxSort() + 1);
                        else
                        {
                            spine.setSortingOrder(1);
                        }
                    }
                    else
                    {
                        spine.setSortingOrder(1);
                    }
                    if (this.info.during == 0)
                    {
                        spine.pushAnimation("animation", false, 1);
                        spine.EndEvent += (string s) => { if (go != null) EffectObjectCache.Instance.FreeObject(go); };
                        if (this.info.effectMusic != "0" && this.info.effectMusic != string.Empty)
                        {
                            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(this.info.effectMusic, go.transform));
                        }
                    }
                    else
                    {
                        spine.pushAnimation("animation", true, 1);
                        if (this.info.followSpecil == 1)
                        {
                            SpecilBuff tempBuff = new SpecilBuff();
                            tempBuff.initialize(this.info.effectName, spine, this.info.during, go);

                            if (myRole.SpecilDepotManage == null)
                                return;

                            SpecilBuff result = myRole.SpecilDepotManage.IsContain(tempBuff);
                            if (result != null)
                            {
                                EffectObjectCache.Instance.FreeObject(go);
                                result.time = tempBuff.during;
                            }
                            else
                            {
                                tempBuff.Activate();
                                if (this.info.effectMusic != "0" && this.info.effectMusic != string.Empty)
                                {
                                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(this.info.effectMusic, go.transform));
                                }
                                myRole.SpecilDepotManage.PushSpecilBuff(tempBuff);
                            }
                        }
                        else
                        {
                            SpecilBuff tempBuff = new SpecilBuff();
                            tempBuff.initialize(this.info.effectName, spine, this.info.during, go);
                            tempBuff.Activate();
                            //UpdateTimeTool.Instance.AddTimer
                            //    (
                            //    (float)this.info.during / 1000.0f,
                            //    false,
                            //    () =>
                            //    {
                            //        Debug.LogError("FreeObject");
                            //        if (go != null) EffectObjectCache.Instance.FreeObject(go);
                            //    }
                            //    );
                        }
                    }
                    SkillManage.Instance.PushSpine(spine);
                });
            }
        }
    }
    public void RemoveSpecil(List<RoleAttribute> list)
    {
        if (list != null && this.info != null)
        {
            foreach (RoleAttribute role in list)
            {
                if (role != null && role.SpecilDepotManage != null)
                {
                    role.SpecilDepotManage.RemoveSpecilBuff(this.info.effectName);
                }
            }
        }
    }
}
public class SpecilBuff
{
    public string specilName = "";
    public SpineBase spine = null;
    public float during = 0;
    public float time = 0.0f;
    public SpecilDepot father;
    public GameObject go = null;
    public void initialize(string name, SpineBase spine, int duringTime, GameObject go)
    {
        this.specilName = name;
        this.spine = spine;
        this.during = (float)duringTime / 1000.0f;
        this.go = go;
    }
    ~SpecilBuff()
    {
        UpdateTimeTool.Instance.RemoveUpdator(this.Update);
        UpdateTimeTool.Instance.DelayDelGameObject(go);
        if (father != null)
            father.SpecilList.Remove(this);
    }
    public void OnDestory()
    {
        UpdateTimeTool.Instance.RemoveUpdator(this.Update);
        EffectObjectCache.Instance.FreeObject(go);
        if (father != null)
            father.SpecilList.Remove(this);
    }
    public void RefreshTime(float time)
    {
        this.during = time;
        this.time = this.during;
    }
    public void RefreshTime()
    {
        this.time = this.during;
    }
    public void Activate()
    {
        this.time = this.during;
        UpdateTimeTool.Instance.AddUpdator(this.Update);
    }
    public void Update(float detail)
    {
        if (this.time <= 0)
        {
            this.OnDestory();
            return;
        }
        else
        {
            this.time -= detail;
            //if (this.time <= 0)
            //    this.OnDestory();
        }
    }
}
public class SpecilDepot
{
    public List<SpecilBuff> SpecilList = null;
    public SpecilDepot()
    {
        SpecilList = new List<SpecilBuff>();
    }
    public void PushSpecilBuff(SpecilBuff specilBuff)
    {
        SpecilBuff temp = this.IsContain(specilBuff);
        if (temp != null)
        {
            temp.OnDestory();
        }
        else
        {
            this.SpecilList.Add(specilBuff);
            specilBuff.father = this;
        }
    }
    public void RemoveSpecilBuff(string name)
    {
        if (name == null) return;
        SpecilBuff temp = this.SpecilList.Find((SpecilBuff bf) => { if (bf == null) return false; return bf.specilName == name; });
        if (temp != null)
        {
            temp.OnDestory();
            this.SpecilList.Remove(temp);
        }
    }
    public void Destory()
    {
        if(this.SpecilList != null && this.SpecilList.Count > 0)
        {
            for (int i = 0; i < this.SpecilList.Count; ++i )
            {
                SpecilBuff tmp = this.SpecilList[i];
                if (tmp != null)
                    tmp.OnDestory();
            }
            this.SpecilList.Clear();
        }
    }
    public SpecilBuff IsContain(SpecilBuff specilBuff)
    {
        SpecilBuff temp = this.SpecilList.Find((SpecilBuff bf) => { if (bf == null) return false; if (bf.time < 0 || bf.go == null || bf.go.activeSelf == false) { return false; } return bf.specilName == specilBuff.specilName; });
        return temp;
    }
}

public class SkillManage
{
    public float currTimeScale = 1.5f;
    List<SpineBase> ListSpine;

    private static SkillManage pInstance;
    public static SkillManage Instance
    {
        get
        {
            if (pInstance == null)
                pInstance = new SkillManage();
            return pInstance;
        }
    }
    SkillManage()
    {
        ListSpine = new List<SpineBase>();
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SetPause);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SetResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
    }
    ~SkillManage()
    {
        ListSpine = null;
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SetPause);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SetResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
    }

    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetPause(object vDataObj)
    {
        foreach (SpineBase tempSpine in this.ListSpine)
        {
            if (tempSpine == null)
            {
                continue;
            }
            tempSpine.Pause();
        }

        UpdateTimeTool.Instance.Pause();
    }
    /// <summary>
    /// 继续
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetResume(object vDataObj)
    {
        foreach (SpineBase tempSpine in this.ListSpine)
        {
            if (tempSpine == null)
            {
                continue;
            }
            tempSpine.Resume();
        }

        UpdateTimeTool.Instance.Resume();
    }
    /// <summary>
    /// 修改战斗速度
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ChangeSpeed(object vDataObj)
    {
        if (vDataObj == null)
            return;
        float tmpTimeScale = (float)vDataObj;
        if (tmpTimeScale == GlobalConst.MIN_FIGHT_SPEED)
            tmpTimeScale = 1.5f;
        if (tmpTimeScale == 0.0f)
            return;
        this.currTimeScale = tmpTimeScale;
        foreach (SpineBase tempSpine in this.ListSpine)
        {
            if (tempSpine == null)
                continue;
            tempSpine.SetMultiple(this.currTimeScale);
        }
        UpdateTimeTool.Instance.SetTimeScale(this.currTimeScale);
    }

    public void PushSpine(SpineBase spine)
    {
        if (spine == null)
            return;
        if (this.ListSpine.Contains(spine))
            return;

        _RemoveNull();
        this.ListSpine.Add(spine);
        spine.SetMultiple(currTimeScale);
    }

    /// <summary>
    /// 删除所有技能
    /// </summary>
    public void RemoveAll()
    {
        for (int i = (this.ListSpine.Count - 1); i >= 0;--i )
        {
            SpineBase temp = this.ListSpine[i];
            if (temp != null)
                GameObject.Destroy(temp.gameObject);
            temp = null;
        }
        this.ListSpine.Clear();
    }

    public void RemoveSpine(SpineBase spine)
    {
        this.ListSpine.Remove(spine);
        UpdateTimeTool.Instance.Remove();
    }

    private void _RemoveNull()
    {
        this.ListSpine.RemoveAll((SpineBase tempSpine) => { return tempSpine == null; });
    }
}
public class NoTargetRole
{
    List<Vector3> destPosition;
    Vector3 startPostion;
    SkillAttributeInfo skillInfo;
    List<SkillEffectInfo> EffectInfoList;
    float roleAttributeScale = 1.0f;
    Traectory tt;
    ERoleDirection direction = ERoleDirection.erdRight;
    public void Init(RoleAttribute vResource, SkillAttributeInfo vSkillInfo)
    {
        if (vResource == null || vSkillInfo == null)
            return;
        if (SceneManager.Instance.Get_CurScene == null)
            return;

        this.skillInfo = vSkillInfo;

        startPostion = SkillTool.GetWorldPosition(vResource, skillInfo.trajectoryPosition);

        this.EffectInfoList = ConfigManager.Instance.mSkillEffectData.FindAllByID(this.skillInfo.nId);

        if (this.EffectInfoList.Count <= 0)
            return;

        if (vResource != null)
        {
            roleAttributeScale = vResource.Get_SkillTimeScale;
        }

        this.destPosition = new List<Vector3>(this.EffectInfoList.Count);
        for (int i = 0; i < this.EffectInfoList.Count; ++i)
        {
            SkillEffectInfo tempEffectInfo = this.EffectInfoList[i];

            if (tempEffectInfo == null)
            {
                destPosition.Add(Vector3.zero);
                continue;
            }

            float distance = 0;

            distance = ((float)(tempEffectInfo.targetLockonInfo.maxDistance));
            direction = vResource.Get_Direction;
            if (vResource.Get_Direction == ERoleDirection.erdLeft)
                distance = -distance;

            distance = distance / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
            destPosition.Add(new Vector3(vResource.transform.position.x + distance, startPostion.y, 0));
        }
        this.Active();
    }
    private void Active()
    {

        if (this.skillInfo.trajectory == "0")
        {
            DestSpecil();
        }
        else
        {
            if (this.skillInfo.noTarget == 0)
            {
                this.DestSpecil();
                return;
            }
            this.tt = null;
            switch (this.skillInfo.trajectoryType)
            {
                case 1: this.tt = new PerpenDicular(); break;
                case 2: this.tt = new Parabola(); break;
                case 3: this.tt = new FreelyFalling(); break;
                case 4: this.tt = new TransverseLine(); break;
                default: this.tt = new Parabola(); break;
            }
            UpdateTimeTool.Instance.AddTimer(((float)this.skillInfo.conjureDelay / 1000.0f) / roleAttributeScale, false, this.Trajectory);
            tt.ColliderEvent += (int id) =>
            {
                DestSpecil();
            };
        }
    }
    private void Trajectory()
    {
        tt.Instance(startPostion, null, this.skillInfo.trajectory, 2, this.destPosition[0]);
        tt.SetSpeed(this.skillInfo.speed);
    }
    private void DestSpecil()
    {
        for (int i = 0; i < this.EffectInfoList.Count; ++i)
        {
            if (i >= this.destPosition.Count)
                break;

            int count = i;
            SkillEffectInfo tempInfo = this.EffectInfoList[i];
            foreach (SkillSpecilInfo specilInfo in tempInfo.specilInfoList)
            {
                if (specilInfo == null)
                    continue;
                if (specilInfo.noTarget == 0)
                    continue;
                SkillSpecilInfo tempSpecilInfo = specilInfo;

                UpdateTimeTool.Instance.AddTimer(
                    ((float)tempSpecilInfo.timeDelay / 1000.0f) / roleAttributeScale,
                    false,
                    () =>
                    {
                        int num = count;

                        EffectObjectCache.Instance.LoadGameObject(tempSpecilInfo.effectName,
                        (GameObject gb) =>
                        {
                            GameObject go = CommonFunction.SetParent(gb, SceneManager.Instance.Get_CurScene.transOther);
                            go.transform.position = this.destPosition[num];
                            go.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                            TdSpine.SpineBase spine = go.GetComponent<SpineBase>();
                            if (spine == null)
                                spine = go.AddComponent<SpineBase>();
                            spine.InitSkeletonAnimation();
                            //spine.skeletonAnimation.Reset();
                            if (this.direction == ERoleDirection.erdRight)
                                spine.TurnLeft();
                            if (tempSpecilInfo.sortLayer != 2)
                                spine.setSortingOrder(30);
                            else
                            {
                                spine.setSortingOrder(1);
                            }
                            if (tempSpecilInfo.effectMusic != "0" && tempSpecilInfo.effectMusic != string.Empty)
                            {
                                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(tempSpecilInfo.effectMusic, go.transform));
                            }

                            if (tempSpecilInfo.during == 0)
                            {
                                spine.pushAnimation("animation", false, 1);
                                spine.EndEvent += (string s) => { if (go != null) EffectObjectCache.Instance.FreeObject(go); };
                            }
                            else
                            {
                                spine.pushAnimation("animation", true, 1);
                                UpdateTimeTool.Instance.AddTimer
                                (
                                (float)tempSpecilInfo.during / 1000.0f,
                                false,
                                () => { if (go != null) EffectObjectCache.Instance.FreeObject(go); }
                                );
                            }
                            SkillManage.Instance.PushSpine(spine);
                        });
                    });
            }
        }
    }
}
