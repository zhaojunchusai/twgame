using UnityEngine;
using System.Collections;
//鼠标点击特效
public class ClickEffect : MonoBehaviour
{
    private float DelayTimes;
    private float maxInterval = 0.1F;
    private float maxHoverIntervalTime = 0.2F;
    private float currentHoverTime = 0.0F;
    private Vector3 currentPos = Vector3.zero;
    private bool isHover = false;
    private bool isPress = false;
    private int HitCount = 0;
    private ParticleSystem[] Click1, Click2, Click3, Taril1;
    private GameObject Go_Click1, Go_Click2, Go_Click3, Go_Trial1;

    void Awake()
    {
        //InitEffect
        //DelayTimes = 0.05F;
        Go_Click1 = this.transform.FindChild("ClickEffect/LHF_ClickEffect1").gameObject;
        Click1 = Go_Click1.GetComponentsInChildren<ParticleSystem>();
        Go_Click2 = this.transform.FindChild("ClickEffect/LHF_ClickEffect2").gameObject;
        Click2 = Go_Click2.GetComponentsInChildren<ParticleSystem>();
        Go_Click3 = this.transform.FindChild("ClickEffect/LHF_ClickEffect3").gameObject;
        Click3 = Go_Click3.GetComponentsInChildren<ParticleSystem>();
        Go_Trial1 = this.transform.FindChild("ClickEffect/LHF_ClickEffect4").gameObject;
        Taril1 = Go_Trial1.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (Main.Instance == null || Main.Instance.StateMachine == null || Main.Instance.StateMachine.CurrentState() == null)
            return;
        string stateName = Main.Instance.StateMachine.CurrentState().GetStateName();
        if (stateName == "ReadResState" || stateName == "CheckUpdateState" || stateName == "VersionSelectionState")
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            DelayTimes = Time.time + maxInterval;
            Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0F;
            HitCount = HitCount % 3;
            switch (HitCount)
            {
                case 0:
                    Go_Click1.transform.position = worldPos;
                    PlayEffect(Click1);
                    HitCount += 1;
                    break;
                case 1:
                    Go_Click2.transform.position = worldPos;
                    PlayEffect(Click2);
                    HitCount += 1;
                    break;
                case 2:
                    Go_Click3.transform.position = worldPos;
                    PlayEffect(Click3);
                    HitCount += 1;
                    break;
            }

        }

        if (Input.GetMouseButton(0))
        {
            if (!isPress)
            {
                if (Time.time > DelayTimes) isPress = true;
            }
            else
            {
                if (Mathf.Abs(currentPos.x - Input.mousePosition.x) < 2F && Mathf.Abs(currentPos.y - Input.mousePosition.y) < 2F)
                {
                    if (!isHover)
                    {
                        isHover = true;
                        currentHoverTime = Time.time + maxHoverIntervalTime;
                    }

                }
                else
                {
                    isHover = false;
                }
                currentPos = Input.mousePosition;
                if (isHover)
                {
                    if (Time.time > currentHoverTime)
                    {

                        currentHoverTime = Time.time + maxHoverIntervalTime;
                        if (Go_Trial1.activeSelf)
                            Go_Trial1.SetActive(false);
                    }

                }
                else
                {
                    if (!Go_Trial1.activeSelf)
                        Go_Trial1.SetActive(true);
                    Vector3 WorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                    WorldPos.z = 0F;
                    Go_Trial1.transform.position = WorldPos;
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPress = false;
            isHover = false;
            if (Go_Trial1.activeSelf)
                Go_Trial1.SetActive(false);
        }
    }

    void PlayEffect(ParticleSystem[] array)
    {
        foreach (ParticleSystem ps in array)
        {
            ps.loop = false;
            ps.Play();
        }

    }
}
