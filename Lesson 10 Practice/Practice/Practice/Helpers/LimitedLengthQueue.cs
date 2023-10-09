using System;
using System.Collections.ObjectModel;

namespace Practice.Helpers
{
    public class LimitedLengthQueue<T> : PracticeQueue<T>
    {
        protected int MaxLength;

        public LimitedLengthQueue(int length)
        {
            MaxLength = length;
        }

        /// <summary>
        /// 如果当前队列达到最大长度，则移出开头对象，队列尾部加入新的对象
        /// </summary>
        /// <param name="item"></param>
        public override void Enqueue(T item)
        {
            if (Count == MaxLength)
            {
                Dequeue();
            }

            base.Enqueue(item);
        }
    }

    public class PracticeQueue<T> : Collection<T>
    {
        /// <summary>
        /// queue尾部入队
        /// </summary>
        /// <param name="item"></param>
        public virtual void Enqueue(T item)
        {
            if (Count == 0)
            {
                Add(item);
            }
            else
            {
                Insert(Count, item);
            }
        }

        /// <summary>
        /// 移除并返回在 Queue 的开头的对象
        /// </summary>
        public virtual T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Invalid operation empty queue.");
            }

            T removed = this[0];
            RemoveItem(0);
            return removed;
        }
    }
}
