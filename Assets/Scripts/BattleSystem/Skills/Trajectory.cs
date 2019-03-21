using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
/// <summary>
/// 弹道类
/// </summary>
public class Traectory
{
    protected int id = 0;
    protected float a = -1;
    protected double b;
    public Vector3 destPos;
    protected Vector3 startPos;
    public RoleAttribute dest;
    protected GameObject gb;
    protected Vector3 lastPosition;
    public float speed = 20.0f;
    public EFightCamp _roleCamp;
    public delegate void Collider(int id);
    public event Collider ColliderEvent;
    /// <summary>
    /// 调用函数，会直接产生弹道
    /// </summary>
    /// <param name="startPos">发起者世界坐标</param>
    /// <param name="dest">目标</param>
    /// <param name="spcecil">特效名字</param>
    /// <param name="bone">特效挂点</param>
    /// <param name="father">父类</param>
    /// <param name="id">表示ID</param>
    public virtual void Instance(Vector3 startPos, RoleAttribute dest, string spcecil, int bone = 2,Vector3 destPos = new Vector3(), Transform father = null, string musicFilt = "", int id = 0)
    {
        if (father == null || dest == null)
        {
            father = SceneManager.Instance.Get_CurScene.transOther;
        }
        if (spcecil.Equals("0"))
            return;
        if (dest == null && destPos == Vector3.zero)
            return;

        Vector3 tempDestPos = destPos;
        if (spcecil == null) return;
        this.id = 0;
        this.dest = dest;
        if (dest != null)
            this.destPos = SkillTool.GetWorldPosition(dest, bone);
        else
            this.destPos = tempDestPos;
            
        if (dest != null)
            this._roleCamp = dest.Get_RoleCamp;
        else
            this._roleCamp = EFightCamp.efcEnemy;
        this.startPos = startPos;
        if (dest != null)
            this.a = -0.8f / Math.Abs(startPos.x - dest.transform.position.x);
        else
            this.a = -0.8f / Math.Abs(startPos.x - tempDestPos.x);
        EffectObjectCache.Instance.LoadGameObject(spcecil,
            (GameObject gb) => 
            {
                if (father == null) return;
                Scheduler.Instance.AddTimer(0.034f,true,this.Update);

                this.gb = CommonFunction.SetParent(gb, father);
                this.gb.transform.localScale = new Vector3(1, 1, 1);
                this.SetPostion(this.gb);
                TdSpine.SpineBase spine = this.gb.GetComponent<TdSpine.SpineBase>();

                if(spine == null)
                    spine = this.gb.AddComponent<TdSpine.SpineBase>();
                spine.InitSkeletonAnimation();
                //spine.skeletonAnimation.Reset();
                spine.pushAnimation("animation",true,1);
                spine.setSortingOrder(50);

                if(musicFilt != "0" && musicFilt != "")
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(musicFilt, this.gb.transform));
                }
                SkillManage.Instance.PushSpine(spine);
            });
    }
    public virtual void SetPostion(GameObject gb)
    {
        gb.transform.position = this.startPos;
    }
    public virtual void SetSpeed(float speed)
    {
        if (speed == 0)
            return;
        this.speed = speed;
    }
    public virtual void PosXAdd()
    {
        
    }
    public virtual void PosYAdd()
    {
        
    }
    public virtual void RotationAdd()
    {
        Transform temp = this.gb.transform;
        Vector3 pos = temp.position;
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        float tmpScreenProportion_Y = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_Y;
        float x = pos.x - lastPosition.x;
        float y = pos.y - lastPosition.y;
        if (x == 0 && y == 0) return;
        double angle = 0.0;
        if(x == 0)
        {
            if(y > 0.000001f)
                angle = 90;
            else
                angle = -90;
        }
        else
        {
            angle = Math.Atan(Math.Abs(y) / Math.Abs(x));
            angle = angle * 180 / 3.141592653589793;
        }

        if (x > 0.000001f && y < -0.000001f)
        {
            angle = -angle;
        }
        if (x < -0.000001f && y > 0.000001f)
        {
            angle = 180 - angle;
        }
        if (x < -0.000001f && y < -0.000001f)
        {
            angle = 180 + angle;
        }
        int angleX = 0;
        if(angle > 90 && angle < 270)
        {
            angleX = 180;
        }
        else
        {
            if(angle < -90 && angle > 270)
            {
                angleX = 180;
            }
            else
            {
                angleX = 0;
            }
        }
        temp.rotation = Quaternion.Euler(angleX, 0,0);

        temp.rotation = Quaternion.Euler(0, 0, (float)angle);
    }
    public virtual void _isPosChange()
    {
        if (this.dest == null) return;

        Vector3 temp = SkillTool.GetWorldPosition(dest, 2);

        if (this.destPos != temp)
            this.destPos = temp;
    }
    public virtual bool _isCollider()
    {
        return true;
    }
    public virtual void _onCollider()
    {
        Scheduler.Instance.RemoveTimer(this.Update);
        //UpdateTimeTool.Instance.RemoveTimer(this.Update);
        EffectObjectCache.Instance.FreeObject(this.gb);
        this.gb = null;
        if (ColliderEvent != null)
            ColliderEvent(this.id);
    }
    ~Traectory()
    {
        UpdateTimeTool.Instance.DelayDelGameObject(this.gb);
    }
    public virtual void _Calculate()
    {

    }
    private void Update()
    {
        if (this.gb == null)
            return;
        if (this._isCollider())
        {
            this._onCollider();
            return;
        }

        this._isPosChange();
        this._Calculate();

        this.PosXAdd();
        this.PosYAdd();
        this.RotationAdd();
    }
}
/// <summary>
/// 抛物线
/// </summary>
public class Parabola : Traectory
{
    public override void PosXAdd()
    {
        lastPosition = this.gb.transform.position;
        Vector3 pos = this.gb.transform.position;
        Vector3 temp = new Vector3(pos.x, pos.y, pos.z);
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        if(destPos.x > pos.x)
        {
            temp.x = (pos.x * tmpScreenProportion_X + speed *  SkillManage.Instance.currTimeScale) / tmpScreenProportion_X;
        }
        else
        {
            temp.x = (pos.x * tmpScreenProportion_X - speed *  SkillManage.Instance.currTimeScale) / tmpScreenProportion_X;
        }
        this.gb.transform.position = temp;
    }
    public override bool _isCollider()
    {
        if (this.gb == null) return true;
        if (SceneManager.Instance.Get_CurScene == null)
            return true;
        float x = Math.Abs(this.gb.transform.position.x - destPos.x);
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        x *= tmpScreenProportion_X;
        return x <= (speed * SkillManage.Instance.currTimeScale / 2);
    }
    public override void PosYAdd()
    {
        Vector3 pos = this.gb.transform.position;

        float x = pos.x - startPos.x;

        float y = (float)(a * x * (x - b)) + startPos.y;
        this.gb.transform.position = new Vector3(pos.x, y, pos.z);
    }

