namespace Console_Runner.AccountService
{
    public interface IActiveSessionTrackerGateway
    {
        public Task<int> GetActiveUserAsync(string jwt);

        public Task<bool> StartSessionAsync(int userId, string jwt);

    }
}
