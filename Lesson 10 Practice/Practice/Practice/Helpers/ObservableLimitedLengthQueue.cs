namespace Practice.Helpers
{
    /// <summary>
    /// 可观察的有限长度队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableLimitedLengthQueue<T> : ObservableQueue<T>
    {
        protected int MaxLength;

        public ObservableLimitedLengthQueue(int length)
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
}
