using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int _index;
    public bool _isFree;

    void Start()
    {
        _index = int.Parse(gameObject.name);
    }

    public void _onClickMe()
    {
        if (_isFree && !GameManager.Instance._currentPlayer._isAI)
        {
            GameManager.Instance._currentPlayer._playManual(this);
        }
    }

}
