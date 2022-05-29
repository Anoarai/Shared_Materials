namespace Template_for_ASP.NET___entity_framework.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string FunctionName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Paremeters { get; set; }

        private Log()
        {
        }

        public Log(string method, string functionName, string paremeters)
        {
            Method = method;
            FunctionName = functionName;
            Paremeters = paremeters;
        }
    }
}
