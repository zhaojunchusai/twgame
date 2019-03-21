using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public enum SkillCheck
{
    none = 200,
    Level,//等级原因
    Money = 202,//金钱原因
    Goods = 903,//材料原因
    MaxLevel = 325,//最大等级
    HeroLevelLower = 327,//英雄等级不足
    SoldierLevelLower = 328,//士兵等级不足
    Ok//成功
}
public enum SkillChange
{
    All,//整个列表发生变化
    One//某一个发生变化
}
public enum SkillControl
{
    /// <summary>
    /// 技能升级
    /// </summary>
    UpgradeSkillResp
}

public class Skill
{
    public static int MAXQUALITY = 6;
    public static int MAXSTAR = 6;

    private SkillAttributeInfo att;
    private Effects _effect;
    private object _resource;
    public int Level;
    public int Slot = -1;
    public UInt64 uId = 0;
    public Texture Icon = null;
    public bool isActive = false;
    public RoleAttribute target;
    public Soldier soldier;
    private Traectory tt;
    private int LockLevel = -1;
    public bool islock = false;
    public SkillAttributeInfo Att { get { return att; } }//获取装备的信息
    public int Lv { get { return this.Level; } }//获取装备当前等级
    public Effects _Effect { get { return this._effect; } }//获取效果器

