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
        public string Left { get; set; }

        public string Right { get; set; }

        public long? GetTimestamp()
        {
            if (Left.IsNullOrWhiteSpace()) return null;

            string str = Left + " " + Right;
            if (DateTimeOffset.TryParse(str, out DateTimeOffset output))
            {
                return output.ToUnixTimeMilliseconds();
            }

            return null;
        }
    }
}
