using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DragObj : MonoBehaviour {

    public enum MoveDir
    {
        None,
        Left,
        Right
    }

    public int ID = 1;
    public Transform MoveTarget;
    public float MoveDistance;
    public float MoveTime = 1;
    public int MaxStep = 0;
    public int MinStep = 0;

    public delegate bool CheckMoveStartConditionHandler(int id, int step);
    public CheckMoveStartConditionHandler CheckMoveStartConditionEvent;
    public delegate void OnMoveEndHandler(int id, int step);
    public OnMoveEndHandler OnMoveEndEvent;

    private List<float> _dragDeltas = new List<float>();
    private MoveDir _moveDir = MoveDir.None;
    private bool _startMove = false;
    private Vector3 _targetPos;
    private Vector3 _originalPos;
    private bool _moveCompleted = true;
    private float _factor = 0;
    private int CurStep = 0;
    private int _moveStep = 0;

    void OnEnable()
    {
        UIEventListener.Get(gameObject).onDrag += Drag;
        UIEventListener.Get(gameObject).onDragEnd += DragEnd;
    }

    void OnDisable()
    {
        UIEventListener.Get(gameObject).onDrag -= Drag;
        UIEventListener.Get(gameObject).onDragEnd -= DragEnd;
    }

    void Update () {
        
        if(!_startMove)
            return;

	    _factor += Time.deltaTime;

        MoveTarget.localPosition = _originalPos + (_targetPos - _originalPos) * Mathf.Clamp01(_factor / MoveTime);

        if (_factor >= MoveTime)
        {
            MoveEnd();
        }
	}

    public void MoveByDrag()
    {
        switch (_moveDir)
        {
                case MoveDir.Right:
                {
                    _moveStep = -1;
                    StartMove();
                    break;
                }
                case MoveDir.Left:
                {
                    _moveStep = 1;
                    StartMove();
                    break;
                }
                //case MoveDir.None:
                //{
                //    dir = 0;
                //    break;
                //}
        }

    }

    public bool MoveByStep(int step ,bool immediately = false)
    {
        if (!CheckDragCondition())
            return false;

        if (CurStep + step < MaxStep && CurStep + step >= MinStep)
        {
            _moveStep = step;
            StartMove(immediately);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartMove(bool immediately = false)
    {
        if (!CheckMoveStepLimit())
            return;

        if (!CheckMoveStartCondition())
            return;

        CurStep = CurStep + _moveStep;
        _targetPos = MoveTarget.localPosition + new Vector3(-_moveStep * MoveDistance, 0, 0);
        _originalPos = MoveTarget.localPosition;
        if (!immediately )
        {
            _startMove = true;
            _moveCompleted = false;
        }
        else
        {
            MoveTarget.localPosition = _targetPos;
            MoveEnd();
        }
    }

    private void MoveEnd()
    {
        if (OnMoveEndEvent != null)
        {
            OnMoveEndEvent(ID, _moveStep);
        }

        _factor = 0;
        _startMove = false;
        _moveCompleted = true;
        _moveStep = 0;
    }

    private bool CheckMoveStepLimit()
    {
        if (CurStep + _moveStep < MaxStep && CurStep + _moveStep >= MinStep)
            return true;

        return false;
    }

    private bool CheckMoveStartCondition()
    {
        if (CheckMoveStartConditionEvent != null)
        {
            return CheckMoveStartConditionEvent(ID,_moveStep);
        }

        return true;
    }

    private bool CheckDragCondition()
    {
        if (!MoveTarget.gameObject.activeSelf)
        {
            return false;
        }

        if (!_moveCompleted)
        {
            return false;
        }
        
        return true;
    }

    public void Drag(GameObject btn, Vector2 delta)
    {
        if (!CheckDragCondition())
            return;

        _dragDeltas.Add(delta.x);
    }

    public void DragEnd(GameObject btn)
    {

        if (!CheckDragCondition())
            return;

        float sum = 0;
        for (int i = 0; i < _dragDeltas.Count; i++)
        {
            sum += _dragDeltas[i];
        }
        float ave = sum / _dragDeltas.Count;

        if (ave > 20)
        {
            _moveDir = MoveDir.Right;
        }
        else if (ave < -20)
        {
            _moveDir = MoveDir.Left;
        }
        else
        {
            _moveDir = MoveDir.None;
        }
        _dragDeltas.Clear();
        MoveByDrag();
    }

}