    public static Skill createByID(uint Id)
    {
        Skill tempSkill = new Skill();
        tempSkill.att = ConfigManager.Instance.mSkillAttData.FindById(Id);
        if (tempSkill.att == null) return null;
        tempSkill.uId = (UInt64)tempSkill.att.nId;
        tempSkill.Level = tempSkill.att.initLevel;
        return tempSkill;
    }
    public bool Serialize(fogs.proto.msg.Skill skill)//读档
    {
        if (skill != null)
            this.Level = (int)skill.level;
        return true;
    }
    ~Skill()
    {
        UpdateTimeTool.Instance.RemoveTimer(ShakeEffect);
        UpdateTimeTool.Instance.RemoveTimer(ConjureSpecil);
        UpdateTimeTool.Instance.RemoveTimer(Trajectory);
    }
    public void CacheEffect()
    {
        if (!this.Att.conjure.Equals("0") && !this.Att.conjure.Equals(string.Empty))
        {
            EffectObjectCache.Instance.LoadGameObject(this.Att.conjure, (go) =>
            {
                EffectObjectCache.Instance.FreeObject(go);
            });
        }
        if (!this.Att.trajectory.Equals("0") && !this.Att.trajectory.Equals(string.Empty))
        {
            EffectObjectCache.Instance.LoadGameObject(this.Att.trajectory, (go) =>
            {
                EffectObjectCache.Instance.FreeObject(go);
            });
        }

        List<SkillEffectInfo> EffectList = ConfigManager.Instance.mSkillEffectData.FindAllByID(this.att.nId);

        if (EffectList == null)
            return;

        for (int i = 0; i < EffectList.Count; ++i )
        {
            SkillEffectInfo tmpInfo = EffectList[i];
            if (tmpInfo == null || tmpInfo.specilInfoList == null)
                continue;
            foreach(SkillSpecilInfo sf in tmpInfo.specilInfoList)
            {
                if (sf == null || tmpInfo.targetFindInfo == null)
                    continue;
                if (!sf.effectName.Equals("0") && !sf.effectName.Equals(string.Empty))
                {
                    EffectObjectCache.Instance.LoadGameObject(sf.effectName, (go) =>
                    {
                        EffectObjectCache.Instance.FreeObject(go);
                    },sf.effectName, tmpInfo.targetFindInfo.maxTarget);
                }
                
            }
        }
    }
    public int GetLockLv()
    {
        if (this.LockLevel != -1)
            return this.LockLevel;
        if(this.soldier != null)
            return 1;
        List<HeroAttributeInfo> tempHeroList = ConfigManager.Instance.mHeroData.GetHeroAttributeList();
        if (tempHeroList == null)
            return 1;
        for (int i = 0; i < tempHeroList.Count; ++i)
        {
            HeroAttributeInfo tempHeroAtt = tempHeroList[i];
            if (tempHeroAtt == null || tempHeroAtt.PassiveSkill == null)
                continue;
            if(tempHeroAtt.PassiveSkill.ContainsKey(this.att.nId))
            {
                this.LockLevel = (int)tempHeroAtt.Level;
                break;
            }
        }
        return this.LockLevel;

    }
    public uint GetRootId()
    {
        List<uint> IdMark = new List<uint>(7);
        IdMark.Add(this.att.nId);
        while (true)
        {
            uint tempId = IdMark[IdMark.Count - 1];
            SkillAttributeInfo temp = ConfigManager.Instance.mSkillAttData.FindById(tempId);
            if (temp == null)
                break;
            if (temp.prepositionId == 0)
            {
                IdMark.Add(temp.nId);
                break;
            }
            if (IdMark.Contains(temp.prepositionId))
                break;
            IdMark.Add(temp.prepositionId);
        }
        if (IdMark.Count > 0)
            return IdMark[IdMark.Count - 1];
        return 0;
    }
    /// <summary>
    /// 强化系数
    /// </summary>
    /// <returns></returns>
    public float StrongCoefficient()
    {
        return 20.0f / (float)(this.att.levelLimit - this.att.initLevel + 1);
    }
    public bool IsLock()
    {
        return this.islock;
    }
    /// <summary>
    /// 概率随机技能（用于自动战斗的Hero或者Soldier)
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public bool RandomSkill(RoleAttribute role, float triggerProb)
    {
        if (this.isActive)
            return false;
        UpdateTimeTool.Instance.AddTimer((float)this.att.waitTime / 1000.0f, false, () =>
        {
            this.isActive = false;
        });
        this.isActive = true;
        if (this.att.triggerType == 0) return false;
        if (SkillTool.ProbMachine(triggerProb))
        {
            Skill skill = Skill.createByID(this.att.nId);
            skill.Level = this.Level;
            skill.UseSkill(role);
            return true;
        }
        return false;
    }
    public string GetDescript(int level)
    {
        return string.Format(
            this.att.Description,
            Math.Abs(Convert.ToDouble(this.att.effect1.num + this.att.effect1.u_num * (level - this.att.initLevel))).ToString("0"),
            this.att.effect1.percent + this.att.effect1.u_percent * (level - this.att.initLevel),
            Math.Abs(Convert.ToDouble(this.att.effect2.num + this.att.effect2.u_num * (level - this.att.initLevel))).ToString("0"),
            this.att.effect2.percent + this.att.effect2.u_percent * (level - this.att.initLevel),
            Math.Abs(Convert.ToDouble(this.att.effect3.num + this.att.effect3.u_num * (level - this.att.initLevel))).ToString("0"),
            this.att.effect3.percent + this.att.effect3.u_percent * (level - this.att.initLevel),

            (float)(this.att.effect1.durTime + this.att.effect1.u_durTime * (level - this.att.initLevel)) / 1000.0f,
            (float)(this.att.effect2.durTime + this.att.effect2.u_durTime * (level - this.att.initLevel)) / 1000.0f,
            (float)(this.att.effect3.durTime + this.att.effect3.u_durTime * (level - this.att.initLevel)) / 1000.0f
            );
    }
    public RoleAttribute GetLockTarget(RoleAttribute role)
    {
        List<SkillEffectInfo> EffectList = ConfigManager.Instance.mSkillEffectData.FindAllByID(this.att.nId);

        for (int i = 0; i < EffectList.Count; ++i)
        {
            SkillEffectInfo tempInfp = EffectList[i];
            TargetLockon tempLockon = new TargetLockon();
            tempLockon.initialize(tempInfp.targetLockonInfo);
            tempLockon.SetResource(role);
            if(tempLockon.Target != null)
            {
                 RoleAttribute target = tempLockon.Target as RoleAttribute;
                 if (target != null)
                     return target;
            }
        }
        return null;
    }
    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="role"></param>
    public void UseSkill(RoleAttribute role)
    {
        if(role == null) return;
        if (this.att.triggerType == 0) return;

        List<SkillEffectInfo> EffectList = ConfigManager.Instance.mSkillEffectData.FindAllByID(this.att.nId);
        List<KeyValuePair<TargetLockon, TargetFind>> LockonAndFind = new List<KeyValuePair<TargetLockon, TargetFind>>(EffectList.Count + 1);
        if (EffectList != null || EffectList.Count > 0)
        {
            for (int i = 0; i < EffectList.Count;++i )
            {
                SkillEffectInfo tempInfp = EffectList[i];
                TargetLockon tempLockon = new TargetLockon();
                tempLockon.initialize(tempInfp.targetLockonInfo);

                TargetFind tempFind = new TargetFind();
                tempFind.initialize(tempInfp, role);
                tempFind.SetSkillAttribute(this.att);
                LockonAndFind.Add(new KeyValuePair<TargetLockon, TargetFind>(tempLockon, tempFind));
            }
        }
        this._resource = null;
        this._resource = role;

        float roleAttributeScale = 1.0f;
        if (role != null)
        {
            roleAttributeScale = role.Get_SkillTimeScale;
        }
        UpdateTimeTool.Instance.AddTimer(this.att.conjureTimeDelay / roleAttributeScale, false, ConjureSpecil);

        if(this.att.ShakeDurationTime > 0)
            UpdateTimeTool.Instance.AddTimer(this.att.ShakeDelayTime, false, ShakeEffect);

        if(this.att.trajectory == "0")
        {
            for (int i = 0; i < LockonAndFind.Count; ++i)
            {
                KeyValuePair<TargetLockon, TargetFind> temp = LockonAndFind[i];
                if (temp.Value.skillEffectInfo == null) continue;
                List<EffectInfo> effectList = new List<EffectInfo>(3);

                List<KeyValuePair<int, int>> list = temp.Value.skillEffectInfo.effect;
                for (int j = 0; j < list.Count; ++j)
                {
                    KeyValuePair<int, int> effectInfo = list[j];
                    if (effectInfo.Key > 3 || effectInfo.Key < 1)
                        continue;
                    EffectInfo newInfo = (EffectInfo)this.att.effect[effectInfo.Key - 1].Clone();
                    newInfo.LevelAdd(this.Level - this.att.initLevel);
                    effectList.Add(newInfo);
                }
                temp.Key.SetResource(role);
                temp.Value.resource = role;
                temp.Value.SetEffectInfoList(effectList,this.Level);
                if (temp.Key.Target == null)
                {
                    NoTargetRole noTargetRole = new NoTargetRole();
                    noTargetRole.Init(role, this.Att);
                    continue;
                }
                if ((RoleAttribute)temp.Key.Target == null) continue;
                RoleAttribute target = (RoleAttribute)temp.Key.Target;
                if(target == null)
                {
                    continue;
                }
                temp.Value.SetResource(target);
            }
        }
        else
        {
            if(LockonAndFind.Count < 1) return;
            LockonAndFind[0].Key.SetResource(role);
            object tar = LockonAndFind[0].Key.Target;
            if ((RoleAttribute)tar == null)
            {
                NoTargetRole noTargetRole = new NoTargetRole();
                noTargetRole.Init(role, this.Att);
                return;
            }
            this.target = (RoleAttribute)tar;

            this.tt = null;
            switch(this.att.trajectoryType)
            {
                case 1: this.tt = new PerpenDicular(); break;
                case 2: this.tt = new Parabola(); break;
                case 3: this.tt = new FreelyFalling(); break;
                case 4: this.tt = new TransverseLine(); break;
                default: this.tt = new Parabola(); break;
            }
            UpdateTimeTool.Instance.AddTimer(((float)this.att.conjureDelay / 1000.0f) / roleAttributeScale, false, this.Trajectory);
            this.tt.ColliderEvent += (int id) => 
            {
                RoleAttribute resource = role;
                for (int i = 0; i < LockonAndFind.Count; ++i)
                {
                    KeyValuePair<TargetLockon, TargetFind> temp = LockonAndFind[i];
                    if (temp.Value.skillEffectInfo == null) continue;
                    List<EffectInfo> effectList = new List<EffectInfo>(3);

                    List<KeyValuePair<int, int>> list = temp.Value.skillEffectInfo.effect;
                    for (int j = 0; j < list.Count; ++j)
                    {
                        KeyValuePair<int, int> effectInfo = list[j];
                        if (effectInfo.Key > 3 || effectInfo.Key < 1)
                            continue;
                        EffectInfo newInfo = (EffectInfo)this.att.effect[effectInfo.Key - 1].Clone();
                        newInfo.LevelAdd(this.Level - this.att.initLevel);
                        effectList.Add(newInfo);
                    }

                    temp.Value.resource = resource;
                    temp.Value.SetEffectInfoList(effectList,this.Level);
                    if (tt.dest != null)
                        temp.Value.SetResource(tt.dest);
                    else
                        temp.Value.SetResource(tt.destPos, tt._roleCamp);
                }
            };

            //弹道类
        }
    }
    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="player"></param>
    public void UseSkill(PlayerData player)
    {
        //if (this.att.triggerType == 1) return;
        //foreach (KeyValuePair<TargetLockon, TargetFind> temp in this.LockonAndFind)
        //{
        //    if (temp.Value.skillEffectInfo == null) continue;
        //    List<EffectInfo> effectList = new List<EffectInfo>(3);
        //    foreach (var effectInfo in temp.Value.skillEffectInfo.effect)
        //    {
        //        if (effectInfo.Key > 3 || effectInfo.Key < 1)
        //            continue;
        //        EffectInfo newInfo = (EffectInfo)this.att.effect[effectInfo.Key - 1].Clone();
        //        newInfo.LevelAdd(this.Level - this.att.initLevel + 1);
        //        effectList.Add(newInfo);
        //    }
        //    temp.Key.SetResource(player);
        //    temp.Value.SetEffectInfoList(effectList);
        //    temp.Value.SetResource(player);
        //}

    }
    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="soldier"></param>
    public void UseSkill(Soldier soldier)
    {
        //if (this.att.triggerType == 1) return;

        //foreach (KeyValuePair<TargetLockon, TargetFind> temp in this.LockonAndFind)
        //{
        //    if (temp.Value.skillEffectInfo == null) continue;
        //    List<EffectInfo> effectList = new List<EffectInfo>(3);
        //    foreach (var effectInfo in temp.Value.skillEffectInfo.effect)
        //    {
        //        if (effectInfo.Key > 3 || effectInfo.Key < 1)
        //            continue;
        //        EffectInfo newInfo = (EffectInfo)this.att.effect[effectInfo.Key - 1].Clone();
        //        newInfo.LevelAdd(this.Level - this.att.initLevel + 1);
        //        effectList.Add(newInfo);
        //    }
        //    temp.Key.SetResource(soldier);
        //    temp.Value.SetEffectInfoList(effectList);
        //    temp.Value.SetResource(soldier);
        //}
    }
    public void ShakeEffect()
    {
        if (this == null || SceneManager.Instance.Get_CurScene == null || SceneManager.Instance.Get_CurScene.transContent == null)
        {
            return;
        }
        if (this.att == null)
            return;
        
        //CommonFunction.ShakeGameObj(SceneManager.Instance.Get_CurScene.transContent, this.att.ShakeDurationTime);
        CommonFunction.ShakeGameObj(SceneManager.Instance.Get_CurScene.shakeCamera.transform, this.att.ShakeDurationTime);
    }
    public void ConjureSpecil()
    {
        RoleAttribute role = (RoleAttribute)this._resource;
        if (role == null) return;
        if (this.att == null) return;
        if(this.att.conjure != "0")
        {
            EffectObjectCache.Instance.LoadGameObject(this.att.conjure, (GameObject gb) =>
            {
                if (role == null) return;
                if (gb == null) return;
                if (SceneManager.Instance.Get_CurScene == null) return;
                float scale = SkillTool.GetLocalScare(role);
                GameObject go = null;
                if (this.att.conjureFollow == 1)
                {
                    go = CommonFunction.SetParent(gb, role.transform);
                    go.transform.localPosition = SkillTool.GetBonePosition(role, this.att.conjurePosition);
                    go.transform.localScale = new Vector3
                        (
                        (1.0f / role.transform.localScale.x) * scale,
                        (1.0f / role.transform.localScale.x) * scale,
                        (1.0f / role.transform.localScale.x)
                        );
                }
                else
                {
                    go = CommonFunction.SetParent(gb, SceneManager.Instance.Get_CurScene.transOther);
                    go.transform.position = SkillTool.GetWorldPosition(role, this.att.conjurePosition);
                    go.transform.localScale = new Vector3
                        (
                        1.0f,
                        1.0f,
                        1.0f
                        );
                }

                //GameObject go = CommonFunction.SetParent(gb,role.transform);
                //go.transform.localScale = new Vector3
                //    (
                //    (1.0f / role.transform.localScale.x) * scale,
                //    (1.0f / role.transform.localScale.x) * scale,
                //    1.0f
                //    );
                //go.transform.localPosition = SkillTool.GetBonePosition(role, this.att.conjurePosition);
                
                TdSpine.SpineBase spine = go.GetComponent<TdSpine.SpineBase>();
                if(spine == null)
                    spine = go.AddComponent<TdSpine.SpineBase>();
                spine.InitSkeletonAnimation();
                //spine.skeletonAnimation.Reset();
                spine.setSortingOrder(5);
                if (role.Get_MainSpine != null)
                    spine.setSortingOrder(role.Get_MainSpine.GetMaxSort() + 1);
                if (role.Get_RoleType == ERoleType.ertHero && this.att.conjureFollow != 1)
                {
                    if (role.gameObject.transform.rotation.y != 0)
                    {
                        spine.TurnLeft();
                    }
                }
                spine.pushAnimation("animation", false, 1);
                spine.EndEvent += (string s) => { if (go != null) EffectObjectCache.Instance.FreeObject(go); };
                spine.setTimeScare(1.0f);

                RoleAttribute tempRole = this._resource as RoleAttribute;
                if (tempRole != null)
                    spine.setTimeScare(tempRole.Get_SkillTimeScale);

                if(this.att.conjureMusic != "0")
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(this.att.conjureMusic, go.transform));
                }

                SkillManage.Instance.PushSpine(spine);
            });
        }
    }
    public void Trajectory()
    {
        if ((RoleAttribute)this._resource == null)
            return;
        if (this.target == null)
            return;
        tt.Instance(SkillTool.GetWorldPosition((RoleAttribute)this._resource, this.att.trajectoryPosition), this.target, this.att.trajectory);
        tt.SetSpeed(this.att.speed);
    }
    public SkillCheck enableStrong(bool needCheckGoods = true)//检查是否能够升级
    {
        return _check(needCheckGoods);
    }
    public string Strong(int level)//升级
    {
        _strong(level);
        return "";
    }
    public bool isMaxLevel()
    {
        return this.Level >= att.levelLimit;
    }

    public Skill()
    {
        _effect = new Effects();
    }
    private string _strong(int level)
    {
        this.Level = level;
        return "";
    }
    private SkillCheck _check(bool needCheckGoods = true)
    {
        if(this.isMaxLevel())
        {
            SkillAttributeInfo temp = ConfigManager.Instance.mSkillAttData.FindById(this.att.evolveId);
            if (temp == null)
                return SkillCheck.MaxLevel;
        }
        if(this.soldier != null)
        {
            if (this.Level >= this.soldier.Level)
                return SkillCheck.SoldierLevelLower;
        }
        else
        {
            float num = 20.0f / (float)(this.att.levelLimit - this.att.initLevel + 1);
            int res = (int)((this.Level + 1) * num);
            if (res > PlayerData.Instance._Level)
                return SkillCheck.HeroLevelLower;
        }

        if (!this.Att.materialBag.ContainsKey(this.Level - this.Att.initLevel + 1)) return SkillCheck.none;
        KeyValuePair<KeyValuePair<MaterialBag.MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<fogs.proto.msg.Item, int>>> result = MaterialBag.getResult(this.Att.materialBag[this.Level - this.Att.initLevel + 1]);
        if (result.Key.Key == MaterialBag.MaterialResult.coin || result.Key.Key == MaterialBag.MaterialResult.diamond)
            return SkillCheck.Money;
        if (result.Key.Key == MaterialBag.MaterialResult.material && needCheckGoods)
            return SkillCheck.Goods;
        if (result.Key.Key == MaterialBag.MaterialResult.noId)
            return SkillCheck.Ok;
        return SkillCheck.Ok;
    }
}
public class SkillsDepot
{
    public enum SlotMax { MaxSkill = 8 }
    public List<Skill> _skillsList;
    public Soldier soldier;

