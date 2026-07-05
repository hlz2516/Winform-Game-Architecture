using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IRecvKeyInput
    {
        void GetCurrentKeyInput(KeyWrapper key);
    }
}
