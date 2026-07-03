using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 人物移动
{
    public class NonRepetitiveAdjQueue<T> : ConcurrentQueue<T>
    {
        /// <summary>
        /// 如果上一个入队的元素跟当前要入队的元素相同，那么不入
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueWithSamePrevCheck(T item)
        {
            if (Count == 0)
            {
                Enqueue(item);
                return;
            }
            if (this.Last().Equals(item))
                return;
            Enqueue(item);
        }
    }
}
