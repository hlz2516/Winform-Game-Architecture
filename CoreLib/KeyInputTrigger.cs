using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CoreLib
{
    public class KeyInputTrigger
    {
        public static KeyInputTrigger Instance { get; } = new KeyInputTrigger();
        private static object lockObj = new object();
        private ConcurrentQueue<KeyWrapper> inputKeys = new ConcurrentQueue<KeyWrapper>(); 
        //对于同一个键，可以支持绑定多个action
        private Dictionary<Keys, List<Action<KeyWrapper>>> keyActions = new Dictionary<Keys, List<Action<KeyWrapper>>>();
        private Thread keyProcessThread;
        private KeyInputTrigger()
        {
            keyProcessThread = new Thread(KeyProcess);
            keyProcessThread.IsBackground = true;
            keyProcessThread.Start();
        }

        private void KeyProcess()
        {
            while (true)
            {
                int popCnt = 0;
                while (!inputKeys.IsEmpty && popCnt < 10)
                {
                    inputKeys.TryDequeue(out var key);
                    if (key != null)
                    {
                        if (keyActions.ContainsKey(key.KeyCode))
                        {
                            foreach (var action in keyActions[key.KeyCode])
                            {
                                action.Invoke(key);
                            }
                        }
                    }
                    popCnt++;
                }
                Thread.Sleep(30);
            }
        }
        /// <summary>
        /// 如果上一个入队的元素跟当前要入队的元素相同，那么不入
        /// </summary>
        /// <param name="key"></param>
        public void InputKey(KeyWrapper key)
        {
            //inputKeys.Enqueue(key);
            lock (lockObj)
            {
                if (inputKeys.Count == 0)
                {
                    inputKeys.Enqueue(key);
                    return;
                }
                if (inputKeys.Last().Equals(key))
                    return;
                inputKeys.Enqueue(key);
            }
        }

        public void Register(Keys key,Action<KeyWrapper> action)
        {
            
            if (keyActions.ContainsKey(key))
            {
                keyActions[key].Add(action);
            }
            else
            {
                keyActions[key] = new List<Action<KeyWrapper>> { action };
            }
        }

        public void Register(Action<KeyWrapper> action,params Keys[] keys)
        {
            foreach (var key in keys)
            {
                Register(key, action);
            }
        }
    }
}
