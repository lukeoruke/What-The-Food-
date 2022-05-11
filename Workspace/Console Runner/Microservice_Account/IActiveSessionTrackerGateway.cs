namespace Console_Runner.AccountService
{
    public interface IActiveSessionTrackerGateway
    {
        public Task<int> GetActiveUserAsync(string jwt);

        public Task<bool> StartSessionAsync(int userId, string jwt);

        public Task<bool> ValidateToken(string jwt);

        public Task<bool> RemoveToken(string jwt);

        public Task<string> GetTokenFromUserID(int userID);

    }
}
