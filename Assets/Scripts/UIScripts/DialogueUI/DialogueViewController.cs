using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueViewController : UIBase 
{
    public enum TypewriterState
    {
        Coding,
        CodeFinish,
    }

    public DialogueView view;
    public TypewriterState CodeState;

    private uint _dialogueTriggerId = 0;
    private List<Dialogue> _dialogueList = new List<Dialogue>();
    private Dictionary<uint, Texture2D> _npcTex = new Dictionary<uint, Texture2D>();
    private Vector3 _rightDialoguePos = new Vector3(-240f,50f, 0f);
    private Vector3 _leftDialoguePos = new Vector3(-600f, 50f, 0f);
    private Vector3 _leftSpPos = new Vector3(-288, -89, 0);
    private Vector3 _rightSpPos = new Vector3(319, -104, 0);
    private string _lastSoundName;

    public delegate void OnDialogueFinishEventHandler();
    public OnDialogueFinishEventHandler OnDialogueFinishEvent;
   
    public override void Initialize()
    {
        if (view == null)
            view = new DialogueView();
        view.Initialize();
        UIEventListener.Get(view.Spt_MaskBG.gameObject).onClick = DialogueClick;
        EventDelegate.Add( view.TypewriterEffect_ContentLabel.onFinished, OnTypeWriterFinish);

        InitContent();
    }

    private void DialogueClick(GameObject go)
    {
        if (CodeState == TypewriterState.CodeFinish)
        {
            SwitchNextContent();
        }
        else
        {
            FinshTypeWrite();
        }
    }

    public void OpenDialogue(uint dialogueTriggerId, OnDialogueFinishEventHandler onDialogueFinishEvent = null)
    {
        _dialogueTriggerId = dialogueTriggerId;
        _dialogueList.Clear();

        DialogueTrigger trigger = ConfigManager.Instance.mDialogueTriggerConfig.GetDialogueTriggerConfigByID(dialogueTriggerId);
        if (trigger == null)
        {
            Close(null, null);
            return;
        }
        for (int i = 0; i < trigger.mContentID.Count; i++)
        {
            _dialogueList.Add(ConfigManager.Instance.mDialogueConfig.GetDialogueConfigByID(trigger.mContentID[i]));
        }

        OnDialogueFinishEvent = onDialogueFinishEvent;
        SwitchNextContent();
    }

    private void SwitchNextContent()
    {
        if (_dialogueList.Count > 0)
        {
            SetDialogue(_dialogueList[0]);
            BeginTypeWrite();
            _dialogueList.RemoveAt(0);
           
        }
        else
        {
            UISystem.Instance.CloseGameUI(DialogueView.UIName);

            GuideManager.Instance.CheckTrigger(GuideTrigger.DialogueClose);

            if(OnDialogueFinishEvent != null)
            {
                OnDialogueFinishEvent();
            }
            if (!CommonFunction.XmlStringIsNull(_lastSoundName)) {
                StopSound(_lastSoundName);
            }
        }
        
    }

    private void FinshTypeWrite()
    {
        view.TypewriterEffect_ContentLabel.Finish();
    }

    private void BeginTypeWrite()
    {
        view.TypewriterEffect_ContentLabel.ResetToBeginning();
        CodeState = TypewriterState.Coding;
    }

    private void OnTypeWriterFinish()
    {
        CodeState = TypewriterState.CodeFinish;
    }

    private void InitContent()
    {
        FinshTypeWrite();
        view.Lbl_ContentLabel.text = "";
    }

    private void SetDialogue(Dialogue dialogue)
    {
        UISprite sayerSprite;
        string spName="";
        int offset = 0;
        //NPC或者导师在右边 玩家自己在左边
        if (dialogue.mTalker == DialogueTalker.Npc)
        {
            view.Spt_LeftSprite.gameObject.SetActive(false);
            view.Spt_RightSprite.gameObject.SetActive(true);
            sayerSprite = view.Spt_RightSprite;
            if (dialogue.mNpcID == 1)
            {
               spName = GlobalConst.SpriteName.DIALOGUE_GUIDER_1;
            }
            else if (dialogue.mNpcID == 2)
            {
               spName = GlobalConst.SpriteName.DIALOGUE_GUIDER_2;
            }
            else
            {
                //npc icon
            }
            view.Lbl_ContentLabel.transform.localPosition = _leftDialoguePos;
        }
        else
        {
            view.Spt_LeftSprite.gameObject.SetActive(true);
            view.Spt_RightSprite.gameObject.SetActive(false);
            sayerSprite = view.Spt_LeftSprite;
            if (PlayerData.Instance._Gender == 1)
            {
                spName = GlobalConst.SpriteName.DIALOGUE_Player_1;
                offset = dialogue.mOffset[0];
            }
            else
            {
                spName = GlobalConst.SpriteName.DIALOGUE_Player_2;
                offset = dialogue.mOffset[1];
            }

            view.Lbl_ContentLabel.transform.localPosition = _rightDialoguePos;
        }
        view.Lbl_ContentLabel.text = dialogue.mContent;
        //PlaySound
        StopSound(_lastSoundName);
        PlaySound(dialogue.mSound);
        CommonFunction.SetSpriteName(sayerSprite, spName);
        sayerSprite.MakePixelPerfect();
        view.Spt_LeftSprite.transform.localPosition = new Vector3(_leftSpPos.x + offset, _leftSpPos.y, _leftSpPos.z);
    }

    public override void Uninitialize()
    {
        view.Lbl_ContentLabel.text = "";
        _npcTex.Clear();
    }

    private void StopSound(string soundName)
    {
        _lastSoundName = "";
        if (CommonFunction.XmlStringIsNull(soundName))
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_DeleteSingle_Audio, new ShowAudioInfo(soundName));

    }

    private void PlaySound(List<string> soundName)
    {
        string sound = "";
        if (soundName.Count > 1 && !CommonFunction.XmlStringIsNull(soundName[1]))
        {
            sound = PlayerData.Instance._Gender == 1 ? soundName[0] : soundName[1];
        }
        else if (soundName.Count > 0)
        {
            sound = soundName[0];
        }
        _lastSoundName = sound;
        if (CommonFunction.XmlStringIsNull(sound))
            return;
        //Debug.LogWarning("AtSea: " + soundName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(sound));
    }
}

