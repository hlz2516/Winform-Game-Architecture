using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 人物移动
{
    public interface IRecvKeyInput
    {
        void GetCurrentKeyInput(KeyWrapper key);
    }
}
