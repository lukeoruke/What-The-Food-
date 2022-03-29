namespace Console_Runner.Logging
{
    public interface ILogGateway
    {
        public Task<bool> WriteLogAsync(Log toSave);
    }
}
