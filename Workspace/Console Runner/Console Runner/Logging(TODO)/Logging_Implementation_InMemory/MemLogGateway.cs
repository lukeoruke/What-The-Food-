﻿namespace Console_Runner.Logging.Testing
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
            cancellationToken.ThrowIfCancellationRequested();
            _logsDB.Add(toLog);
            return true;
        }
    }
}