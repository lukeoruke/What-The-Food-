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

        public async Task<bool> RemoveToken(string jwt)
        {
            try
            {

                Console.WriteLine(jwt);
                var itemBeingRemoved = (_efContext.ActiveSessionTracker.Where(r => r.jwt == jwt)).ToList()[0];
                _efContext.ActiveSessionTracker.Remove(itemBeingRemoved);
                _efContext.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                throw new Exception("EfActiveSessionGateway.RemoveToken: No account is associated with the provided token");
            }

        }

        public async Task<string> GetTokenFromUserID(int userID)
        {
            return (_efContext.ActiveSessionTracker.Where(r => r.UserID == userID).ToList())[0].jwt;
        }
       
    }
}
