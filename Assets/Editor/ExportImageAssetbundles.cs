using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// 打包图片
/// </summary>
class ExportImageAssetbundles : ExportCommand
{
    private const string PRE_STRING = "image";

    public override void Execute()
    {
        var texturePrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (var prefab in texturePrefabs)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            if (path.ToLower().Contains(EXPORTRESPATH + PRE_STRING + "/"))
            {
                this.BuildImageAssetbundle(path);
            }
        }
    }

    /*
     * Name: 打包图片
     * Desc: 替换以前的图片打包方式，同时支持打包压缩纹理、非压缩纹理、及原始图像数据
     * Date: 2015=04-20
     *  add by taiwei
     * 
     * Function:    BuildImageAssetbundle
     * Param:       prefabPath       [IN]      预制体路径
     * 
     * Return:      无
     */
    private void BuildImageAssetbundle(string prefabPath)
    {
        GameObject imagePrefab = AssetDatabase.LoadMainAssetAtPath(prefabPath) as GameObject;
        AloneImageHolder holder = imagePrefab.GetComponent<AloneImageHolder>();
        if (holder == null || holder.Texture == null)
        {
            Debug.LogError("Can not export " + prefabPath + " do you add the AloneImageHolder script and set the texture ?");
            return;
        }

        bool buildSuccess = false;
        string path = AssetbundleDetailPath(AssetBundleResType.Image) + PRE_STRING + "_" + imagePrefab.name + ".assetbundle";
        if (holder.SaveToBytes == false)
        {
            //创建普通图片资源包
            buildSuccess = BuildPipeline.BuildAssetBundle(imagePrefab, null, path, BuildOption, TargetPlatform);
        }
        else
        {
            //创建原始二进制图片包
            GameObject clone = new GameObject(imagePrefab.name);
            AloneImageHolder newHolder = clone.AddComponent<AloneImageHolder>();
            newHolder.name = holder.name;
            newHolder.SaveToBytes = holder.SaveToBytes;
            newHolder.Texture = null;
            newHolder.Desc_ImageName = holder.Texture.name;
            newHolder.Desc_ImageWidth = holder.Texture.width;
            newHolder.Desc_ImageHeight = holder.Texture.height;
            //读取原始图片文件二进制数据到宿主脚本中
            newHolder.ImageData = File.ReadAllBytes(AssetDatabase.GetAssetPath(holder.Texture));
            UnityEngine.Debug.Log("Bytes Count :" + newHolder.ImageData.Length);
            GameObject newPrefab = PrefabUtility.CreatePrefab("Assets/temp/" + imagePrefab.name + ".prefab", clone);
            GameObject.DestroyImmediate(clone);
            AssetDatabase.SaveAssets();
            buildSuccess = BuildPipeline.BuildAssetBundle(newPrefab, null, path, BuildOption, TargetPlatform);
            UnityEngine.Debug.Log("生成二进制");
            //删除临时预设体
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(newPrefab));
        }

        if (buildSuccess)
        {
            UnityEngine.Debug.Log("Export " + path + " Success !");
        }
        else
        {
            UnityEngine.Debug.Log("Export " + path + " Error !");
        }
    }
}