    private bool needSort = false;
    private bool needRemoveNull = false;
    private List<Skill> _allSkills;
    //技能栏发生了变化
    public delegate void SkillsDepotDelet(SkillChange change, int Slot = -1, UInt64 uID = 0);
    public event SkillsDepotDelet SkillsDepotEvent;

    public delegate void SkillsErrorDelet(SkillControl control, int errorCode);
    public event SkillsErrorDelet SkillsErrorEvent;
    public void OnSkillsError(SkillControl control, int errorCode)
    {
        if (this.SkillsErrorEvent != null)
            this.SkillsErrorEvent(control,errorCode);
    }
    public void Clear()
    {
        if (this._skillsList != null)
            this._skillsList.Clear();
        if (this._allSkills != null)
            this._allSkills.Clear();
        this.needRemoveNull = false;
        this.needSort = false;
    }
    public void Update()
    {
        _RemoveNull();
        _sort();
    }
    //通过slot找技能
    public Skill FindBySlot(int slot)
    {
        return _skillsList.Find((Skill p) => { return p.Slot == slot; });
    }
    public Skill FindByUid(UInt64 uId)
    {
        return _skillsList.Find((Skill p) => { return p.uId == uId; });
    }
    public Skill FindById(uint id)
    {
        return _skillsList.Find((Skill p) => { return p.Att.nId == id; });
    }
    public bool IsHadSkillCanStrong()
    {
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.HeroSkillControl))
            return false;

        foreach (Skill skill in this._skillsList)
        {
            if (skill.enableStrong() == SkillCheck.Ok)
            {
                return true;
            }
        }
        return false;
    }
    //添加一组ID的装备
    //添加失败会返回false，但是会把能添加的技能添加进去
    public bool multipleAdd(List<uint> ID)
    {
        bool success = true;

        foreach (var temp in ID)
        {
            if (!_Add(temp)) success = false;
        }
        return success;
    }
    //添加一个ID的技能
    public bool oneAdd(uint Id)
    {
        return _Add(Id);
    }
    //添加一个技能
    public bool oneAdd(Skill tp)
    {
        return _Add(tp);
    }
    public Skill oneAdd(fogs.proto.msg.Skill temp)
    {
        if (temp == null)
            return null;
        Skill tempSkill = Skill.createByID(temp.id);
        if (tempSkill == null) return null;
        tempSkill.uId = temp.id;
        tempSkill.Level = (int)temp.level;
        _skillsList.Add(tempSkill);
        tempSkill.Slot = _skillsList.Count - 1;
        if (this.soldier != null)
            tempSkill.soldier = this.soldier;
        return tempSkill;
    }
    public List<Skill> GetUnLockSkill()
    {
        if(this._allSkills == null || this._allSkills.Count <= 0)
        {
            this._allSkills = new List<Skill>(15);
            List<HeroAttributeInfo> tempHeroList = ConfigManager.Instance.mHeroData.GetHeroAttributeList();
            if(tempHeroList == null)
                return null;
            for(int i = 0;i < tempHeroList.Count;++i)
            {
                HeroAttributeInfo tempHeroAtt = tempHeroList[i];
                if(tempHeroAtt == null||tempHeroAtt.PassiveSkill == null)
                    continue;
                foreach(KeyValuePair<uint, byte> tempSkillId in tempHeroAtt.PassiveSkill)
                {
                    Skill sk = Skill.createByID(tempSkillId.Key);
                    this._allSkills.Add(sk);
                }
            }
        }
        if (this._allSkills.Count == 0)
            return new List<Skill>();
        List<Skill> tempList = new List<Skill>(this._allSkills.Count + 1);
        for (int i = 0; i < this._allSkills.Count;++i )
        {
            Skill tempSkill = this._allSkills[i];
            Skill result = PlayerData.Instance._SkillsDepot._skillsList.Find((vSk) => { if (vSk == null)return false; return vSk.GetRootId() == tempSkill.Att.nId; });

            if (result == null)
            {
                tempSkill.islock = true;
                tempList.Add(tempSkill);
            }

        }
        return tempList;
    }
    public int GetReturnNum()
    {
        int num = 0;
        ReturnMaterialConfig att = ConfigManager.Instance.mReturnMaterialData;

        foreach (Skill sk in this._skillsList)
        {
            ReturnMaterialInfo info = att.FindByLevel(sk.Level);
            if (info == null) continue;
            foreach (KeyValuePair<uint, int> returnSkill in info.ReturnMaterial)
            {
                num += returnSkill.Value;
            }
        }
        return num;
    }
    public uint GetRootId(uint id)
    {
        List<uint> IdMark = new List<uint>(7);
        IdMark.Add(id);
        while(true)
        {
            uint tempId = IdMark[IdMark.Count - 1];
            SkillAttributeInfo temp = ConfigManager.Instance.mSkillAttData.FindById(tempId);
            if(temp == null)
                break;
            if(temp.prepositionId == 0)
            {
                IdMark.Add(temp.nId);
                break;
            }
            if (IdMark.Contains(temp.nId))
                break;
            IdMark.Add(temp.prepositionId);
        }
        if (IdMark.Count > 0)
            return IdMark[IdMark.Count - 1];
        return 0;
    }
    
    //测试技能是否能够强化
    public SkillCheck TextStrong(int slot, bool needCheckGoods = true)
    {
        if (slot >= _skillsList.Count) return SkillCheck.none;
        return _skillsList[slot].enableStrong(needCheckGoods);
    }
    //强化slot对应的技能
    public string Strong(int slot)
    {
        if (slot >= _skillsList.Count) return "Equipt";

        Skill temp = _skillsList[slot];
        UInt64 uid = 0;
        if(this.soldier != null)
        {
            uid = this.soldier.uId;
        }
        //switch (temp.enableStrong())
        //{
        //    case SkillCheck.none: break;
        //    case SkillCheck.Level:
        //        {
        //            if (this.soldier != null)
        //                uid = this.soldier.uId;
        //            SkillModule.Instance.SendUpgradeSkillReq(uid, temp.Att.nId, temp.Level);
        //        }
        //        break;
        //    case SkillCheck.Ok:
        //        {
        //            if (this.soldier != null)
        //                uid = this.soldier.uId;
        //            SkillModule.Instance.SendUpgradeSkillReq(uid, temp.Att.nId, temp.Level);
        //        }
        //        break;
        //}
        SkillModule.Instance.SendUpgradeSkillReq(uid, temp.Att.nId, temp.Level);
        return "Equipt";
    }
    public void AutoStrong(int slot)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.OneKeyEnchance, true))
            return;

        if (slot >= _skillsList.Count) return;

        Skill temp = _skillsList[slot];
        UInt64 uid = 0;
        if (this.soldier != null)
        {
            uid = this.soldier.uId;
        }
        SkillModule.Instance.SendAutoUpgradeSkillReq(uid,temp.Att.nId);
    }
    public void ActivateHalo(RoleAttribute role)
    {
        foreach(Skill skill in this._skillsList)
        {
            if(skill.Att.triggerType == 4)
            {
                for (int i = 0; i < skill.Att.effect.Count; ++i)
                {
                    if (skill.Att.effect[i].effectId == 0)
                        continue;
                    EffectManage tempManage = new EffectManage();

                    EffectInfo newInfo = (EffectInfo)skill.Att.effect[i].Clone();
                    newInfo.LevelAdd(skill.Level - skill.Att.initLevel);

                    tempManage.init(0, newInfo, skill.Level, role);
                    List<RoleAttribute> target = new List<RoleAttribute>(2);
                    target.Add(role);
                    tempManage.SetTarget(target);
                    tempManage.DoEffect();
                }
            }
        }

        foreach (Skill skill in this._skillsList)
        {
            if (skill.Att.triggerType == 3)
            {
                skill.UseSkill(role);
            }
        }
    }
    /// <summary>
    /// 随机触发技能
    /// </summary>
    /// <param name="role">技能释放对象</param>
    /// <param name="_Coeff_Trigger_Pro"></param>
    /// <returns></returns>
    public bool RandomSkill(RoleAttribute role)
    {
        foreach(Skill temp in this._skillsList)
        {
            if (temp.Att.triggerType == 0 || temp.Att.triggerType == 3 || temp.Att.trajectoryType == 4)
                continue;

            if (temp.RandomSkill(role, ((float)temp.Att.triggerProb / 100.0f) * role._Coeff_Trigger_Pro))
                return true;
        }
        return false;
    }
    public SkillsDepot()
    {
        _skillsList = new List<Skill>();
        Scheduler.Instance.AddUpdator(Update);
    }
    ~SkillsDepot()
    {
        Scheduler.Instance.RemoveUpdator(Update);
    }
    public bool Serialize(List<fogs.proto.msg.Skill> skillBag)
    {
        if (skillBag == null || skillBag.Count < 1)
            return false;

        this._skillsList.Clear();
        foreach(var temp in skillBag)
        {
            Skill findResult = this._skillsList.Find((sk) => { return sk.Att.nId == temp.id; });
            if (findResult != null)
                continue;
            oneAdd(temp);
        }
        needSort = true;
        return true;
    }
    public bool RefreshList(List<fogs.proto.msg.Skill> skillBag)
    {
        if (skillBag == null || skillBag.Count < 1)
            return false;

        foreach (var temp in skillBag)
        {
            Skill findResult = this._skillsList.Find((sk) => { return sk.Att.nId == temp.id; });
            if (findResult == null)
            {
                this.oneAdd(temp);
            }
            else
                findResult.Serialize(temp);
        }
        needSort = true;
        return true;
    }
    public void ReceiveUpgradeSkillResp(UpgradeSkillResp tData)
    {
        Skill sk = this._skillsList.Find((Skill s) => { return s.Att.nId == tData.old_skill_id; });
        if (sk == null)
            return;

        if (tData.old_skill_id != tData.skill_id)
        {
            this._evolve(sk,tData.skill_id,(int)tData.skill_level);
        }
        else
        {
            this._strong(sk,(int)tData.skill_level);
        }
    }
    private bool _evolve(Skill sk,uint evolveId,int level)
    {
        if (sk == null) return false;

        Skill temp = sk;
        int Slot = sk.Slot;
        Skill evoTemp = Skill.createByID(evolveId);
        if (evoTemp == null) return false;

        evoTemp.Slot = Slot;
        evoTemp.uId = temp.uId;
        evoTemp.soldier = sk.soldier;
        _skillsList[Slot] = null;
        temp = null;
        _skillsList[Slot] = evoTemp;
        if(level > evoTemp.Level)
        {
            this._strong(evoTemp,level);
            return true;
        }
        needSort = true;
        if (SkillsDepotEvent != null)
            SkillsDepotEvent(SkillChange.One, evoTemp.Slot, evoTemp.uId);
        return true;
    }
    private bool _strong(Skill temp,int level)
    {
        if (temp == null)
            return false;
        temp.Strong(level);
        if (SkillsDepotEvent != null)
            SkillsDepotEvent(SkillChange.One, temp.Slot, temp.uId);
        needSort = true;
        return true;
    }
    private bool _Add(uint Id)
    {
        Skill wp = Skill.createByID(Id);
        if (wp == null) return false;

        _skillsList.Add(wp);
        wp.Slot = _skillsList.Count - 1;
        if (this.soldier != null)
            wp.soldier = this.soldier;

        needSort = true;
        return true;
    }
    private bool _Add(Skill tp)
    {
        if (tp == null) return false;

        _skillsList.Add(tp);
        tp.Slot = _skillsList.Count - 1;
        if (this.soldier != null)
            tp.soldier = this.soldier;

        needSort = true;
        return true;
    }
    private bool _Delete(int Slot)
    {
        if (_skillsList == null) return false;
        if (Slot >= _skillsList.Count) return false;

        _skillsList[Slot] = null;


        needSort = true;
        needRemoveNull = true;
        return true;
    }
    public void Sort()
    {
        this._sort(false);
    }
    /// <summary>
    /// 1、首先按照技能是否解锁排序，已解锁的技能排序靠前，未解锁的技能排序靠后；
    ///2、根据技能可否强化（材料是否足够）排序，可以强化的排序靠前，不可强化的排序靠后；
    ///3、然后根据技能的强化等级排序，强化等级高的排序靠前，强化等级低的排序靠后；
    ///4、根据技能星级排序，星级越高排序越靠前（技能无星级则不考虑该条）
    ///5、根据技能品质排序，技能品质越高的排序越靠前；
    ///6、最后根据技能的ID排序，ID编号越小的排序越靠前；
    /// </summary>
    private void _sort(bool isRefresh = true)
    {
        if (!needSort && isRefresh)
            return;
        _skillsList.Sort
            (
            (left, right) =>
            {
                if (left.Att.triggerType == 2 && right.Att.triggerType != 2)
                    return -1;
                if (left.Att.triggerType != 2 && right.Att.triggerType == 2)
                    return -1;
                if (left.enableStrong() == SkillCheck.Ok && right.enableStrong() != SkillCheck.Ok)
                    return -1;
                if (left.enableStrong() != SkillCheck.Ok && right.enableStrong() == SkillCheck.Ok)
                    return 1;
                if (left.Level != right.Level)
                {
                    if (left.Level > right.Level)
                        return -1;
                    else
                        return 1;
                }
                if(left.Att.Quality != right.Att.Quality)
                {
                    if (left.Att.Quality > right.Att.Quality)
                        return -1;
                    else
                        return 1;
                }
                if (left.Att.nId != right.Att.nId)
                {
                    if (left.Att.nId < right.Att.nId)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            }
            );
        for (int i = 0; i < _skillsList.Count; ++i)
        {
            _skillsList[i].Slot = i;
        }
        needSort = false;
        if (SkillsDepotEvent != null && isRefresh)
            SkillsDepotEvent(SkillChange.All);
    }
    private void _RemoveNull()
    {
        if (!needRemoveNull) return;
        needRemoveNull = false;
        _skillsList.RemoveAll((temp) => { return temp == null; });
    }
}