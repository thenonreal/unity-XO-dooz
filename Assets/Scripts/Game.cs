using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="data",menuName ="New/Data")]
public class Game : ScriptableObject
{
    public int _maxPoint;
    public List<PlayerData> _players;
}
