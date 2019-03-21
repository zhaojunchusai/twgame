using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using Assets.Script.Common;
public struct UpdateFileData
{
    public string FilePath;
    public long FileSize;
    public string FileName;
    public string FileMD5;
}

public struct VersionData
{
    public float ApkVersion;
    public float ResVersion;
    public float CodeVersion;
    public string ApkUrl;
    public string ResUrl;
    public string CodeUrl;
    public long ApkSize;
    public long CodeSize;
}

public struct FilePathData
{
    public string UrlPath;
    public string LocalPath;
    public long FileSize;
}

public enum UpateErrorType : byte
{
    NotFindVersionXml = 1,
    VersionXmlError = 2,
    VersionXmlWriteError = 3,
    VersionXmlReadError = 4,
    NotFindResXml = 5,
    ResXmlWriteError = 6,
    DownCodeError = 7,

}

public class UpdateVersionManager : MonoSingleton<UpdateVersionManager>
{
    private string VersionPath = "";
    private byte[] _bytes;
    private VersionData _serverVersionData;
    private VersionData _localVersionData;
    private UpdateFileState _state;
    private long _downResSize;
    private List<FilePathData> _updateResData = new List<FilePathData>();
    private Dictionary<string, string> _resPath = new Dictionary<string, string>();
    private List<string> _localFileList = new List<string>();
    private List<UpdateFileData> _serverFileList = new List<UpdateFileData>();
    public delegate void UpdateVersionSchedule(System.Object obj);
    public delegate void UpdateVersionScheduleSize(long obj, long obj2);
    public delegate void UpdateVersionScheduleDownWarn(System.Object obj, long obj2);
    public UpdateVersionSchedule UpdateCheckState;
    public UpdateVersionSchedule CheckFailure;
    public UpdateVersionSchedule UpdateResValue;
    public UpdateVersionScheduleSize UpdateResSizeValue;
    public UpdateVersionSchedule UpdateResError;
    public UpdateVersionScheduleDownWarn UpdateDownResWarn;
    public void Initialize()
    {
        _downResSize = 0;
        _updateResData.Clear();
        _resPath.Clear();
        _localFileList.Clear();
        _serverFileList.Clear();
        _state = UpdateFileState.None;
        ReadAPKVersion();
    }

    public void StartCheckVersion()
    {
        SetChangeState(UpdateFileState.CheckVersion);
    }

    public void ReadAPKVersion()
    {
        TextAsset tex = UnityEngine.Resources.Load(ResPath.APKVERSIONXML) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(tex.text);
        XmlNode nodelist = xmlDoc.SelectSingleNode("Data");
        _localVersionData.ApkVersion = float.Parse(nodelist.InnerText);
    }

    private void DoneOperateForStatus()
    {
        switch (_state)
        {
            case UpdateFileState.CheckVersion:
                if (UpdateCheckState != null)
                    UpdateCheckState(UpdateFileState.CheckVersion);
                StartCoroutine(CheckVersion());
                break;
            case UpdateFileState.ComparedVersion:
                CompareVersion();
                break;
            case UpdateFileState.GetUpdateList:
                if (UpdateCheckState != null)
                    UpdateCheckState(UpdateFileState.GetUpdateList);
                StartCoroutine(DownLoadResList());
                break;
            case UpdateFileState.ReadResPath:
                StartCoroutine(ReadLocalResPath());
                break;
            case UpdateFileState.DownCodeWarn:
                DownLoadCode();
                break;
            case UpdateFileState.DownResWarn:
                DownLoadRes();
                break;
            case UpdateFileState.DownApkWarn:
                DownLoadAPK();
                break;
            case UpdateFileState.UpdateResource:
                SetDownLoadResManager();
                if (UpdateCheckState != null)
                    UpdateCheckState(UpdateFileState.UpdateResource);
                if (UpdateDownResWarn != null)
                    UpdateDownResWarn(UpdateFileState.DownResWarn, _downResSize);
                //SetChangeState(UpdateFileState.DownResWarn);
                break;
            case UpdateFileState.DeleteFile:
                DeleteFile();
                break;
            case UpdateFileState.Over:
                UpdateOver();
                break;
            case UpdateFileState.DownAPK:
                UpdateCheckState(UpdateFileState.DownAPK);
                if (UpdateDownResWarn != null)
                    UpdateDownResWarn(UpdateFileState.DownApkWarn, _downResSize);
                //SetChangeState(UpdateFileState.DownApkWarn);
                break;
            case UpdateFileState.DownCode:
                UpdateCheckState(UpdateFileState.DownCode);
                if (UpdateDownResWarn != null)
                    UpdateDownResWarn(UpdateFileState.DownCodeWarn, _downResSize);
                //SetChangeState(UpdateFileState.DownCode);
                break;
        }

    }

