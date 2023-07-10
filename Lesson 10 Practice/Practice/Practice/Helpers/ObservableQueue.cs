using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Practice.Helpers
{
    /// <summary>
    /// 可观察队列
    /// </summary>
    /// <remarks>
    /// 先进先出
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class ObservableQueue<T> : ObservableCollection<T>
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

        /// <summary>
        /// 尝试移除并返回在 Queue 的开头的对象
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual bool TryDequeue([MaybeNullWhen(false)] out T result)
        {
            if (Count == 0)
            {
                result = default!;
                return false;
            }

            result = this[0];
            RemoveItem(0);
            return true;
        }
    }
}
