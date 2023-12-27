using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public List<Player> _allPlayers;

    public void init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Player>() != null)
            {
                _allPlayers.Add(transform.GetChild(i).GetComponent<Player>());
            }
        }
    }

    public Point _createPoint(Player _player, bool _isAuto)
    {
        Point _tmp = Instantiate(GameManager.Instance._pointPre, _player.transform).GetComponent<Point>();
        _tmp._myColor = _player._color;
        if(_isAuto) _tmp._init();
        return _tmp;
    }

    public int _validTas()
    {
        int _tmpR2 = Random.Range(0, GameManager.Instance._allCells.Count);
        if (GameManager.Instance._allCells[_tmpR2]._isFree)
            return _tmpR2;
        else
            return _validTas();
    }
}