using System.Collections.ObjectModel;

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
        public virtual void Enqueue(T item)
        {
            Insert(0, item);
        }

        public virtual void Dequeue()
        {
            RemoveItem(this.Count - 1);
        }
    }
}
