using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreLib
{
    public class KeyWrapper
    {
        public KeyState State { get; }
        public KeyEventArgs Key { get; }
        public Keys KeyCode { get; }
        public KeyWrapper(KeyEventArgs key,KeyState state)
        {
            Key = key;
            KeyCode = key.KeyCode;
            State = state;
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyWrapper otherKey)
            {
                return State == otherKey.State && KeyCode == otherKey.KeyCode;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum KeyState
    {
        KeyPressed,
        KeyUp,
    }
}
