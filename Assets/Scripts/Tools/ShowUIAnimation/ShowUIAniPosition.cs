using UnityEngine;
using System.Collections;

public class ShowUIAniPosition : ShowUIAnimation {

    public Vector3 m_from = Vector3.zero;
    public Vector3 m_to = Vector3.zero;
    public EMoveDirection m_direction = EMoveDirection.LeftToRight;
    public UIBoundary m_boundary;

    public override void StartOpenAnimation()
    {
        base.StartOpenAnimation();
        if (m_from == Vector3.zero)
        {
            GetPos(m_boundary);
        }
        Vector3[] path = new Vector3[2]{m_from,m_to};

        iTween.MoveTo(gameObject, iTween.Hash(
            "path", path,
            "islocal", _isLocal,
            "movetopath", _moveToPath,
            "time", m_animationTime,
            "easeType", m_openEaseType.ToString(),
            "loopType", m_loopType.ToString(),
            "delay", m_delay,
            "oncomplete", "OnOpenAnimationCompleted"
            ));
    }

    public override void StartCloseAnimation()
    {
        base.StartCloseAnimation();
        Vector3[] path = new Vector3[2] { m_to, m_from };
        iTween.MoveTo(gameObject, iTween.Hash(
            "path", path,
            "islocal", _isLocal,
            "movetopath", _moveToPath,
            "time", m_animationTime,
            "easeType", m_closeEaseType.ToString(),
            "loopType", m_loopType.ToString(),
            "delay", m_delay,
            "oncomplete", "CloseAnimationCompleted"
            ));
    }

    public void SetUIBoundary(UIBoundary boundary)
    {
        m_boundary = boundary;
    }

    private void GetPos(UIBoundary boundary)
    {
        float halfW = GlobalConst.DEFAULT_SCREEN_SIZE_X / 2;
        float halfH = GlobalConst.DEFAULT_SCREEN_SIZE_Y / 2;

        switch (m_direction)
        {
            case EMoveDirection.DownToUp:
                {
                    m_from = new Vector3(0, - halfH - boundary.up, 0);
                    break;
                }
            case EMoveDirection.RightToLeft:
                {
                    m_from = new Vector3(halfW - boundary.left, 0, 0);
                    break;
                }
            case EMoveDirection.UpToDown:
                {
                    m_from = new Vector3(0, halfH - boundary.down, 0);
                    break;
                }
            case EMoveDirection.LeftToRight:
                {
                    m_from = new Vector3(- halfW - boundary.right, 0, 0);
                    break;
                }
        }
    }
}
