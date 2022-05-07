using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;

namespace Console_Runner.AccountService
{
    public class EFActiveSessionTrackerGateway : IActiveSessionTrackerGateway
    {
        private readonly ContextAccountDB _efContext = new ContextAccountDB();

        public async Task<bool> StartSessionAsync(int userId, string jwt)
        {
            try
            {
                await _efContext.ActiveSessionTracker.AddAsync(new ActiveSessionTracker(userId, jwt));
                await _efContext.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Microservice_Food.EFActiveSessionTrackerGateway");
                Console.WriteLine(ex.InnerException + " Microservice_Food.EFActiveSessionTrackerGateway");
                throw (new Exception("starting a new active session failed UserID: " + userId.ToString() + " \n JWT: " + jwt));
            }
            
            
        }
        public async Task<int> GetActiveUserAsync(string jwt)
        {
            try
            {
                ActiveSessionTracker session = await _efContext.ActiveSessionTracker.FindAsync(jwt);
                Console.WriteLine("JWT: " + jwt);
                if (session == null)
                {
                    Console.WriteLine("SESSION WAS NULL!, PROVIDED JWT DOESNT EXIST");
                    return -1;
                }
                return session.UserID;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (new Exception("geting the active user from JWT token failed."));
            }
            
        }
        public async Task<bool> ValidateToken(string jwt)
        {
            JWTAuthenticationService authentService = new JWTAuthenticationService("TESTDATAHERE");
            return authentService.ValidateToken(jwt);
        }
       
    }
}
