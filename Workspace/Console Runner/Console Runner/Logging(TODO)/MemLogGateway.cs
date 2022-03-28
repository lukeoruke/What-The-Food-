namespace Console_Runner.Logging
{
    public class MemLogGateway : ILogGateway
    {
        private List<Log> _logsDB;

        public MemLogGateway()
        {
            _logsDB = new List<Log>();
        }
        public bool WriteLog(Log toLog)
        {
            _logsDB.Add(toLog);
            return true;
        }
    }
}