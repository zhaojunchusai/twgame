using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Assets.Script.Common;
public class DownLoadManager : MonoSingleton<DownLoadManager>
{
    private List<DownloadTask> _taskQueue = new List<DownloadTask>();
    private DownloadTask currentTask;
    WWW www;
    public delegate void OnFinished(string vPath);
    public OnFinished mFinished;
    public OnFinished mDownLoadException;
    public delegate void DownLoadProgress(float fProgress);
    public delegate void DownLoadSizeProgress(long currentSize,long totalSize);
    public DownLoadProgress mProgress;
    public DownLoadSizeProgress mSizeProgress;
    private long _needDownloadSize = 0;
    private long _hasDownloadSize = 0;
    public float progress = 0;
    private bool _isTaskDone = true;
    public bool IsTaskDone
    {
        get { return _isTaskDone; }
    }

    public void Initialize()
    {
        _needDownloadSize = 0;
        _hasDownloadSize = 0;
        _isTaskDone = true;
    }

    public void AddTask(DownloadTask task)
    {
        if (_taskQueue.Contains(task)) return;
        _taskQueue.Add(task);
        this._needDownloadSize += task.size;
    }

    public void BeginDownload()
    {
        if (_taskQueue.Count > 0)
        {
            if (www != null && !www.isDone) 
            {
                NGUIDebug.Log("BeginDownload Error");
                return;
            } 
            currentTask = _taskQueue[0];
            StartCoroutine(DownloadFileWWW(currentTask));
        }
        else 
        {
            onTaskDone(new DownloadTask("null","null"));
        }

    }

    public IEnumerator DownloadFileWWW(DownloadTask task)
    {
        if (!Directory.Exists(Path.GetDirectoryName(task.filePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(task.filePath));
        if (www != null)
            www = null;
        www = new WWW(task.downloadUrl);
        while (!www.isDone || www.progress < 1.0F)
        {
             UpdateSizeProgress();
             yield return 0;
        }
      
        if (www.error != null)
        {
            NGUIDebug.Log(www.error);
            www.Dispose();
            www = null;
            task.reTryTimes++;
            if (task.reTryTimes >= 3)
            {
                onTaskError(task);
                _taskQueue.Remove(task);
            }
            else
            {
                BeginDownload();
            }
        }
        else
        {
            try
            {
                // File.WriteAllBytes(task.filePath, www.bytes);
                CommonFunction.WriteFile(www.bytes, task.filePath);
            }
            catch (Exception e)
            {
                NGUIDebug.Log(e.Message);
                onTaskError(task);
                www.Dispose();
                www = null;
                yield break;
            }
            www.Dispose();
            www = null;
            _hasDownloadSize += task.size;
            if (_taskQueue.Contains(task))
            {
                _taskQueue.Remove(task);
            }
            if (_taskQueue.Count > 0)
            {
                BeginDownload();
            }
            else
            {
                onTaskDone(task);
            }
        }
    }

    public float getProgress() 
    {
        float temp;

        if (_needDownloadSize == 0)
            return 0;

        if(www == null) {
            temp =  1.0F * _hasDownloadSize / _needDownloadSize * 100;
        }
        else if (www.isDone)
        {
            if (www.progress == 0 || www.progress == 1)
            {
                temp = 1.0F * _hasDownloadSize / _needDownloadSize * 100;
            }
            else
            {
                temp = (1.0F * _hasDownloadSize / _needDownloadSize * 100);
            }
        }
        else {
            temp = ((_hasDownloadSize + currentTask.size * www.progress) / _needDownloadSize * 100);
        }
        progress = temp > progress ? temp : progress;
        return progress;
    }

    public void UpdateSizeProgress() 
    {
        if (mSizeProgress != null)
            mSizeProgress(_hasDownloadSize + (long)(currentTask.size * www.progress), _needDownloadSize);
        if (mProgress != null)
            mProgress((float)(_hasDownloadSize +   (long)currentTask.size * www.progress )  /_needDownloadSize);
    }

    public void onTaskDone(DownloadTask task)
    {
        Initialize();
        if (mFinished != null)
            mFinished(task.filePath);
    }

    public void onTaskError(DownloadTask task)
    {
        // 下载出错
        if (mDownLoadException != null)
            mDownLoadException(task.downloadUrl);
    }


}
