using System.Collections;
using System;
public class DownloadTask 
{
    public string downloadUrl;
    public string filePath;
    public long size;
    public int reTryTimes = 0;
    public long downLoadSize;

    public DownloadTask(string url, string path, long size = 0)
    {
        downloadUrl = url;
        filePath = path;
        this.size = size;
    }

}
