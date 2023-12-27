using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public bool _isAI;
    public List<Point> _myPoints;
    public Color _color;
    public string _playerName;

    public void init(string name, Color color)
    {
        _playerName = name;
        _color = color;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Point>() != null)
                _myPoints.Add(transform.GetChild(i).GetComponent<Point>());
        }
    }

    public void _myRound()
    {
        GameManager.Instance._currentPlayer = this;
        _setCanClick();
        if (_isAI)
            _playAuto();
    }

    void _playAuto()
    {
        List<_Task> _tmp = _checkTaskToPlay();
        if (_tmp.Count == 0)
        {
            _tmp.Add(new _Task());
            _tmp[0]._initNewPoint(this);
        }
        else
        {
            List<_Task> _t = _tmp;
            int _r = Random.Range(0, _tmp.Count);
            _myPoints[_r]._playMyRound(_t[_r]);
        }

    }

    public void _playManual(Cell _cell)
    {
        if (_myPoints.Count < GameManager.Instance._maxPoint)
        {
            _cell._isFree = false;
            Point _tmpP = GameManager.Instance._board._createPoint(GameManager.Instance._currentPlayer, false);
            _tmpP._move(_cell._index, false);
            _tmpP._init();
            _tmpP._currentCell = _cell._index;
            _myPoints.Add(_tmpP);
            if(!_checkWin())
                GameManager.Instance._nextRound();
        }
    }

    public void _setCanClick()
    {
        if (!_isAI && _myPoints.Count == GameManager.Instance._maxPoint)
        {
            for (int i = 0; i < _myPoints.Count; i++)
            {
                _myPoints[i]._canClick = true;
            }
        }
    }

    public bool _checkWin()
    {
        if (_checkV() || _checkH() || _crossCheck(4, 0, 0) || _crossCheck(2, -2, 2))
        {
            GameManager.Instance._win.Invoke(this);
            return true;
        }
        
        return false;
    }

    bool _checkV()
    {
        int _mxPlayer = GameManager.Instance._maxPoint;
        if (_myPoints.Count == _mxPlayer)
        {
            for (int i = 0; i < _mxPlayer; i++)
            {
                int _counter = 0;
                for (int j = 0; j < (_mxPlayer * _mxPlayer); j += 3)
                {
                    int _res = i + j;
                    for (int k = 0; k < _myPoints.Count; k++)
                    {
                        if (_myPoints[k]._currentCell == _res)
                            _counter++;
                    }
                    if (_counter == 3)
                        return true;
                }
            }
        }
        return false;
    }
    
    bool _checkH()
    {
        int _mxPlayer = GameManager.Instance._maxPoint;
        if (_myPoints.Count == _mxPlayer)
        {
            for (int i = 0; i < (_mxPlayer * _mxPlayer); i+=3)
            {
                int _counter = 0;
                for (int j = i; j < i+_mxPlayer; j++)
                {
                    int _res = j;
                    for (int k = 0; k < _myPoints.Count; k++)
                    {
                        if (_myPoints[k]._currentCell == _res)
                            _counter++;
                    }
                    if (_counter == _mxPlayer)
                    return true;
                }
            }
        }
        return false;
    }

    bool _crossCheck(int _value, int _delta, int _start)
    {
        int _mxPlayer = GameManager.Instance._maxPoint;
        if (_myPoints.Count == _mxPlayer)
        {
            int _counter = 0;
            for (int i = _start; i < ((_mxPlayer * _mxPlayer) + _delta); i += _value)
            {
                //Debug.Log(i);
                for (int k = 0; k < _myPoints.Count; k++)
                {
                    if (_myPoints[k]._currentCell == i)
                        _counter++;
                }
                if (_counter == _mxPlayer)
                    return true;
            }
        }
        return false;
    }

    public List<_Task> _checkTaskToPlay()
    {
        List<_Task> _tmp = new List<_Task>();

        if (_myPoints.Count >= GameManager.Instance._maxPoint)
        {
            for (int i = 0; i < _myPoints.Count; i++)
            {
                int _r = GameManager.Instance._board._validTas();
                _tmp.Add(new _Task(i, _r));
            }
        }
        return _tmp;
    }
}


[Serializable]
public class PlayerData
{
    public bool _isAI;
    public Color _color;
    public string _playerName;
}