using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class ExportBundles
{
    // [MenuItem("PXTools/Create Assetbundles/CurrentTarget")]
    public static void ExecuteCurrent()
    {
        Execute();
        AssetDatabase.Refresh();
    }
    private static void Execute()
    {
        Dictionary<string, ExportCommand> dictAsset = new Dictionary<string, ExportCommand>();
        foreach (Object asset in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (asset.name.Contains("@")) continue;

            //角色//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/Role/"))
            {
                if (!dictAsset.ContainsKey("Role"))
                {
                    dictAsset.Add("Role", new ExportCharacterAssetbundles());
                }
                continue;
            }
            //特效//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/Effect/"))
            {
                if (!dictAsset.ContainsKey("Effect"))
                {
                    dictAsset.Add("Effect", new ExportEffectAssetbundles());
                }
                continue;
            }
            //图片//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/Image/"))
            {
                if (!dictAsset.ContainsKey("Image"))
                {
                    dictAsset.Add("Image", new ExportImageAssetbundles());
                }
                continue;
            }
            //图集//
            //if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/Atlas/") || AssetDatabase.GetAssetPath(asset).Contains("AssetRes/Atlas"))
            //{
                if (!dictAsset.ContainsKey("Atlas"))
                {
                    dictAsset.Add("Atlas", new ExportAloneAtlasAssetbundles());
                }
            //    continue;
            //}
            //字体//
            if (AssetDatabase.GetAssetPath(asset).Contains("AssetRes/Atlas/Font"))
            {
                if (!dictAsset.ContainsKey("Fonts"))
                {
                    dictAsset.Add("Fonts", new ExportAloneFontAssetbundles());
                }
                continue;
            }
            //配置表//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/Config/"))
            {
                if (!dictAsset.ContainsKey("Config"))
                {
                    dictAsset.Add("Config", new ExportConfigAssetbundles());
                }
                continue;
            }
            //独立资源//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/AloneRes/"))
            {
                if (!dictAsset.ContainsKey("AloneRes"))
                {
                    dictAsset.Add("AloneRes", new ExportAloneAssetbundles());
                }
                continue;
            }
            //音效//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/Sound/"))
            {
                if (!dictAsset.ContainsKey("Sound"))
                {
                    dictAsset.Add("Sound", new CreateSoundAssetbundles());
                }
                continue;
            }
            //界面//
            if (AssetDatabase.GetAssetPath(asset).Contains("ExportRes/View/"))
            {
                if (!dictAsset.ContainsKey("View"))
                {
                    dictAsset.Add("View", new CreateNGUIAssetbundles());
                }
                continue;
            }
        }

        //初始化导出的资源文件夹
        ExportCommand.InitAssetbundleDirectory();

        foreach (ExportCommand command in dictAsset.Values)
        {
            command.Execute();
        }
    }



}
