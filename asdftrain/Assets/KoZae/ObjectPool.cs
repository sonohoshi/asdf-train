using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
    public class ObjectPool : MonoBehaviour
    {
        #region PoolData

        public class PoolData
        {
            public long Duration { get; private set; }
            public Object Resource { get; private set; }

            public PoolData(long _duration,Object _resource)
            {
                Duration = _duration;
                Resource = _resource;
            }

            public void ResetDuration()
            {
                Duration = 0;
            }
        }

        #endregion

        private const double DELETE_TIME = 60000;   // 60초
        private readonly Dictionary<string,List<PoolData>> m_PoolDict = new Dictionary<string,List<PoolData>>();

        private readonly List<Object> m_DestoryList                   = new List<Object>();

        private readonly List<PoolData> m_RemoveList                  = new List<PoolData>();
        private readonly List<string> m_EmptyList                     = new List<string>();

        private Transform m_Storage                                   = null;

        private void Start()
        {
            StartCoroutine(CheckLiftTime_CR());

            var data = new GameObject("Pool_Storage");

            data.transform.parent = transform;
            m_Storage = data.transform;
        }

        private IEnumerator CheckLiftTime_CR()
        {
            while(true)
            {
                foreach(var pair in m_PoolDict)
                {
                    m_RemoveList.Clear();

                    foreach(var data in pair.Value)
                    {
                        if(data.Duration > 0 && data.Duration < System.DateTime.Now.Ticks)
                        {
                            if(data.Resource is GameObject)
                            {
                                m_DestoryList.Add(data.Resource);
                            }

                            m_RemoveList.Add(data);
                        }
                    }

                    pair.Value.RemoveAll(m_RemoveList.Contains);

                    if(pair.Value.Count == 0)
                    {
                        m_EmptyList.Add(pair.Key);
                    }
                }

                foreach(var empty in m_EmptyList)
                {
                    if(m_PoolDict.ContainsKey(empty))
                    {
                        m_PoolDict.Remove(empty);
                    }
                }

                ClearDestoryList();

                yield return new WaitForSeconds(30.0f);
            }
        }

        public void Release()
        {
            foreach(var pair in m_PoolDict)
            {
                m_RemoveList.Clear();

                foreach(var data in pair.Value)
                {
                    if(data.Resource is GameObject)
                    {
                        m_DestoryList.Add(data.Resource);
                    }
                }

                pair.Value.Clear();

                m_EmptyList.Add(pair.Key);
            }

            foreach(var empty in m_EmptyList)
            {
                m_PoolDict.RemoveSafe(empty);
            }

            m_PoolDict.Clear();

            Destroy(m_Storage);

            ClearDestoryList();
        }

        private void ClearDestoryList()
        {
            foreach(var data in m_DestoryList)
            {
                Destroy(data);
            }

            m_DestoryList.Clear();
        }

        public T Get<T>(string _name) where T : Object
        {
            if (m_PoolDict.TryGetValue($"[{typeof(T).Name}] {_name}", out var datas) && datas.Count > 0 && datas.RemoveAt(0,out var data))
            {
                data.ResetDuration();

                return (T) data.Resource;
            }

            return null;
        }

        public void Put(Object _object,double _duration = DELETE_TIME)
        {
            if(_object is GameObject data)
            {
                var scale = data.transform.localScale;

                data.transform.SetParent(m_Storage);
                data.transform.localScale = scale;
                data.SetActive(false);
            }

            m_PoolDict.AddOrCreate($"[{_object.GetType().Name}] {_object.name}", new PoolData(System.DateTime.Now.AddMilliseconds(_duration).Ticks, _object));
        }
    }
}