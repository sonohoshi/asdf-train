using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ContainerEx
{
    private static readonly System.Random random = new System.Random();

    #region Contains

    public static bool NotContainsKey<TKey, TValue>(this IDictionary<TKey,TValue> _dict,TKey _key)
    {
        return !_dict.ContainsKey(_key);
    }

    public static bool NotContains<TValue>(this IList<TValue> _list,TValue _value)
    {
        return !_list.Contains(_value);
    }

    public static bool ContainsIndex<TValue>(this IEnumerable<TValue> _sources,int _idx)
    {
        return (0<=_idx && _idx<_sources.Count());
    }

    #endregion Contains

    #region Remove

    public static bool RemoveSafe<TKey, TValue>(this IDictionary<TKey,TValue> _dict,TKey _key)
    {
        return _dict.ContainsKey(_key)&&_dict.Remove(_key);
    }

    public static bool Remove<TKey, TValue>(this IDictionary<TKey,TValue> _dict,TKey _key,out TValue _value)
    {
        return _dict.TryGetValue(_key,out _value);
    }

    public static bool RemoveSafeValueInList<TKey, TValue>(this IDictionary<TKey,ICollection<TValue>> _dict,TKey _key,TValue _value)
    {
        return _dict.ContainsKey(_key)&&_dict[_key].Remove(_value);
    }

    public static bool RemoveAt<TValue>(this ICollection<TValue> _sources,int _idx,out TValue _value)
    {
        _value = _sources.ElementAt(_idx);

        return _sources.Remove(_value);
    }

    #endregion Remove

    #region Find

    public static bool TryGetValue<TValue>(this IEnumerable<TValue> _sources,int _idx,out TValue _value)
    {
        if(0<=_idx && _idx<_sources.Count())
        {
            _value = _sources.ElementAt(_idx);

            return true;
        }
        else
        {
            _value = default;

            return false;
        }
    }

    public static int FindIndex<TValue>(this IEnumerable<TValue> _sources,Func<TValue,bool> _predicate)
    {
        for(var i=0;i<_sources.Count();i++)
        {
            if(_predicate(_sources.ElementAt(i)))
            {
                return i;
            }
        }

        return -1;
    }

    public static List<TValue> FindAll<TKey, TValue>(this IDictionary<TKey,TValue> _dict,Func<TValue,bool> _predicate)
    {
        var values = new List<TValue>();

        foreach(var value in _dict.Values)
        {
            if(_predicate(value))
            {
                values.Add(value);
            }
        }

        return values;
    }
    
    public static bool FindAll<TKey, TValue>(this IDictionary<TKey,TValue> _dict,Func<TValue,bool> _predicate,out List<TValue> _list)
    {
        _list = new List<TValue>();

        foreach(var value in _dict.Values)
        {
            if(_predicate(value))
            {
                _list.Add(value);
            }
        }

        return _list.Count != 0;
    }

    public static List<TValue> FindAllByKey<TKey, TValue>(this IDictionary<TKey,TValue> _dict,Func<TKey,bool> _predicate)
    {
        var datas = new List<TValue>();

        foreach(var pair in _dict)
        {
            if(_predicate(pair.Key))
            {
                datas.Add(pair.Value);
            }
        }

        return datas;
    }
    
    public static bool FindAllByKey<TKey, TValue>(this IDictionary<TKey,TValue> _dict,Func<TKey,bool> _predicate,out List<TValue> _list)
    {
        _list = new List<TValue>();

        foreach(var pair in _dict)
        {
            if(_predicate(pair.Key))
            {
                _list.Add(pair.Value);
            }
        }

        return _list.Count != 0;
    }

    public static TValue FindValue<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,bool> _match)
    {
        foreach(var datas in _dict.Values)
        {
            foreach(var data in datas)
            {
                if(_match(data))
                {
                    return data;
                }
            }
        }

        return default;
    }

    public static bool FindValue<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,bool> _match,out TValue _value)
    {
        foreach(var datas in _dict.Values)
        {
            foreach(var data in datas)
            {
                if(_match(data))
                {
                    _value=data;

                    return true;
                }
            }
        }

        _value = default;

        return false;
    }

    public static KeyValuePair<TKey,TValue> FindPair<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,bool> _match)
    {
        foreach(var pair in _dict)
        {
            foreach(var data in pair.Value)
            {
                if(_match(data))
                {
                    return new KeyValuePair<TKey,TValue>(pair.Key,data);
                }
            }
        }

        return default;
    }

    public static bool FindPair<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,bool> _match,out KeyValuePair<TKey,TValue> _pair)
    {
        foreach(var pair in _dict)
        {
            foreach(var data in pair.Value)
            {
                if(_match(data))
                {
                    _pair = new KeyValuePair<TKey,TValue>(pair.Key,data);

                    return true;
                }
            }
        }

        _pair = default;

        return false;
    }
    #endregion Find

    #region Get

    public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey,TValue> _dict,TKey _key) where TValue : new()
    {
        if(_dict.NotContainsKey(_key))
        {
            _dict.Add(_key,new TValue());
        }

        return _dict[_key];
    }

    public static TValue GetNearValue<TKey, TValue>(this IDictionary<TKey,TValue> _dict,TKey _key,int _near)
    {
        var keys = _dict.Keys.ToList();

        for(var i=0;i<keys.Count;i++)
        {
            if(keys[i].Equals(_key))
            {
                var idx = Mathf.Clamp(i+_near,0,keys.Count-1);

                return _dict[keys[idx]];
            }
        }

        return default;
    }

    public static TKey GetKeyByIdx<TKey, TValue>(this IDictionary<TKey,TValue> _dict,int _idx)
    {
        return _dict.Keys.Count>_idx && _idx>=0 ? _dict.Keys.ElementAt(_idx) : default;
    }

    public static TValue GetValueByIdx<TKey, TValue>(this IDictionary<TKey,TValue> _dict,int _idx)
    {
        return _dict.Values.Count>_idx && _idx>=0 ? _dict.Values.ElementAt(_idx) : default;
    }

    public static KeyValuePair<TKey,TValue> GetPairByIdx<TKey, TValue>(this IDictionary<TKey,TValue> _dict,int _idx)
    {
        return _dict.Keys.Count>_idx && _idx>=0 ? new KeyValuePair<TKey,TValue>(_dict.Keys.ElementAt(_idx),_dict.Values.ElementAt(_idx)) : default;
    }

    public static KeyValuePair<TKey,TValue> GetMaxPair<TKey, TValue>(this IDictionary<TKey,TValue> _dict)
    {
        return new KeyValuePair<TKey,TValue>(_dict.Keys.Max(),_dict.Values.Max());
    }

    public static bool? GetAsBool<TKey>(this IDictionary<TKey,object> _dict,TKey _key)
    {
        return _dict.TryGetValue(_key,out var value) ? value as bool? : null;
    }

    public static int? GetAsInt<TKey>(this IDictionary<TKey,object> _dict,TKey _key)
    {
        return _dict.TryGetValue(_key,out var value) ? value as int? : null;
    }

    public static float? GetAsFloat<TKey>(this IDictionary<TKey,object> _dict,TKey _key)
    {
        return _dict.TryGetValue(_key,out var value) ? value as float? : null;
    }

    public static string GetAsString<TKey>(this IDictionary<TKey,object> _dict,TKey _key)
    {
        return _dict.TryGetValue(_key,out var value) ? value as string : null;
    }

    #endregion Get

    #region Add

    public static void AddRange<TKey, TValue>(this IDictionary<TKey,TValue> _dict1,IDictionary<TKey,TValue> _dict2)
    {
        foreach(var pair in _dict2)
        {
            _dict1.Add(pair.Key,pair.Value);
        }
    }

    public static void AddRange<TKey, TValue>(this IDictionary<TKey,TValue> _dict,IEnumerable<TValue> _values,Func<TValue,TKey> _key)
    {
        foreach(var value in _values)
        {
            var key = _key(value);

            if(key != null)
            {
                _dict.Add(key,value);
            }
        }
    }

    public static void AddRange<TValue>(this IDictionary<string,TValue> _dict,IEnumerable<TValue> _values,Action _option = null) where TValue : UnityEngine.Object
    {
        foreach(var value in _values)
        {
            _option?.Invoke();

            _dict.Add(value.name,value);
        }
    }

    public static void AddRange<TValue>(this IDictionary<string,TValue> _dict,IEnumerable<TValue> _values,Action<TValue> _option) where TValue : UnityEngine.Object
    {
        foreach(var value in _values)
        {
            _option?.Invoke(value);

            _dict.Add(value.name,value);
        }
    }

    public static void AddRange<TValue, UValue>(this IDictionary<string,TValue> _dict1,IDictionary<string,UValue> _dict2,Func<UValue,TValue> _option) where TValue : UnityEngine.Object
    {
        foreach(var pair in _dict2)
        {
            var data = _option(pair.Value);

            if(data != null)
            {
                _dict1.Add(data.name,data);
            }
        }
    }

    public static void AddOrCreate<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,TKey _key,TValue _value)
    {
        if(_dict.NotContainsKey(_key))
        {
            _dict.Add(_key,new List<TValue>());
        }

        _dict[_key].Add(_value);
    }

    public static void AddOrCreate<TKey, TValue>(this IDictionary<TKey,Queue<TValue>> _dict,TKey _key,TValue _value)
    {
        if(_dict.NotContainsKey(_key))
        {
            _dict.Add(_key,new Queue<TValue>());
        }

        _dict[_key].Enqueue(_value);
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey,TValue> _dict,TKey _key,TValue _value)
    {
        if(_dict.ContainsKey(_key))
        {
            _dict[_key] = _value;
        }
        else
        {
            _dict.Add(_key,_value);
        }
    }

    public static bool AddNotOverlap<TValue>(this IList<TValue> _list,TValue _value)
    {
        if(_list.NotContains(_value))
        {
            _list.Add(_value);

            return true;
        }

        return false;
    }

    #endregion Add

    #region Sort

    public static void SortEachList<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,TKey> _predicate)
    {
        foreach(var pair in _dict)
        {
            _dict[pair.Key] = pair.Value.OrderBy(_predicate).ToList();
        }
    }

    #endregion Sort

    #region Merge

    public static IEnumerable<TValue> MergeAllValues<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,params TKey[] _exceptions)
    {
        var values = new List<TValue>(_dict.Count);

        foreach(var pair in _dict)
        {
            if(_exceptions == null || !_exceptions.Any(x=>x.Equals(pair.Key)))
            {
                foreach(var data in pair.Value)
                {
                    values.Add(data);
                }
            }
        }

        return values;
    }

    public static IEnumerable<TValue> MergeAllValues<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,bool> _predicate)
    {
        var values = new List<TValue>(_dict.Count);

        foreach(var pair in _dict)
        {
            foreach(var data in pair.Value)
            {
                if(_predicate(data))
                {
                    values.Add(data);
                }
            }
        }

        return values;
    }

    public static int MergeCount<TKey, TValue>(this IDictionary<TKey,List<TValue>> _dict,Func<TValue,bool> _predicate = null)
    {
        var count = 0;

        foreach(var pair in _dict)
        {
            if(_predicate != null)
            {
                count += pair.Value.Count(_predicate);
            }
            else
            {
                count += pair.Value.Count();
            }
        }

        return count;
    }

    #endregion Merge

    #region Exist



    public static bool Exist<TValue>(this IEnumerable<TValue> _sources,Func<TValue,bool> _predicate)
    {
        for(int i=0;i<_sources.Count();i++)
        {
            if(_predicate(_sources.ElementAt(i)))
            {
                return true;
            }
        }

        return false;
    }

    public static bool NotExist<TValue>(this IEnumerable<TValue> _sources,Func<TValue,bool> _predicate)
    {
        for(int i=0;i<_sources.Count();i++)
        {
            if(_predicate(_sources.ElementAt(i)))
            {
                return false;
            }
        }

        return true;
    }

    #endregion Exist

    #region Each

    public static void Each<TValue>(this IEnumerable<TValue> _sources,Action<TValue,int> _action)
    {
        var i=0;

        foreach(var source in _sources)
        {
            _action(source,i++);
        }
    }

    public static void Each<TValue>(this IEnumerable<TValue> _sources,Action<TValue> _action)
    {
        foreach(var source in _sources)
        {
            _action(source);
        }
    }

    #endregion Each

    #region Condition

    public static bool IsCondition<TValue>(this IEnumerable<TValue> _sources,Func<TValue,bool> _predicate)
    {
        foreach(var source in _sources)
        {
            if(_predicate(source))
            {
                return true;
            }
        }

        return false;
    }

    #endregion Condition

    #region Random

    public static TValue GetRndElement<TValue>(this IEnumerable<TValue> _sources)
    {
        return _sources.ElementAt(random.Next(0,_sources.Count()));
    }

    #endregion Random

    #region IsNullOrEmpty

    public static bool IsNullOrEmpty<TValue>(this IEnumerable<TValue> _sources)
    {
        return null == _sources || _sources.Count() < 1;
    }
    
    public static bool IsOk<TValue>(this IEnumerable<TValue> _sources)
    {
        return !(IsNullOrEmpty(_sources));
    }

    #endregion IsNullOrEmpty

    #region Shuffle

    public static List<T> ShuffleList<T>(this IList<T> _sources)
    {
        var resultList = new List<T>();

        while(_sources.Count > 0)
        {
            var idx = random.Next(_sources.Count);
            resultList.Add(_sources.ElementAt(idx));
            _sources.RemoveAt(idx);
        }

        return resultList;
    }

    #endregion Shuffle
}