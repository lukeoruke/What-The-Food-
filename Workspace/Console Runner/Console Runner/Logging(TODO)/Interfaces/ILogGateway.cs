namespace Console_Runner.Logging
{
    public interface ILogGateway
    {
        public bool WriteLog(Log toSave);
    }
}
