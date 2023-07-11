namespace Practice.Models
{
    public class SystemInformation
    {
        public string OperationSystem { get; set; }

        public string? ProcessorArchitecture { get; set; }

        public string? ProcessorModel { get; set; }

        public string? ProcessorLevel { get; set; }

        public string SystemDirectory { get; set; }

        public int ProcessorCount { get; set; }

        public string UserDomainName { get; set; }
        public string UserName { get; set; }

        public string Version { get; set; }
    }
}
