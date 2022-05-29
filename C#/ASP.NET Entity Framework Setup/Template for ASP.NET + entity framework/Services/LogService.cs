using Template_for_ASP.NET___entity_framework.Database;
using Template_for_ASP.NET___entity_framework.Interfaces;
using Template_for_ASP.NET___entity_framework.Models;

namespace Template_for_ASP.NET___entity_framework.Services
{
    public class LogService : ILogService
    {
        private ApplicationDbContext database;

        public LogService(ApplicationDbContext database)
        {
            this.database = database;
        }
        public void AddLog(string method, string functionName, string parameters)
        {
            database.Logs.Add(new Log(method, functionName, parameters));
        }
    }
}