    public void SetChangeState(UpdateFileState vState)
    {
        _state = vState;
        DoneOperateForStatus();
    }

    private IEnumerator CheckVersion()
    {
        StringBuilder sub = new StringBuilder();
        sub.Append(ResPath.DOWNLOADPATH);
        sub.Append(GlobalConst.PLATFORM);
        sub.Append(ResPath.CHECKVERSION_ASSETNAME);
        string url = sub.ToString();
        WWW www;
        try
        {
            www = new WWW(url);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            UpdateFailure(UpateErrorType.NotFindVersionXml);
            yield break;
        }
        yield return www;
        if (www.error != null)
        {
            UpdateFailure(UpateErrorType.NotFindVersionXml);
            yield break;
        }
        ResolveUpdateList(www.bytes);
    }

    private void ResolveUpdateList(byte[] bytes)
    {
        _bytes = bytes;
        try
        {
            string xmlstr = System.Text.Encoding.UTF8.GetString(bytes);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement xe in nodelist)
            {
                if (xe.Name == "apkversion")
                {
                    _serverVersionData.ApkVersion = float.Parse(xe.InnerText);
                }

                if (xe.Name == "resversion")
                {
                    _serverVersionData.ResVersion = float.Parse(xe.InnerText);
                }

                if (xe.Name == "codeversion")
                {
                    _serverVersionData.CodeVersion = float.Parse(xe.InnerText);
                }

                if (xe.Name == "codeurl")
                {
                    _serverVersionData.CodeUrl = xe.InnerText;
                }

                if (xe.Name == "resurl")
                {
                    _serverVersionData.ResUrl = xe.InnerText;
                }

                if (xe.Name == "apkurl")
                {
                    _serverVersionData.ApkUrl = xe.InnerText;
                }

                if (xe.Name == "apksize")
                {
                    _serverVersionData.ApkSize = long.Parse(xe.InnerText);
                }

                if (xe.Name == "codesize")
                {
                    _serverVersionData.CodeSize = long.Parse(xe.InnerText);
                }

            }

        }
        catch (System.Exception ex)
        {
            UpdateFailure(UpateErrorType.VersionXmlError);
            Debug.LogError(ex.Message);
            return;
        }
        StringBuilder sub = new StringBuilder();
        sub.Append(ResPath.WLOCALPATH);
        sub.Append(ResPath.CHECHVERSIONXMLPATH);
        sub.Append(GlobalConst.PLATFORM);
        sub.Append(ResPath.CHECKVERSION_ASSETNAME);
        string localPath = sub.ToString();
        VersionPath = localPath;
        if (!Directory.Exists(Path.GetDirectoryName(localPath)))
            Directory.CreateDirectory(Path.GetDirectoryName(localPath));
        if (!File.Exists(localPath))
        {
            try
            {
                File.WriteAllBytes(localPath, bytes);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                UpdateFailure(UpateErrorType.VersionXmlWriteError);
                return;
            }
            _localVersionData.ResVersion = 0;
            _localVersionData.ApkUrl = _serverVersionData.ApkUrl;
            _localVersionData.CodeUrl = _serverVersionData.CodeUrl;
            _localVersionData.ResUrl = _serverVersionData.ResUrl;
        }
        else
        {
            ReadLocalVersionData(localPath);
            try
            {
                File.WriteAllBytes(localPath, bytes);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                UpdateFailure(UpateErrorType.VersionXmlWriteError);
                return;
            }
        }
        SetChangeState(UpdateFileState.ComparedVersion);
    }

    public void ReadLocalVersionData(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(path);
            XmlNode nodelist = xmlDoc.SelectSingleNode("Data");

            foreach (XmlElement xe in nodelist)
            {
                if (xe.Name == "resversion")
                {
                    _localVersionData.ResVersion = float.Parse(xe.InnerText);
                }

                if (xe.Name == "codeurl")
                {
                    _localVersionData.CodeUrl = xe.InnerText;
                }

                if (xe.Name == "resurl")
                {
                    _localVersionData.ResUrl = xe.InnerText;
                }

                if (xe.Name == "apkurl")
                {
                    _localVersionData.ApkUrl = xe.InnerText;
                }
            }
        }
        catch (System.Exception ex)
        {
            UpdateFailure(UpateErrorType.VersionXmlReadError);
            Debug.LogError(ex.Message);
            return;
        }
    }

    private void CompareVersion()
    {
        if (_serverVersionData.ApkVersion > _localVersionData.ApkVersion)
        {
            DelVersionXml();
            _downResSize = _serverVersionData.ApkSize;
            SetChangeState(UpdateFileState.DownAPK);
            return;
        }

        if (Application.platform == RuntimePlatform.Android && GlobalConst.ISOPENCODEUPDATE)
        {
            if (_serverVersionData.CodeVersion > GlobalConst.VERSION_CODENUMBER)
            {
                DelVersionXml();
                _downResSize = _serverVersionData.CodeSize;
                SetChangeState(UpdateFileState.DownCode);
                return;
            }
        }

        if (_serverVersionData.ResVersion > _localVersionData.ResVersion)
        {
            SetChangeState(UpdateFileState.GetUpdateList);
        }
        else
        {
            SetChangeState(UpdateFileState.ReadResPath);
        }

    }

    private IEnumerator DownLoadResList()
    {
        StringBuilder sub = new StringBuilder();
        sub.Append(ResPath.DOWNLOADPATH);
        sub.Append(ResPath.RESPATHXML);
        string url = sub.ToString();
        WWW www;
        try
        {
            www = new WWW(url);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            UpdateFailure(UpateErrorType.NotFindResXml);
            yield break;
        }
        yield return www;
        if (www.error != null)
        {
            UpdateFailure(UpateErrorType.NotFindResXml);
            yield break;
        }
        try
        {
            string local = ResPath.WLOCALPATH + ResPath.RESPATHXML;
            if (!Directory.Exists(Path.GetDirectoryName(local)))
                Directory.CreateDirectory(Path.GetDirectoryName(local));
            File.WriteAllBytes(local, www.bytes);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            UpdateFailure(UpateErrorType.ResXmlWriteError);
            yield break;
        }
        SetChangeState(UpdateFileState.ReadResPath);
    }

    public IEnumerator ReadLocalResPath()
    {
        StringBuilder sub = new StringBuilder();
        sub.Append(ResPath.WLOCALPATH);
        sub.Append(ResPath.RESPATHXML);
        string localPath = sub.ToString();
        if (!File.Exists(localPath))
        {
            SetChangeState(UpdateFileState.GetUpdateList);
            yield break;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(localPath);
        XmlNodeList nodelist = xmlDoc.SelectSingleNode("Items").ChildNodes;
        UpdateFileData data;
        data.FileSize = 0;
        data.FilePath = "";
        data.FileMD5 = "";
        foreach (XmlElement xe in nodelist)
        {
            data.FileName = xe.GetAttribute("Name");
            data.FilePath = xe.GetAttribute("Path");
            data.FileMD5 = xe.GetAttribute("MD5");
            data.FileSize = long.Parse(xe.GetAttribute("Size"));
            _serverFileList.Add(data);
            _resPath.Add(data.FileName, data.FilePath);
        }
        CompareRes();
    }

    public void CompareRes()
    {
        GetLocalFilePath(ResPath.WLOCALPATH);
        for (int i = 0; i < _serverFileList.Count; i++)
        {
            FilePathData data = new FilePathData();
            data.LocalPath = ResPath.WLOCALPATH + _serverFileList[i].FilePath;
            data.UrlPath = _serverVersionData.ResUrl + _serverFileList[i].FilePath;
            data.FileSize = _serverFileList[i].FileSize;
            if (!File.Exists(data.LocalPath))
            {
                _updateResData.Add(data);
            }
            else
            {
                string md5 = "";
                CommonFunction.GetFileMD5(data.LocalPath, out md5);
                if (md5 != _serverFileList[i].FileMD5 || md5 == "")
                {
                    DeleteSuperfluousFile(data.LocalPath);
                    _updateResData.Add(data);
                }
            }
            _localFileList.Remove(data.LocalPath);
        }
        if (_updateResData.Count > 0)
            SetChangeState(UpdateFileState.UpdateResource);
        else
            SetChangeState(UpdateFileState.DeleteFile);

    }



    private void DownLoadRes()
    {
        DownLoadManager.Instance.BeginDownload();
    }

    private void SetDownLoadResManager()
    {
        int count = _updateResData.Count;
        if (count <= 0)
        {
            SetChangeState(UpdateFileState.DeleteFile);
            return;
        }
        DownLoadManager.Instance.mFinished = DownLoadFinish;
        DownLoadManager.Instance.mDownLoadException = DownLoadException;
        DownLoadManager.Instance.mProgress = Progress;
        DownLoadManager.Instance.mSizeProgress = SizeProgress;
        for (int i = 0; i < count; i++)
        {
            DownloadTask data = new DownloadTask(_updateResData[i].UrlPath, _updateResData[i].LocalPath, _updateResData[i].FileSize);
            DownLoadManager.Instance.AddTask(data);
            _downResSize += _updateResData[i].FileSize;
        }
    }

    public void DownLoadFinish(string vPath)
    {
        SetChangeState(UpdateFileState.DeleteFile);
    }

    public void DownLoadException(string vPath)
    {
        if (UpdateResError != null)
            UpdateResError(vPath);
        //TODO:服务器上没有这个文件说明是有问题的 但是一切服务器为准
    }

    public void Progress(float progress)
    {
        if (UpdateResValue != null)
            UpdateResValue(progress);
    }

    public void SizeProgress(long currentSize, long totalSize)
    {
        if (UpdateResSizeValue != null)
            UpdateResSizeValue(currentSize, totalSize);
    }

    private void DeleteFile()
    {
        StringBuilder sub = new StringBuilder();
        sub.Append(ResPath.WLOCALPATH);
        sub.Append(ResPath.CHECHVERSIONXMLPATH);
        sub.Append(GlobalConst.PLATFORM);
        sub.Append(ResPath.CHECKVERSION_ASSETNAME);
        string localPath = sub.ToString();
        _localFileList.Remove(localPath);
        _localFileList.Remove(ResPath.WLOCALPATH + ResPath.RESPATHXML);

        for (int i = 0; i < _localFileList.Count; i++)
        {
            File.Delete(_localFileList[i]);
        }
        SetChangeState(UpdateFileState.Over);
    }

    private void UpdateOver()
    {
        if (UpdateCheckState != null)
            UpdateCheckState(UpdateFileState.Over);
    }

    private void GetLocalFilePath(string vDirectoryPath)
    {
        if (string.IsNullOrEmpty(vDirectoryPath))
            return;
        if (!Directory.Exists(vDirectoryPath))
            return;
        string[] tmpFileArray = Directory.GetFiles(vDirectoryPath);
        foreach (string tmpSingleFile in tmpFileArray)
        {
            _localFileList.Add(tmpSingleFile.Replace(@"\", "/"));
        }


        string[] tmpDirectoryArray = Directory.GetDirectories(vDirectoryPath);
        foreach (string tmpSingleDirectory in tmpDirectoryArray)
            GetLocalFilePath(tmpSingleDirectory);
    }

    private void DeleteSuperfluousFile(string vFilePath)
    {
        if (!File.Exists(vFilePath))
            return;
        File.Delete(vFilePath);
    }

    private void DownLoadCode()
    {
        //DelVersionXml();
        if (UpdateCheckState != null)
            UpdateCheckState(UpdateFileState.DownCode);
        FilePathData data;
        data.UrlPath = _serverVersionData.CodeUrl;
        data.LocalPath = ResPath.DLLPATH;
        DownLoadManager.Instance.mProgress = Progress;
        DownLoadManager.Instance.mSizeProgress = SizeProgress;
        DownLoadManager.Instance.mFinished = DownLoadCodeFinish;
        DownLoadManager.Instance.mDownLoadException = DownLoadCodeException;
        DownLoadManager.Instance.AddTask(new DownloadTask(data.UrlPath, data.LocalPath, _serverVersionData.CodeSize));
        DownLoadManager.Instance.BeginDownload();
    }

    private void DownLoadAPK()
    {
        if (File.Exists(ResPath.DLLPATH))
            File.Delete(ResPath.DLLPATH);

        //DelVersionXml();
        if (UpdateCheckState != null)
            UpdateCheckState(UpdateFileState.DownAPK);
        int index = _serverVersionData.ApkUrl.LastIndexOf("/");
        string str = _serverVersionData.ApkUrl.Substring(index);
        if (Application.platform == RuntimePlatform.Android)
        {
            if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
            {
                DownLoadAPKFinish(_serverVersionData.ApkUrl);
            }
            else
            {
                FilePathData data;
                data.UrlPath = _serverVersionData.ApkUrl;
                data.LocalPath = ResPath.APKPATH + str;
                DownLoadManager.Instance.mProgress = Progress;
                DownLoadManager.Instance.mSizeProgress = SizeProgress;
                DownLoadManager.Instance.mFinished = DownLoadAPKFinish;
                DownLoadManager.Instance.mDownLoadException = DownLoadAPKException;
                DownLoadManager.Instance.AddTask(new DownloadTask(data.UrlPath, data.LocalPath, _serverVersionData.ApkSize));
                DownLoadManager.Instance.BeginDownload();
            }

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DownLoadAPKFinish(_serverVersionData.ApkUrl);
        }
        else
        {
            Debug.LogError("Plz change apkversion number");
        }

    }

    public void DownLoadAPKFinish(string vPath)
    {
        Application.OpenURL(vPath);
        Application.Quit();
    }

    public void DownLoadCodeFinish(string vPath)
    {
        if (UpdateCheckState != null)
            UpdateCheckState(UpdateFileState.DownCodeOver);
    }

    public void DownLoadCodeException(string vPath)
    {
        if (CheckFailure != null)
            CheckFailure(UpateErrorType.DownCodeError);
    }

    public void DownLoadAPKException(string vPath)
    {
        if (CheckFailure != null)
            CheckFailure(UpateErrorType.DownCodeError);
    }

    private void UpdateFailure(UpateErrorType errortype)
    {
        if (CheckFailure != null)
            CheckFailure(errortype);
    }

    public string GetFilePath(string vName)
    {
        string path = "";
        if (_resPath.ContainsKey(vName))
        {
            _resPath.TryGetValue(vName, out path);
        }
        return path;
    }


    public void DelVersionXml()
    {
        StringBuilder sub = new StringBuilder();
        sub.Append(ResPath.WLOCALPATH);
        sub.Append(ResPath.RESPATHXML);
        if (File.Exists(sub.ToString()))
            File.Delete(sub.ToString());
        if (File.Exists(VersionPath))
            File.Delete(VersionPath);
    }

    public void Uninitialize()
    {
        _downResSize = 0;
        _updateResData.Clear();
        _resPath.Clear();
        _localFileList.Clear();
        _serverFileList.Clear();
        _state = UpdateFileState.None;
    }

}
