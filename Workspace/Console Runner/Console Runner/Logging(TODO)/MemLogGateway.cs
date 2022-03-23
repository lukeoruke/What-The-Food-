/*namespace Console_Runner.Logging
{
    public class MemLogGateway : IlogGateway
    {
        private List<Logs> _logsDB;

        public MemLogGateway()
        {
            _logsDB = new List<Logs>();
        }
        public bool WriteLog(Logs toLog)
        {
            _logsDB.Add(toLog);
            return true;
        }
    }
}
*/