    public override void _Calculate()
    {
        float x = destPos.x - startPos.x;
        float y = destPos.y - startPos.y;
        b = (Math.Pow(x, 2) * a - y) / (a * x);
    }
}
/// <summary>
/// 斜线
/// </summary>
public class PerpenDicular : Traectory
{

    public override void PosXAdd()
    {
        lastPosition = this.gb.transform.position;
        Vector3 pos = this.gb.transform.position;
        Vector3 temp = new Vector3(pos.x, pos.y, pos.z);
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        if (destPos.x > pos.x)
        {
            temp.x = (pos.x * tmpScreenProportion_X + speed * SkillManage.Instance.currTimeScale) / tmpScreenProportion_X;
        }
        else
        {
            temp.x = (pos.x * tmpScreenProportion_X - speed * SkillManage.Instance.currTimeScale) / tmpScreenProportion_X;
        }
        this.gb.transform.position = temp;
    }
    public override void PosYAdd()
    {
        Vector3 pos = this.gb.transform.position;

        float x = pos.x - startPos.x;
        float y = x * a + startPos.y;
        this.gb.transform.position = new Vector3(pos.x, y, pos.z);
    }
    public override void RotationAdd()
    {
        Transform temp = this.gb.transform;
        Vector3 pos = temp.position;
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        float tmpScreenProportion_Y = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_Y;
        float x = pos.x - lastPosition.x;
        float y = pos.y - lastPosition.y;
        if (x == 0 && y == 0) return;
        double angle = 0.0;
        if (x != 0)
        {
            if (x > 0.000001f)
                angle = 0;
            else
                angle = -180;
            temp.rotation = Quaternion.Euler(0, (float)angle, 0);
        }
        
    }

