using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] Game _data;
    public static GameManager Instance;
    public Board _board;
    public List<Player> _allPlayers;
    public List<Cell> _allCells;
    public GameObject _playerPre, _pointPre;
    public Player _currentPlayer;
    public int _maxPoint;
    public UnityAction<Player> _win;
    public bool _isWin;


    void Awake()
    {
        Instance = this;
        _win += _winner;
    }

    void Start()
    {
        init();
        for (int i = 0; i < _data._players.Count; i++)
        {
            Player _tmpPlayer = _createPlayer();
            _tmpPlayer._isAI = _data._players[i]._isAI;
            _tmpPlayer.init(_data._players[i]._playerName, _data._players[i]._color);
            _allPlayers.Add(_tmpPlayer);
        }
        _allPlayers[_round]._myRound();
    }

    void init()
    {
        _board = GameObject.Find("Board").GetComponent<Board>();
        _board.init();
        _allPlayers = _board._allPlayers;
        _maxPoint = _data._maxPoint;

        Transform _tmp = _board.transform.Find("Cells");
        for (int i = 0; i < _tmp.childCount; i++)
        {
            _allCells.Add(_tmp.transform.GetChild(i).GetComponent<Cell>());
        }
    }

    Player _createPlayer()
    {
        Player player = Instantiate(_playerPre, _board.transform).GetComponent<Player>();
        return player;
    }

    public int _round = 0;
    public void _nextRound()
    {
        if(!_isWin)
            StartCoroutine(_IE_nextRun());
    }

    IEnumerator _IE_nextRun()
    {
        yield return new WaitForSeconds(.2f);
        _round++;
        if (_round >= _allPlayers.Count) _round = 0;
        _allPlayers[_round]._myRound();
    }

    void _winner(Player _plr)
    {
        _isWin = true;
        Debug.Log($"player {_plr._playerName} is win!");
    }

}

[Serializable]
public class _Task
{
    public int _point;
    public int _target;

    public _Task() { }

    public _Task(int point, int target)
    { 
        _point = point;
        _target = target;
    }

    public void _initNewPoint(Player _plr)
    {
        Point _tmpP = GameManager.Instance._board._createPoint(_plr, true);
        _plr._myPoints.Add(_tmpP);
        int _tas = GameManager.Instance._board._validTas();
        _tmpP._init();
        _tmpP._currentCell = _tas;
        _tmpP._playMyRound(new _Task(_tmpP.transform.GetSiblingIndex(), _tas));
    }
}
