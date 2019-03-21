using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 声效管理
/// </summary>
public class SoundManager : MonoSingleton<SoundManager>
{
    //常量--------------------------------------------------------------------//
    /// <summary>
    /// 存取Key值-音乐禁止
    /// </summary>
    private const string SAVEKEY_MUSIC_MUTE = "Music_Mute";
    /// <summary>
    /// 存取Key值-音乐音量
    /// </summary>
    private const string SAVEKEY_MUSIC_VOLUME = "Music_Volume";
    /// <summary>
    /// 存取Key值-音效禁止
    /// </summary>
    private const string SAVEKEY_AUDIO_MUTE = "Audio_Mute";
    /// <summary>
    /// 存取Key值-音效音量
    /// </summary>
    private const string SAVEKEY_AUDIO_VOLUME = "Audio_Volume";
    /// <summary>
    /// 优先级-背景音乐
    /// </summary>
    private const int PRIORITY_MUSIC = 0;
    /// <summary>
    /// 优先级-音效
    /// </summary>
    private const int PRIORITY_AUDIO = 128;
    /// <summary>
    /// 音量减小时间
    /// </summary>
    private const float REDUCEVOLUMETIME_MUSIC = 3;


    //属性--------------------------------------------------------------------//
    /// <summary>
    /// 背景音乐组件
    /// </summary>
    private AudioSource _Music;
    /// <summary>
    /// 背景音乐是否静音[true/1-禁止 false/0-开启]
    /// </summary>
    private bool _MusicMute;
    /// <summary>
    /// 背景音乐音量
    /// </summary>
    private float _MusicVolume;
    /// <summary>
    /// 音效列表组件
    /// </summary>
    private Dictionary<uint, AudioSource> _DicAudio = new Dictionary<uint, AudioSource>();
    /// <summary>
    /// 音效是否静音[true/1-禁止 false/0-开启]
    /// </summary>
    private bool _AudioMute;
    /// <summary>
    /// 音效音量
    /// </summary>
    private float _AudioVolume;
    /// <summary>
    /// 音效索引
    /// </summary>
    private uint _AudioIndex;
    /// <summary>
    /// 声音音调[速度]
    /// </summary>
    private float _SoundPitch;
    /// <summary>
    /// 背景音乐名字-资源
    /// </summary>
    private string musicName_Res = string.Empty;
    /// <summary>
    /// 背景音乐名字-最终命名
    /// </summary>
    private string musicName_Final = string.Empty;

    //属性接口--------------------------------------------------------------------//
    /// <summary>
    /// 背景音乐音量修改/设置
    /// </summary>
    public float Get_MusicVolume
    {
        get {
            return _MusicVolume;
        }
    }
    /// <summary>
    /// 背景音乐是否禁止
    /// </summary>
    public bool Get_MusicMute
    {
        get {
            return _MusicMute;
        }
    }
    /// <summary>
    /// 音效音量修改/设置
    /// </summary>
    public float Get_AudioVolume
    {
        get {
            return _AudioVolume;
        }
    }
    /// <summary>
    /// 音效是否禁止
    /// </summary>
    public bool Get_AudioMute
    {
        get {
            return _AudioMute;
        }
    }


