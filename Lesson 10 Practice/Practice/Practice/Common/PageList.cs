namespace Practice.Common
{
    public class PageList<T>
    {
        public T Data { get; set; }

        public int Count { get; set; }

        public PageList(T data, int count)
        {
            Data = data;
            Count = count;
        }
    }
}
