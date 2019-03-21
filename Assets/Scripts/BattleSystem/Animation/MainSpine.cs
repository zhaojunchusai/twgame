using System;
using System.Collections.Generic;
using UnityEngine;
using Spine;

namespace TdSpine
{
    public class MainSpine : SpineBase
    {
        public int id = -1;

        //public AtlasAsset atlasAsset;
        SkeletonRenderer skeletonRenderer;
        public bool RepleaceHoursing = false;
        public bool IsInit = false;
        public List<string> hoursAltlas = new List<string>();

        public GameObject EquipEffect;
        public int EquipEffectType = 1;
        public Color EquipEffectColor;
        public bool isPet = false;
        public void SetEquipEffectType(int vType)
        {
            if (EquipEffect == null)
                return;
            this.EquipEffectType = vType;
            SpineBase tmp = EquipEffect.GetComponent<SpineBase>();
            if(tmp != null)
            {
                switch (vType)
                {
                    case 1:
                        tmp.ReSetColor();
                        break;
                    case 2:
                        tmp.SetColorNoSave(GlobColor.EquipEffectType2);
                        break;
                    default:
                        tmp.SetColorNoSave(GlobColor.EquipEffectType2);
                        break;
                }
            }
        }
        /// <summary>
        /// 换N件装备
        /// </summary>
        /// <param name="depot"></param>
        /// <returns></returns>
        public bool RepleaceEquipment(ArtifactedDepot depot,string petResource = "",bool isPet = false)
        {
            this.isPet = isPet;
            if (depot == null)
                return false;
            if (depot._EquiptList.Count < 8)
                return false;
            this.IsInit = true;

            this.AddWeaponEffect("0");

            if ((!isPet && depot._EquiptList[7] == null) || (isPet && string.IsNullOrEmpty(petResource)))
                this.RepleaceHorse("role_21004001.assetbundle");

            bool isHadWeapon = false;
            for (int i = 0; i < depot._EquiptList.Count; ++i)
            {
                Weapon tmp = depot._EquiptList[i];
                if (tmp == null)
                    continue;
                if (tmp.Att.type == 0)
                    isHadWeapon = transform;
            }
            if (!isHadWeapon)
                this.RepleaceEquipment(ConfigManager.Instance.mEquipData.GetEquipAttributeList()[0].id);

            List<Weapon> tmpList = new List<Weapon>();
            foreach (Weapon temp in depot._EquiptList)
            {
                if (temp == null)
                    continue;
                if (temp.Att.type == 4)
                    this.RepleaceEquipment(temp.Att.id);
                if (temp.Att.type != 0)
                    continue;

                tmpList.Add(temp);
            }
            if (tmpList.Count > 0)
                this.RepleaceEquipment(tmpList[tmpList.Count - 1].Att.id);
            if (isPet && !string.IsNullOrEmpty(petResource))
                this.RepleaceHorse(petResource);

            return true;
        }
        /// <summary>
        /// 换一件装备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RepleaceEquipment(uint id)
        {
            EquipAttributeInfo equipInfo = ConfigManager.Instance.mEquipData.FindById(id);

            if (equipInfo == null) return false;

            if (equipInfo.type == 4 && !this.isPet)
            {
                
                return this._repleaceHourse(equipInfo);
            }

            if (equipInfo.altlasPath == "0")
                return false;
            if (equipInfo.type != 0)
                return false;
            ResourceLoadManager.Instance.LoadAssetAlone
                (
                equipInfo.altlasPath,
                (gb) =>
                {
                    GameObject weapon = (gb) as GameObject;
                    AloneAtlasHolder holder = weapon.GetComponent<AloneAtlasHolder>();
                    AtlasAsset resourceAtlas = holder.atlas;

                    if (resourceAtlas == null) return;
                    if (RuntimePlatform.IPhonePlayer == Application.platform || RuntimePlatform.WindowsEditor == Application.platform || RuntimePlatform.OSXEditor == Application.platform)
                    {
                        for (int j = 0; j < resourceAtlas.materials.Length; j++)
                        {
                            string name = resourceAtlas.materials[j].shader.name;
                            Shader shader = Shader.Find(name);
                            resourceAtlas.materials[j].shader = shader;
                        }
                    }
                    EquipAttributeInfo myInfo = equipInfo;

                    AtlasAttachmentLoader loader = new AtlasAttachmentLoader(resourceAtlas.GetAtlas());

                    float scaleMultiplier = skeletonRenderer.skeletonDataAsset.scale;

                    RepleaceAttachment(loader, scaleMultiplier, "weapon1", myInfo.region);
                    RepleaceAttachment(loader, scaleMultiplier, "weapon1_death", myInfo.region);

                    this.AddWeaponEffect(myInfo.EffectName);
                    return;
                },
                null);
            return true;
            //Atlas文件加载
        }
        private bool _repleaceHourse(EquipAttributeInfo equipInfo)
        {
            if (equipInfo.type != 4) return false;

            this.RepleaceHorse(equipInfo.altlasPath);
            return true;
        }
        //测试代码
        public void RepleaceEquip(string AtlasPath, string region)
        {

            ResourceLoadManager.Instance.LoadAssetAlone
                (
                AtlasPath,
                (gb) =>
                {
                    //AtlasAsset resourceAtlas = gb as AtlasAsset;
                    GameObject weapon = (gb) as GameObject;

                    AloneAtlasHolder holder = weapon.GetComponent<AloneAtlasHolder>();
                    AtlasAsset resourceAtlas = holder.atlas;

                    if (resourceAtlas == null) return;
                    if (RuntimePlatform.IPhonePlayer == Application.platform || RuntimePlatform.WindowsEditor == Application.platform || RuntimePlatform.OSXEditor == Application.platform)
                    {
                        for (int j = 0; j < resourceAtlas.materials.Length; j++)
                        {
                            string name = resourceAtlas.materials[j].shader.name;
                            Shader shader = Shader.Find(name);
                            resourceAtlas.materials[j].shader = shader;
                        }
                    }

                    AtlasAttachmentLoader loader = new AtlasAttachmentLoader(resourceAtlas.GetAtlas());

                    float scaleMultiplier = skeletonRenderer.skeletonDataAsset.scale;

                    RepleaceAttachment(loader, scaleMultiplier, "weapon1", region);
                    RepleaceAttachment(loader, scaleMultiplier, "weapon1_death", region);

                    return;
                },
                null);
        }
        public void RepleaceHorse(string AtlasPath)
        {
            this.hoursAltlas.Add(AtlasPath);
            if (!RepleaceHoursing)
                this.repleaceHorse();
        }
        public override bool setSortingOrder(int _order)
        {
            if (this.EquipEffect != null)
            {
                SpineBase tmp = this.EquipEffect.GetComponent<SpineBase>();
                if (tmp != null)
                    tmp.setSortingOrder(_order + 2);
            }
            return base.setSortingOrder(_order);
        }
        public override int GetMaxSort()
        {
            if (this.EquipEffect != null)
                return base.GetMaxSort() + 1;
            else
                return base.GetMaxSort();
        }
        private void repleaceHorse()
        {
            if (hoursAltlas.Count <= 0)
            {
                this.RepleaceHoursing = false;
                return;
            }
            ResourceLoadManager.Instance.LoadCharacter(hoursAltlas[hoursAltlas.Count - 1], ResourceLoadType.AssetBundle,
             (Gb) =>
             {
                 //Transform child = this.transform.FindChild("hourse");

                 List<GameObject> child = new List<GameObject>(this.transform.childCount);
                 for (int i = 0; i < this.transform.childCount; ++i)
                 {
                     if (this.transform.GetChild(i).name == "hourse")
                         child.Add(this.transform.GetChild(i).gameObject);
                 }
                 GameObject go = CommonFunction.InstantiateObject(Gb, this.transform);
                 if (go != null)
                 {
                     if (child != null)
                     {
                         for (int i = 0; i < child.Count;++i )
                         {
                             GameObject.Destroy(child[i]);
                         }
                         //UpdateTimeTool.Instance.DelayDelGameObject(child);
                     }
                     go.layer = this.gameObject.layer;
                     go.name = "hourse";
                     this.hourseAnimation = go.GetComponent<SkeletonAnimation>();
                     this.hourseAnimation.state.SetAnimation(0, GlobalConst.ANIMATION_NAME_IDLE, true);

                     //this.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 1);
                     this.hourseAnimation.renderer.sortingOrder = this.skeletonAnimation.renderer.sortingOrder - 1;
                 }
                 this.RepleaceHoursing = false;
             });
            this.hoursAltlas.Clear();
        }
        private void RepleaceAttachment(AtlasAttachmentLoader loader, float _scale, string _slot, string _region)
        {
            var regionAttachment = loader.NewRegionAttachment(null, _region, _region);
            regionAttachment.Width = regionAttachment.RegionOriginalWidth * _scale;
            regionAttachment.Height = regionAttachment.RegionOriginalHeight * _scale;
            var slot = skeletonRenderer.skeleton.FindSlot(_slot);
            regionAttachment.SetColor(new Color(1, 1, 1, 1));
            regionAttachment.UpdateOffset();
            if (slot != null)
                slot.Attachment = regionAttachment;
        }
        protected override void Awake()
        {
            base.Awake();
            //skeletonRenderer = GetComponent<SkeletonRenderer>();
        }
        protected override void Start()
        {
            base.Start();
            this.Restore();
        }
        public override void InitSkeletonAnimation()
        {
            base.InitSkeletonAnimation();
            skeletonRenderer = GetComponent<SkeletonRenderer>();
        }
        private void Restore()
        {
            if (IsInit)
                return;
            RoleBase role = this.GetComponent<RoleBase>();
            if (role != null)
            {
                if (role.Get_RoleType == ERoleType.ertHero)
                {
                    this.RepleaceHorse("role_21004001.assetbundle");
                    this.RepleaceEquip("aloneres_wuqi_10.assetbundle", "wuqi_10_fangtian");
                }
            }
            this.IsInit = true;
        }
        private void AddWeaponEffect(string EffectName)
        {
            for (int i = 0; i < this.transform.childCount;++i )
            {
                Transform delet = this.transform.GetChild(i);
                if(delet != null && delet.name.Equals("EquipEffectName"))
                    GameObject.Destroy(delet.gameObject);
            }

            if (string.IsNullOrEmpty(EffectName) || EffectName == "0")
                return;
            ResourceLoadManager.Instance.LoadEffect(EffectName, (go) =>
            {
                GameObject temp = CommonFunction.InstantiateObject(go, this.gameObject.transform);
                temp.SetActive(true);
                this.EquipEffect = temp;

                temp.name = "EquipEffectName";
                SpineBase tempSpine = temp.GetComponent<SpineBase>();
                if (tempSpine == null)
                    tempSpine = temp.AddComponent<SpineBase>();

                tempSpine.InitSkeletonAnimation();

                tempSpine.pushAnimation("animation", true, 1);
                tempSpine.setSortingOrder(this.skeletonAnimation.renderer.sortingOrder + 1);

                BoneFollower follower = temp.GetComponent<BoneFollower>();
                if (follower == null)
                    follower = temp.AddComponent<BoneFollower>();

                follower.skeletonRenderer = this.skeletonRenderer;

                Bone followBone = this.skeletonRenderer.skeleton.FindBone("bone_weapon1");
                follower.bone = followBone;
                follower.boneName = "bone_weapon1";
                follower.followBoneRotation = true;
                follower.followZPosition = true;
                SetEquipEffectType(this.EquipEffectType);
            });
        }


        /// <summary>
        /// 读档存档
        /// </summary>
        /// <returns></returns>
        public bool Seriliztion()
        {
            return true;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool initialize()
        {
            return true;
        }

        public bool repleaceEquipment()
        {
            return true;
        }
        public override void UnInit()
        {
            base.UnInit();
            if (this.EquipEffect != null)
                GameObject.Destroy(this.EquipEffect);
            this.EquipEffect = null;
            this.skeletonRenderer = null;
            if (this.hoursAltlas != null)
                this.hoursAltlas.Clear();
        }
        protected override void FadeOutUpDate()
        {
            if (this.EquipEffect != null)
            {

                SkeletonAnimation tmpAnimation = this.EquipEffect.GetComponent<SkeletonAnimation>();
                if (tmpAnimation != null)
                {
                    foreach (Slot slot in tmpAnimation.skeleton.Slots)
                    {
                        if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                        {
                            if (slot.a <= 0)
                                continue;
                        }
                        slot.a -= ((float)this.arf / 10);
                    }
                }
            }
            base.FadeOutUpDate();
        }
    }
};