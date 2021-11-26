using System;
using System.Collections.Generic;

namespace KZLib.Develop
{
    static internal class BroadcasterInner
    {
        private static readonly Dictionary<string,Delegate> m_ListenerDict = new Dictionary<string,Delegate>();

        public static void RegistListener(string _name,Delegate _delegate)
        {
            if(_delegate != null)
            {
                if(m_ListenerDict.TryGetValue(_name,out var data))
                {
                    if(data == null)
                    {
                        m_ListenerDict[_name] = _delegate;
                    }
                    else if(data != null && data.GetType() == _delegate.GetType())
                    {
                        m_ListenerDict[_name] = Delegate.Combine(data,_delegate);
                    }
                }
                else
                {
                    m_ListenerDict.Add(_name,_delegate);
                }
            }
        }

        public static void RemoveListener(string _name,Delegate _delegate)
        {
            if(_delegate != null)
            {
                if(m_ListenerDict.TryGetValue(_name,out var data))
                {
                    if(data.GetType() == _delegate.GetType())
                    {
                        m_ListenerDict[_name] = Delegate.Remove(data,_delegate);
                    }

                    if(data == null)
                    {
                        m_ListenerDict.Remove(_name);
                    }
                }
            }
        }

        public static Delegate GetDelegate(string _name)
        {
            return m_ListenerDict.TryGetValue(_name,out var data) ? data : null;
        }
    }

    public delegate void BroadcastCallBack();

    public static class Broadcaster
    {
        public static void EnableListener(string _name,BroadcastCallBack _method)
        {
            BroadcasterInner.RegistListener($"[BC]_{_name}",_method);
        }

        public static void DisableListener(string _name,BroadcastCallBack _method)
        {
            BroadcasterInner.RemoveListener($"[BC]_{_name}",_method);
        }

        public static void SendEvent(string _name)
        {
            var data = BroadcasterInner.GetDelegate($"[BC]_{_name}");

            if(data != null && data is BroadcastCallBack)
            {
                (data as BroadcastCallBack)?.Invoke();
            }
        }
    }

    public delegate void BroadcastCallBack<T>(T _value);

    public static class Broadcaster<T>
    {
        public static void EnableListener(string _name,BroadcastCallBack<T> _method)
        {
            BroadcasterInner.RegistListener($"[BCT]_{_name}",_method);
        }

        public static void DisableListener(string _name,BroadcastCallBack<T> _method)
        {
            BroadcasterInner.RemoveListener($"[BCT]_{_name}",_method);
        }

        public static void SendEvent(string _name,T _value1)
        {
            var data = BroadcasterInner.GetDelegate($"[BCT]_{_name}");

            if(data != null && data is BroadcastCallBack<T>)
            {
                (data as BroadcastCallBack<T>)?.Invoke(_value1);
            }
        }
    }

    public delegate void BroadcastCallBack<T, U>(T _value1,U _value2);

    public static class Broadcaster<T, U>
    {
        public static void EnableListener(string _name,BroadcastCallBack<T,U> _method)
        {
            BroadcasterInner.RegistListener($"[BCTU]_{_name}",_method);
        }

        public static void DisableListener(string _name,BroadcastCallBack<T,U> _method)
        {
            BroadcasterInner.RemoveListener($"[BCTU]_{_name}",_method);
        }

        public static void SendEvent(string _name,T _value1,U _value2)
        {
            var data = BroadcasterInner.GetDelegate($"[BCTU]_{_name}");

            if(data != null && data is BroadcastCallBack<T,U>)
            {
                (data as BroadcastCallBack<T,U>)?.Invoke(_value1,_value2);
            }
        }
    }
}