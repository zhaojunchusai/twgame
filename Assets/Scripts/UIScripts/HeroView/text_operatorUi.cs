using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class text_operatorUi : MonoBehaviour {

    // Use this for initialization
    GameObject role = null;
    GameObject woMan = null;
    List<string> tempList = null;
    List<string> weaponAtlas = null;
    List<string> weaponRegion = null;
    float time = 2.0f;
    float timeStart = 0.0f;
    int count = 0;
    void Start () {
        //role = GameObject.Find("Role_M");
        //role.AddComponent<TdSpine.MainSpine>().InitSkeletonAnimation();
        //woMan = GameObject.Find("Role_W");
        //woMan.AddComponent<TdSpine.MainSpine>().InitSkeletonAnimation();

        Test tt = new Test();
        tt.MainTest((Object ob) => 
        {
            GameObject go = (GameObject)ob;
            if (go != null)
                role = go;
            GameObject fa = GameObject.Find("Camera");
            if(fa)
            {
               role = CommonFunction.InstantiateObject(go, fa.transform);
               role.gameObject.AddComponent<TdSpine.MainSpine>().InitSkeletonAnimation();
            }
        });

        tempList = new List<string>(7);
        tempList.Add(GlobalConst.ANIMATION_NAME_DEATH);
        tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
        tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
        tempList.Add(GlobalConst.ANIMATION_NAME_IDLE);
        tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_BASIC);
        tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);
        weaponAtlas = new List<string>();
        weaponAtlas.Add("aloneres_wuqi_1.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_2.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_3.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_4.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_5.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_6.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_7.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_8.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_9.assetbundle");
        weaponAtlas.Add("aloneres_wuqi_10.assetbundle");

        weaponRegion = new List<string>();
        weaponRegion.Add("wuqi_1_fengchi");
        weaponRegion.Add("wuqi_2_zizai");
        weaponRegion.Add("wuqi_3_moli");
        weaponRegion.Add("wuqi_4_qinglong");
        weaponRegion.Add("wuqi_5_biri");
        weaponRegion.Add("wuqi_6_bixue");
        weaponRegion.Add("wuqi_7_erlang");
        weaponRegion.Add("wuqi_8_zhanjin");
        weaponRegion.Add("wuqi_9_zhangba");
        weaponRegion.Add("wuqi_10_fangtian");

        timeStart = Time.time;
	}
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 20, 50, 30), "Repleace"))
        {
            if (count >= this.weaponAtlas.Count)
                count = 0;
            role.GetComponent<TdSpine.MainSpine>().RepleaceEquip(this.weaponAtlas[count],this.weaponRegion[count]);
            //woMan.GetComponent<TdSpine.MainSpine>().RepleaceEquip(this.weaponAtlas[count], this.weaponRegion[count]);
            role.GetComponent<TdSpine.MainSpine>().RepleaceHorse("role_21004001.assetbundle");
            //woMan.GetComponent<TdSpine.MainSpine>().RepleaceHorse("role_21004003.assetbundle");
            ++count;
        }
        if (GUI.Button(new Rect(500, 20, 50, 30), "Repleace"))
        {
            if (count >= this.tempList.Count)
                count = 0;

            role.GetComponent<TdSpine.MainSpine>().pushAnimation(tempList[count], true, 0);
            role.GetComponent<TdSpine.MainSpine>().FadeColor(Color.red);
            //woMan.GetComponent<TdSpine.MainSpine>().pushAnimation(tempList[count], true, 0);
            //role.GetComponent<TdSpine.MainSpine>().FadeOut(1.0f);
            ++count;
        }
    }
	// Update is called once per frame
	void Update () {
        Assets.Script.Common.Scheduler.Instance.Update();
       // UpdateTimeTool.Instance.Update();
       if(Time.time - timeStart > time)
       {
          
       }
	}



}

public class Test 
{
    public delegate void OnLoad(Object go);

    public void MainTest(OnLoad function) 
    {
        string url = "file:///" + Application.dataPath + "/Test/role_heromale.assetbundle";
        Main.Instance.StartCoroutine(WWWLoad(url,function));      
    }
    public IEnumerator WWWLoad(string url,OnLoad function) 
    {
        WWW www = new WWW(url);
        yield return www;
        Object asset = www.assetBundle.mainAsset;
        if(asset != null)
            function(asset);
    }
}