    //控制方法--------------------------------------------------------------------//
    void OnDestroy()
    {
        SaveInfo();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        //CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SetPause);
        //CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SetResume);
        //CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_SetPitch);

        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_PlayMusic, CommandEvent_PlayMusic);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_PlayAudio, CommandEvent_PlayAudio);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_ChangeVolume_Music, CommandEvent_ChangeVolume_Music);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_ChangeVolume_Audio, CommandEvent_ChangeVolume_Audio);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Music, CommandEvent_SetMuteStatus_Music);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Audio, CommandEvent_SetMuteStatus_Audio);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_Clear_Sound, CommandEvent_ClearSound);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_Clear_Music, CommandEvent_ClearMusic);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_Clear_Audio, CommandEvent_ClearAudio);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_Save_Sound_Status, CommandEvent_SaveSoundStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_DeleteSingle_Audio, CommandEvent_DeleteSingle_Audio);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Sound.SM_ReduceVolume_Music, CommandEvent_ReduceMusicVolume);

        
        _AudioIndex = 0;
        _SoundPitch = 1;

        LoadInfo();
    }
    /// <summary>
    /// 反初始化
    /// </summary>
    public void Uninitialize()
    {
        //CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SetPause);
        //CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SetResume);
        //CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_SetPitch);

        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_PlayMusic, CommandEvent_PlayMusic);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_PlayAudio, CommandEvent_PlayAudio);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_ChangeVolume_Music, CommandEvent_ChangeVolume_Music);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_ChangeVolume_Audio, CommandEvent_ChangeVolume_Audio);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Music, CommandEvent_SetMuteStatus_Music);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Audio, CommandEvent_SetMuteStatus_Audio);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_Clear_Sound, CommandEvent_ClearSound);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_Clear_Music, CommandEvent_ClearMusic);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_Clear_Audio, CommandEvent_ClearAudio);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_Save_Sound_Status, CommandEvent_SaveSoundStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_DeleteSingle_Audio, CommandEvent_DeleteSingle_Audio);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Sound.SM_ReduceVolume_Music, CommandEvent_ReduceMusicVolume);
    }


    //命令方法--------------------------------------------------------------------//
    /// <summary>
    /// 设置是否暂停
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetPause(object vDataObj)
    {
        if (_Music != null)
            _Music.Pause();
        if (_DicAudio != null)
        {
            foreach (KeyValuePair<uint, AudioSource> tmpInfo in _DicAudio)
            {
                if (tmpInfo.Value == null)
                    continue;
                tmpInfo.Value.Pause();
            }
        }
    }
    /// <summary>
    /// 取消暂停
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetResume(object vDataObj)
    {
        if (_Music != null)
            _Music.Play();
        if (_DicAudio == null)
        {
            foreach (KeyValuePair<uint, AudioSource> tmpInfo in _DicAudio)
            {
                if (tmpInfo.Value == null)
                    continue;
                tmpInfo.Value.Play();
            }
        }
    }
    /// <summary>
    /// 设置音调[速度]
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetPitch(object vDataObj)
    {
        if (_SoundPitch == 1)
            _SoundPitch = 2;
        else
            _SoundPitch = 1;

        if (_Music != null)
            _Music.pitch = _SoundPitch;
        if (_DicAudio == null)
        {
            foreach (KeyValuePair<uint, AudioSource> tmpInfo in _DicAudio)
            {
                if (tmpInfo.Value == null)
                    continue;
                tmpInfo.Value.pitch = _SoundPitch;
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="vDataObj">名字</param>
    private void CommandEvent_PlayMusic(object vDataObj)
    {
        //string tmpName = (string)vDataObj;
        //if (string.IsNullOrEmpty(tmpName))
        //{
        //    Debug.LogError("Music Is Empty");
        //    return;
        //}
        //if (tmpName.Equals("0"))
        //{
        //    return;
        //}
        //if (!tmpName.Contains("sound_"))
        //    tmpName = string.Format("sound_{0}", tmpName);
        //if (!tmpName.Contains(".assetbundle"))
        //    tmpName = string.Format("{0}.assetbundle", tmpName);

        //string tmpFinalName = tmpName.Substring(0, tmpName.IndexOf('.'));
        //if (!CheckMusicIsCanPlay(tmpFinalName))
        //    return;
        //if (_Music != null)
        //    GameObject.Destroy(_Music.gameObject);
        //_Music = null;
        //ResourceLoadManager.Instance.LoadSound(tmpName, ResourceLoadType.AssetBundle, (Source) => {
        //        GameObject tmpObj = GameObject.Instantiate(Source) as GameObject;
        //        tmpObj.transform.parent = this.transform;
        //        if (tmpObj != null)
        //        {
        //            _Music = tmpObj.GetComponent<AudioSource>();
        //            _Music.name = tmpFinalName;
        //            _Music.volume = _MusicVolume;
        //            _Music.pitch = _SoundPitch;
        //            _Music.loop = true;
        //            _Music.priority = PRIORITY_MUSIC;
        //            _Music.Play();
        //            _Music.mute = _MusicMute;
        //        }
        //    });


        Scheduler.Instance.RemoveUpdator(ReduceMusicVolumeOperate);
        musicName_Res = (string)vDataObj;
        if (string.IsNullOrEmpty(musicName_Res))
        {
            Debug.LogError("Music Is Empty");
            return;
        }
        if (musicName_Res.Equals("0"))
        {
            return;
        }
        if (!musicName_Res.Contains("sound_"))
            musicName_Res = string.Format("sound_{0}", musicName_Res);
        if (!musicName_Res.Contains(".assetbundle"))
            musicName_Res = string.Format("{0}.assetbundle", musicName_Res);

        musicName_Final = musicName_Res.Substring(0, musicName_Res.IndexOf('.'));
        if (!CheckMusicIsCanPlay(musicName_Final))
            return;
        PlayMusicOperate();
    }

    /// <summary>
    /// 背景音乐设置操作
    /// </summary>
    private void PlayMusicOperate()
    {
        if (string.IsNullOrEmpty(musicName_Res))
            return;
        if (_Music != null)
            GameObject.Destroy(_Music.gameObject);
        _Music = null;
        ResourceLoadManager.Instance.LoadSound(musicName_Res, ResourceLoadType.AssetBundle, (Source) =>
        {
            GameObject tmpObj = GameObject.Instantiate(Source) as GameObject;
            tmpObj.transform.parent = this.transform;
            if (tmpObj != null)
            {
                _Music = tmpObj.GetComponent<AudioSource>();
                _Music.name = musicName_Final;
                _Music.volume = _MusicVolume;
                _Music.pitch = _SoundPitch;
                _Music.loop = true;
                _Music.priority = PRIORITY_MUSIC;
                _Music.Play();
                _Music.mute = _MusicMute;
            }
        });
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="vDataObj">名字</param>
    private void CommandEvent_PlayAudio(object vDataObj)
    {
        ShowAudioInfo tmpInfo = (ShowAudioInfo)vDataObj;
        if (tmpInfo == null)
            return;
        if (string.IsNullOrEmpty(tmpInfo.mName))
        {
            Debug.LogError(string.Format("Sound Is Empty: [{0}]", tmpInfo.mName));
            return;
        }
        if (tmpInfo.mName.Equals("0"))
        {
            return;
        }
        string tmpAudioName = tmpInfo.mName;
        if (!tmpAudioName.Contains("sound_"))
            tmpAudioName = string.Format("sound_{0}", tmpAudioName);
        if (!tmpAudioName.Contains(".assetbundle"))
            tmpAudioName = string.Format("{0}.assetbundle", tmpAudioName);

        string tmpFinalName = tmpAudioName.Substring(0, tmpAudioName.IndexOf('.'));
        if (!CheckSingleAudioIsCanPlay(tmpFinalName))
            return;
        ResourceLoadManager.Instance.LoadSound(tmpAudioName, ResourceLoadType.AssetBundle, (Source) =>
            {
                if (Source != null)
                {
                    GameObject tmpObj = GameObject.Instantiate(Source) as GameObject;
                    if (tmpObj != null)
                    {
                        if ((tmpInfo != null) || (tmpInfo.mParent != null))
                        {
                            tmpObj.transform.parent = tmpInfo.mParent;
                        }
                        else
                        {
                            tmpObj.transform.parent = this.transform;
                        }

                        AudioSource tmpSource = tmpObj.GetComponent<AudioSource>();
                        if (tmpSource != null)
                        {
                            tmpSource.name = tmpFinalName;
                            //tmpSource.volume = _AudioVolume;
                            tmpSource.volume = 1;
                            tmpSource.pitch = _SoundPitch;
                            tmpSource.loop = false;
                            tmpSource.priority = PRIORITY_AUDIO;
                            tmpSource.Play();
                            _DicAudio.Add(_AudioIndex, tmpSource);
                            StartCoroutine("CloseFinishAudio", _AudioIndex);
                            _AudioIndex++;
                        }
                    }
                }
            });
    }
    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ChangeVolume_Music(object vDataObj)
    {
        Scheduler.Instance.RemoveUpdator(ReduceMusicVolumeOperate);
        if (vDataObj == null)
            return;
        float tmpValue = (float)vDataObj;
        _MusicVolume = tmpValue;
        if (_MusicVolume < 0)
            _MusicVolume = 0;
        if (_MusicVolume > 1)
            _MusicVolume = 1;
        if (_Music == null)
            return;
        _Music.volume = _MusicVolume;
        SaveInfo();
    }
    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ChangeVolume_Audio(object vDataObj)
    {
        if (vDataObj == null)
            return;
        float tmpValue = (float)vDataObj;
        _AudioVolume = tmpValue;
        if (_DicAudio == null)
            return;
        foreach (KeyValuePair<uint, AudioSource> tmpInfo in _DicAudio)
        {
            if (tmpInfo.Value == null)
                continue;
            tmpInfo.Value.volume = _AudioVolume;
        }
        SaveInfo();
    }
    /// <summary>
    /// 设置静音状态-背景音乐
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetMuteStatus_Music(object vDataObj)
    {
        Scheduler.Instance.RemoveUpdator(ReduceMusicVolumeOperate);
        if (vDataObj == null)
            return;
        bool tmpValue = (bool)vDataObj;
        _MusicMute = tmpValue;
        _MusicVolume = !_MusicMute ? 0.5f : 0;
        if (_Music == null)
        {
            PlayMusicOperate();
            return;
        }
        _Music.mute = _MusicMute;
        _Music.volume = _MusicVolume;
        SaveInfo();
    }
    /// <summary>
    /// 设置静音状态-音效
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetMuteStatus_Audio(object vDataObj)
    {
        if (vDataObj == null)
            return;
        bool tmpValue = (bool)vDataObj;
        _AudioMute = tmpValue;
        if (_DicAudio == null)
            return;
        foreach (KeyValuePair<uint, AudioSource> tmpInfo in _DicAudio)
        {
            if (tmpInfo.Value == null)
                continue;
            tmpInfo.Value.mute = _AudioMute;
        }
        _AudioVolume = !_AudioMute ? 0.5f : 0;
        SaveInfo();
    }
    /// <summary>
    /// 清空
    /// </summary>
    private void CommandEvent_ClearSound(object vDataObj)
    {
        CommandEvent_ClearMusic(null);
        CommandEvent_ClearAudio(null);
    }
    /// <summary>
    /// 清空背景音乐
    /// </summary>
    private void CommandEvent_ClearMusic(object vDataObj)
    {
        if (_Music == null)
            return;
        _Music.Stop();
        GameObject.Destroy(_Music.gameObject);
    }
    /// <summary>
    /// 清空音效
    /// </summary>
    private void CommandEvent_ClearAudio(object vDataObj)
    {
        _AudioIndex = 0;
        if (_DicAudio == null)
            return;
        foreach (KeyValuePair<uint, AudioSource> tmpInfo in _DicAudio)
        {
            if (tmpInfo.Value == null)
                continue;
            tmpInfo.Value.Stop();
            GameObject.Destroy(tmpInfo.Value.gameObject);
        }
        _DicAudio.Clear();
    }
    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SaveSoundStatus(object vDataObj)
    {
        SaveInfo();
    }
    /// <summary>
    /// 删除一个音效
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_DeleteSingle_Audio(object vDataObj)
    {
        if (_DicAudio == null)
            return;
        if (_DicAudio.Count <= 0)
            return;
        ShowAudioInfo tmpInfo = (ShowAudioInfo)vDataObj;
        if (tmpInfo == null)
            return;
        if (string.IsNullOrEmpty(tmpInfo.mName))
            return;

        string tmpAudioName = tmpInfo.mName;
        if (!tmpAudioName.Contains("sound_"))
            tmpAudioName = string.Format("sound_{0}", tmpAudioName);
        if (!tmpAudioName.Contains(".assetbundle"))
            tmpAudioName = string.Format("{0}.assetbundle", tmpAudioName);
        string tmpFinalName = tmpAudioName.Substring(0, tmpAudioName.IndexOf('.'));

        List<uint> tmpListKey = new List<uint>();
        foreach (KeyValuePair<uint, AudioSource> tmpSingleInfo in _DicAudio)
        {
            if (tmpSingleInfo.Value == null)
            {
                tmpListKey.Add(tmpSingleInfo.Key);
                continue;
            }
            if (!tmpSingleInfo.Value.name.Equals(tmpFinalName))
                continue;

            if (tmpInfo.mParent != null)
            {
                if (tmpInfo.mParent != tmpSingleInfo.Value.transform.parent)
                    continue;
            }

            tmpSingleInfo.Value.Stop();
            GameObject.Destroy(tmpSingleInfo.Value.gameObject);
            tmpListKey.Add(tmpSingleInfo.Key);
            break;
        }

        for (int i = 0; i < tmpListKey.Count; i++)
        {
            if (_DicAudio.ContainsKey(tmpListKey[i]))
                _DicAudio.Remove(tmpListKey[i]);
        }
    }

    /// <summary>
    /// 逐渐减小背景音量
    /// </summary>
    /// <param name="vDataObj"></param>
    private float initReduceMusicVolumeTime;
    private bool isReduceMusicVolumeTime = false;
    private void CommandEvent_ReduceMusicVolume(object vDataObj)
    {
        if (isReduceMusicVolumeTime)
            return;
        isReduceMusicVolumeTime = true;
        initReduceMusicVolumeTime = Time.time;
        Scheduler.Instance.AddUpdator(ReduceMusicVolumeOperate);
    }


    //工具方法--------------------------------------------------------------------//
    /// <summary>
    /// 逐渐减小背景音量
    /// </summary>
    private void ReduceMusicVolumeOperate()
    {
        if (_Music == null)
        {
            Scheduler.Instance.RemoveUpdator(ReduceMusicVolumeOperate);
            isReduceMusicVolumeTime = false;
            return;
        }
        _Music.volume = _MusicVolume * (1 - (Time.time - initReduceMusicVolumeTime) / REDUCEVOLUMETIME_MUSIC);
        if (_Music.volume <= 0)
        {
            _Music.volume = 0;
            Scheduler.Instance.RemoveUpdator(ReduceMusicVolumeOperate);
            isReduceMusicVolumeTime = false;
        }
    }

    /// <summary>
    /// 检测一个背景音乐是否能够播放
    /// </summary>
    /// <param name="vName">名字</param>
    /// <returns>结果[true-可以播放 false-不能播放]</returns>
    private bool CheckMusicIsCanPlay(string vName)
    {
        //检测是否静音//
        if (_MusicMute)
            return false;
        //检测是否空文件//
        if (string.IsNullOrEmpty(vName))
            return false;
        //检测是否当前播放音乐//
        if (_Music != null)
        {
            if (_Music.name.Equals(vName))
            {
                if (_Music.isPlaying)
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测一个音效是否能够播放
    /// </summary>
    /// <param name="vName">名字</param>
    /// <returns>结果[true-可以播放 false-不能播放]</returns>
    private bool CheckSingleAudioIsCanPlay(string vName)
    {
        if (_AudioMute)
            return false;
        if (string.IsNullOrEmpty(vName))
            return false;
        return true;
    }

    /// <summary>
    /// 检测并关闭已经播放完毕的音效
    /// </summary>
    /// <param name="vIndex">音效索引</param>
    /// <returns></returns>
    private IEnumerator CloseFinishAudio(uint vIndex)
    {
        if (_DicAudio == null)
            yield break;
        if (!_DicAudio.ContainsKey(vIndex))
            yield break;
        if (_DicAudio[vIndex] == null)
        {
            _DicAudio.Remove(vIndex);
            yield break;
        }
        if (_DicAudio[vIndex].clip == null)
        {
            GameObject.Destroy(_DicAudio[vIndex].gameObject);
            _DicAudio.Remove(vIndex);
            yield break;
        }

        yield return new WaitForSeconds(_DicAudio[vIndex].clip.length + 1);

        if (!_DicAudio.ContainsKey(vIndex))
            yield break;
        if (_DicAudio[vIndex] != null)
        {
            GameObject.Destroy(_DicAudio[vIndex].gameObject);
        }
        _DicAudio.Remove(vIndex);
    }


    //存储-读取数据------------------------------------------------------------------------------//
    /// <summary>
    /// 保存信息
    /// </summary>
    private void SaveInfo()
    {
        Save_Music_Mute();
        Save_Music_Volume();
        Save_Audio_Mute();
        Save_Audio_Volume();
    }

    /// <summary>
    /// 读取信息
    /// </summary>
    private void LoadInfo()
    {
        Load_Music_Mute();
        Load_Music_Volume();
        Load_Audio_Mute();
        Load_Audio_Volume();
        _SoundPitch = 1;
    }

    private void Save_Music_Mute()
    {
        if (_MusicMute)
            PlayerPrefs.SetInt(SAVEKEY_MUSIC_MUTE, 1);
        else
            PlayerPrefs.SetInt(SAVEKEY_MUSIC_MUTE, 0);
    }
    private void Load_Music_Mute()
    {
        if (PlayerPrefs.GetInt(SAVEKEY_MUSIC_MUTE, 0) == 1)
            _MusicMute = true;
        else
            _MusicMute = false;
    }

    private void Save_Music_Volume()
    {
        PlayerPrefs.SetFloat(SAVEKEY_MUSIC_VOLUME, _MusicVolume);
    }
    private void Load_Music_Volume()
    {
        _MusicVolume = PlayerPrefs.GetFloat(SAVEKEY_MUSIC_VOLUME, 0.5f);
    }

    private void Save_Audio_Mute()
    {
        if (_AudioMute)
            PlayerPrefs.SetInt(SAVEKEY_AUDIO_MUTE, 1);
        else
            PlayerPrefs.SetInt(SAVEKEY_AUDIO_MUTE, 0);
    }
    private void Load_Audio_Mute()
    {
        if (PlayerPrefs.GetInt(SAVEKEY_AUDIO_MUTE, 0) == 1)
            _AudioMute = true;
        else
            _AudioMute = false;
    }

    private void Save_Audio_Volume()
    {
        PlayerPrefs.SetFloat(SAVEKEY_AUDIO_VOLUME, _AudioVolume);
    }
    private void Load_Audio_Volume()
    {
        _AudioVolume = PlayerPrefs.GetFloat(SAVEKEY_AUDIO_VOLUME, 0.5f);
    }

}

/// <summary>
/// 播放音效信息
/// </summary>
public class ShowAudioInfo
{
    /// <summary>
    /// 音效名字
    /// </summary>
    public string mName;
    /// <summary>
    /// 挂载的父物件
    /// </summary>
    public Transform mParent;

    /// <summary>
    /// 播放音效信息
    /// </summary>
    /// <param name="vName">音效名字</param>
    /// <param name="vParent">挂载的父物件-按钮音效为默认值</param>
    public ShowAudioInfo(string vName, Transform vParent = null)
    {
        mName = vName;
        mParent = vParent;
    }
}