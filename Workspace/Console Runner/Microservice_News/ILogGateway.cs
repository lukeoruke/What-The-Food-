namespace Console_Runner.Logging
{
    public interface ILogGateway
    {
        /// <summary>
        /// Write the given Log to the log database. Aborts if the CancellationToken requests.
        /// </summary>
        /// <param name="toSave">The Log to write to the database.</param>
        /// <param name="ct">CancellationToken used to signal whether to abort the action.</param>
        /// <returns>True if the Log was successfully written to the database.</returns>
        public Task<bool> WriteLogAsync(Log toSave, CancellationToken ct = default);
        public Task<bool> WriteLogsAsync(List<Log> toSave, CancellationToken ct = default);

        public List<Log> GetLogsWhere(params Func<Log, bool>[] predicates);
    }
}