    public override bool _isCollider()
    {
        if (this.gb == null) return true;
        if (SceneManager.Instance.Get_CurScene == null)
            return true;
        float x = Math.Abs(this.gb.transform.position.x - destPos.x);
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        x *= tmpScreenProportion_X;
        return x <= (speed * SkillManage.Instance.currTimeScale / 2);
    }
    public override void _Calculate()
    {
        float x = destPos.x - startPos.x;
        float y = destPos.y - startPos.y;
        a = (y) / (x);
    }
}
/// <summary>
/// 自由落体
/// </summary>
public class FreelyFalling : Traectory
{
    private float Length = 300.0f;
    public override void SetPostion(GameObject gb)
    {
        float tmpScreenProportion_Y = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_Y;

        this.startPos = new Vector3(destPos.x, destPos.y + (Length / tmpScreenProportion_Y), destPos.z);
        gb.transform.position = startPos;
    }
    public override void PosXAdd()
    {
        Vector3 pos = this.gb.transform.position;

        float y = pos.y - startPos.y;
        float x = y / a + startPos.x;
        this.gb.transform.position = new Vector3(x, pos.y, pos.z);
    }
    public override void PosYAdd()
    {
        lastPosition = this.gb.transform.position;
        Vector3 pos = this.gb.transform.position;
        Vector3 temp = new Vector3(pos.x, pos.y, pos.z);
        float tmpScreenProportion_Y = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_Y;

        temp.y = (pos.y * tmpScreenProportion_Y - speed * SkillManage.Instance.currTimeScale) / tmpScreenProportion_Y;
       
        this.gb.transform.position = temp;
    }
    public override bool _isCollider()
    {
        if (this.gb == null) return true;
        if (SceneManager.Instance.Get_CurScene == null)
            return true;
        float y = Math.Abs(this.gb.transform.position.y - destPos.y);
        float tmpScreenProportion_Y = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_Y;
        y *= tmpScreenProportion_Y;
        return y <= (speed * SkillManage.Instance.currTimeScale / 2);
    }
    public override void _Calculate()
    {
        float x = destPos.x - startPos.x;
        float y = destPos.y - startPos.y;
        a = (y) / (x);
    }
}
/// <summary>
/// 直线弹道
/// </summary>
public class TransverseLine : Traectory
{
    public override void PosXAdd()
    {
        lastPosition = this.gb.transform.position;
        Vector3 pos = this.gb.transform.position;
        Vector3 temp = new Vector3(pos.x, pos.y, pos.z);
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        if (destPos.x > pos.x)
        {
            temp.x = (pos.x * tmpScreenProportion_X + speed * SkillManage.Instance.currTimeScale) / tmpScreenProportion_X;
        }
        else
        {
            temp.x = (pos.x * tmpScreenProportion_X - speed * SkillManage.Instance.currTimeScale) / tmpScreenProportion_X;
        }
        this.gb.transform.position = temp;
    }
    public override void RotationAdd()
    {
        Transform temp = this.gb.transform;
        Vector3 pos = temp.position;
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        float tmpScreenProportion_Y = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_Y;
        float x = pos.x - lastPosition.x;
        float y = pos.y - lastPosition.y;
        if (x == 0 && y == 0) return;
        double angle = 0.0;
        if (x != 0)
        {
            if (x > 0.000001f)
                angle = 0;
            else
                angle = -180;
            temp.rotation = Quaternion.Euler(0, (float)angle, 0);
        }

    }

    public override bool _isCollider()
    {
        if (this.gb == null) return true;
        if (SceneManager.Instance.Get_CurScene == null)
            return true;
        float x = Math.Abs(this.gb.transform.position.x - destPos.x);
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        x *= tmpScreenProportion_X;
        return x <= (speed * SkillManage.Instance.currTimeScale / 2);
    }

}
public class EffectManage
{
    public Effects effect;
    protected int DelayTime;
    protected Soldier soldier = null;
    protected PlayerData player = null;
    protected List<RoleAttribute> role = null;
    protected RoleAttribute resource = null;
    public void init(int DelayTime, EffectInfo effectInfo,int lv,RoleAttribute role = null)
    {
        if (effectInfo == null) return;
        this.DelayTime = DelayTime;
        this.effect = SkillTool.EffectCreate(effectInfo,lv, role);
        this.role = new List<RoleAttribute>();
        this.resource = role;
    }
    public bool IsEmpty()
    {
        return true;
    }
    public void SetTarget(Soldier soldier)
    {

        this.soldier = soldier;
    }
    public void SetTarget(PlayerData player)
    {

        this.player = player;
    }
    public void SetTarget(List<RoleAttribute> role)
    {
        if (role == null) return;
        this.role.Clear();
        this.role.Capacity = role.Count + 1;
        this.role.AddRange(role);
    }
    public void DoEffect()
    {
        float roleAttributeScale = 1.0f;
        if (this.resource != null)
        {
            roleAttributeScale = this.resource.Get_SkillTimeScale;
        }
        if (this.DelayTime == 0)
            this.OnTimeDelay();
        else
            UpdateTimeTool.Instance.AddTimer(((float)this.DelayTime / 1000.0f) / roleAttributeScale, false, this.OnTimeDelay);
    }
    private void OnTimeDelay()
    {
        if (this.player != null)
            this.effect.DoEffect(this.player);
        else if (this.soldier != null)
            this.effect.DoEffect(this.soldier);
        else if(this.role != null)
        {
            this.effect.DoEffect(this.role);
        }
    }
}