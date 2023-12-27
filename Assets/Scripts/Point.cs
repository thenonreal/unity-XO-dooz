using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Point : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int _currentCell;
    public bool _canClick;
    public Player _myPlayer;
    public Color _myColor;
    public Image _myImage;
    Cell _enterCell;

    public void _init()
    {
        _currentCell = -1;
        _myPlayer = GetComponentInParent<Player>();
        _myImage = GetComponent<Image>();
        _myImage.color = _myColor;
        _enterCell = null;
        if (_myPlayer._isAI)
            _canClick = false;
    }

    public void _move(int _target, bool _isAuto)
    {
        transform.position = GameManager.Instance._allCells[_target].transform.position;
        if (_isAuto) GameManager.Instance._allCells[_currentCell]._isFree = true;
        GameManager.Instance._allCells[_target]._isFree = false;
        _currentCell = _target;
    }

    public void _playMyRound(_Task _task)
    {
        _move(_task._target, true);
        if(!_myPlayer._checkWin())
            GameManager.Instance._nextRound();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canClick && collision.tag == "cell")
        {
            _enterCell = collision.GetComponent<Cell>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (_canClick && collision.tag == "cell")
        {
            _enterCell = null;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (!_myPlayer._isAI && _canClick && GameManager.Instance._currentPlayer == _myPlayer && !GameManager.Instance._isWin)
        {
            Vector2 _mousePos = Input.mousePosition;
            transform.position = _mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(_enterCell != null && _enterCell != GameManager.Instance._allCells[_currentCell] && _enterCell._isFree)
        {
            GameManager.Instance._allCells[_currentCell]._isFree = true;
            _currentCell = int.Parse(_enterCell.name);
            _playMyRound(new _Task(transform.GetSiblingIndex(), _currentCell));
        }
        else
        {
            transform.position = GameManager.Instance._allCells[_currentCell].transform.position;
        }
    }

}
