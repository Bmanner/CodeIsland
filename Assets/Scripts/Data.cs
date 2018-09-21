using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

[Serializable]
public class PlayerData
{
    public TimeSpan TotalPlayTime;
    public StageDictionary stageDictionary = new StageDictionary();
}

[Serializable]
public class StageDictionary : SerializableDictionary<string, int>
{
    public StageDictionary() : base() { }
    protected StageDictionary(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
}

public class SerializableDictionary<TK, TV> : Dictionary<TK, TV>, ISerializationCallbackReceiver
{
    [SerializeField] List<TK> _keys = new List<TK>();
    [SerializeField] List<TV> _values = new List<TV>();

    public SerializableDictionary() : base() { }
    protected SerializableDictionary(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (KeyValuePair<TK, TV> pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (_keys.Count != _values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < _keys.Count; i++)
            this.Add(_keys[i], _values[i]);
    }
}

/* 나중에 스테이지가 최고 점수 외에 다른 값을 포함하게 될 경우 사용한다.
public class StageData : ISerializable
{
    private bool _isUnlock;
    private short _score;

    public StageData(bool unlock, short score, int stage)
    {

    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("IsUnlock", this._isUnlock);
        info.AddValue("Score", this._score);
        info.AddValue("StageNumber", this._stageNo);
    }
}
*/
