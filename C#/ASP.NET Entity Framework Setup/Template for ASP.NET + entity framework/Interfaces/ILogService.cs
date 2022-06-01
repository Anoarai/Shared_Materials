namespace Template_for_ASP.NET___entity_framework.Interfaces
{
    public interface ILogService
    {
        public void AddLog(string method, string functionName, string statusCode, string parameters);
    }
}
