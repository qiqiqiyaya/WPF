using System;

#pragma warning disable CS8618


namespace Practice.Dtos.Inputs
{
    /// <summary>
    /// 日志查询条件输入值
    /// </summary>
    public class LogSearchInput
    {
        public string Key { get; set; }

        public string? Level { get; set; }

        public SearchInputTimeDetail BeginTime { get; set; } = new SearchInputTimeDetail();

        public SearchInputTimeDetail EndTime { get; set; } = new SearchInputTimeDetail();
    }

    public class SearchInputTimeDetail
    {
        public DateTime? Left { get; set; }

        public DateTime? Right { get; set; }

        public long? GetTimestamp()
        {
            if (!Left.HasValue) return null;
            var date = Left.Value;

            if (Right.HasValue)
            {
                date = date.AddHours(Right.Value.Hour);
                date = date.AddMinutes(Right.Value.Minute);
                date = date.AddMilliseconds(Right.Value.Millisecond);
            }

            return new DateTimeOffset(date).ToUnixTimeMilliseconds();
        }
    }
}
