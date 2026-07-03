using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 人物移动
{
    public class KeyInputTrigger
    {
        public static KeyInputTrigger Instance { get; } = new KeyInputTrigger();

        public NonRepetitiveAdjQueue<KeyWrapper> InputKeys { get; } = new NonRepetitiveAdjQueue<KeyWrapper>(); 
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
                while (!InputKeys.IsEmpty && popCnt < 10)
                {
                    InputKeys.TryDequeue(out var key);
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
    }
}
