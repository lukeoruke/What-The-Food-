namespace Console_Runner.Logging.Testing
{
    public class MemLogGateway : ILogGateway
    {
        private List<Log> _logsDB;

        public MemLogGateway()
        {
            _logsDB = new List<Log>();
        }
        public async Task<bool> WriteLogAsync(Log toLog, CancellationToken cancellationToken = default)
        {
            Thread.Sleep(100);
            cancellationToken.ThrowIfCancellationRequested();
            _logsDB.Add(toLog);
            return true;
        }

        public async Task<bool> WriteLogsAsync(List<Log> toSave, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _logsDB.AddRange(toSave);
            Thread.Sleep(100);
            return true;
        }
    }
}