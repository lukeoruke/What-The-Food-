using Console_Runner.AccountService;

namespace Console_Runner.AccountService
{
    public class EFActiveSessionTrackerGateway : IActiveSessionTrackerGateway
    {
        private readonly ContextAccountDB _efContext;

        public async Task<bool> StartSessionAsync(int userId, string jwt)
        {
            try
            {
                await _efContext.ActiveSessionTracker.AddAsync(new ActiveSessionTracker(userId, jwt));
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (new Exception("starting a new active session failed UserID: " + userId.ToString() + " \n JWT: " + jwt));
            }
            
            
        }
        public async Task<int> GetActiveUserAsync(string jwt)
        {
            try
            {
                ActiveSessionTracker  session = await _efContext.ActiveSessionTracker.FindAsync(jwt);
                if (session == null)
                {
                    return -1;
                }
                return session.UserID;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (new Exception("geting the active user from JWT token failed."));
            }
            
        }

       
    }
}
