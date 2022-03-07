using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Console_Runner.DAL;

namespace Console_Runner.Logging
{
    public class Archiving
    {
        private Thread _archiveThread;
        private readonly string _archiveName = "Logs Archive";
        private readonly string _archiveDefault = Path.Combine(Directory.GetCurrentDirectory(), "Logs Archive");    //default location of logs archive
        private string _currentMonth;       //current month
        private string _archiveDirectory;   //the path of the directory that the archive folder will be stored in
        private string _archiveFolder;      //the actual path of the archive folder

        public Archiving()
        {
            _currentMonth = DateTime.Now.ToString("MMMM");
            _archiveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); //dir location of where archives are to be stored.
            _archiveFolder = Path.Combine(_archiveDirectory, _archiveName);
        }

        /// <summary>
        /// Creates the thread that will check if we need to update archive
        /// </summary>
        public void archiveStartThread()
        {
            ThreadStart archiver = new ThreadStart(_archiveActivate);
            _archiveThread = new Thread(archiver);
            _archiveThread.IsBackground = true;
            _archiveThread.Start();
        }

        /// <summary>
        /// Activates the thread and begins checking if archives needs to update
        /// </summary>
        private void _archiveActivate()
        {
            //get working on the minute
            string current = DateTime.Now.ToString("s"); //in the format 2009-06-15T13:45:30
            string offset = current.Substring(current.Length - 2);
            int numSecOff = Int32.Parse(offset);

            Thread.Sleep(1000 * (60 - numSecOff));

            //get working on the hour
            current = DateTime.Now.ToString("s");
            offset = current.Substring(current.Length - 5, 2);
            int numHourOff = Int32.Parse(offset);

            Thread.Sleep(1000 * 60 * (60 - numHourOff));

            while (true)
            {
                this._checkToArchive();
                Thread.Sleep(1000 * 60 * 60); //check every hour on the hour
            }

            this.archiveStopThread();
        }

        /// <summary>
        /// stops the thread checking for archiving updates
        /// </summary>
        /// <returns>A bool indicating whether the action was successful</returns>
        public bool archiveStopThread()
        {
            try
            {
                _archiveThread.Abort();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR IN STOPPING THREAD: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Function that handles the logic of checking if archives needs to be updated 
        /// as well as executing archiving if needed
        /// </summary>
        /// <returns>A bool indicating whether the action was successful</returns>
        private bool _checkToArchive()
        {
            /**Intended to only be ran if a "Log Archive" directory does not exist. If so, create a directory that will hold archived files.
             * NOTE: Upon Archive object being created this code is ran. HOWEVER, if for whatever reason if someone on the machine were to alter this file,
             *       a deletion or file name change, this code will create the new file.
             * TODO: INTEGRATION TESTING FOR DIRECTORY ACCESSABILITY ERRORS
             */

            if (!File.Exists(_archiveFolder))               //check if folder we want to archive to exists
            {
                var isWritable = dirIsWritable(_archiveDirectory);

                if (isWritable)                   //checks if the desired directory to host log archives exist AND if we are able to write to
                {
                    Directory.CreateDirectory(_archiveFolder);
                    Console.WriteLine("Archive Created: " + _archiveFolder);
                }
                else
                {
                    _archiveFolder = _archiveDefault;   //set new logs archive location to be stored in the project folder since we know we have permissions
                    Console.WriteLine("Error creating Logs Archive:" +
                                        "\nLogs Archive has been rerouted to: " + _archiveFolder);
                    Directory.CreateDirectory(_archiveFolder);
                    Console.WriteLine("Archive Created: " + _archiveFolder);
                }
            }
            else if (!dirIsWritable(_archiveFolder))    //if for some reason Logs Archive exists AND we can no longer write to it 
            {
                _archiveFolder = _archiveDefault;       //set new logs archive location to be stored in the project folder since we know we have permissions
                Console.WriteLine("Error Can No Longer Write To Logs Archive:" +
                                    "\nLogs Archive has been rerouted to: " + _archiveFolder);
                Directory.CreateDirectory(_archiveFolder);
                Console.WriteLine("Archive Created: " + _archiveFolder);
            }

            try
            {
                if (_newMonth())
                {
                    DateTime currentDate = DateTime.Now;

                    string newFileName = Path.Combine(_archiveDirectory, (DateTime.Now.ToString("Y") + ".txt"));
                    using (StreamWriter sw = File.CreateText(newFileName))
                    {
                        Console.WriteLine("Archive Log file created.");
                        sw.WriteLine("-------Start of logs-------");

                        using (var context = new Context())
                        {
                            DateTime logDate;
                            foreach (var oldLogs in context.Logs)
                            {
                                logDate = Convert.ToDateTime(oldLogs.Date + " " + oldLogs.Time);             //get the string value of Date from Logs datastore and convert to DateTime
                                Console.WriteLine(oldLogs.ToString());
                                if ((int)DateTime.Now.Subtract(logDate).Seconds >= 30)                      //get the integer value of days between now and the log in question,
                                                                                                            //if greater than or equal to 30 days then execute
                                {
                                    sw.WriteLine(oldLogs.ToString());
                                    context.Remove(oldLogs);
                                }
                            }

                            context.SaveChanges();
                        }

                    }

                    FileInfo fInfo = new FileInfo(newFileName);
                    fInfo.IsReadOnly = true;                    //set this new archive file to read only 

                    String tgzName = Path.Combine(_archiveFolder, DateTime.Now.ToString("Y") + ".tar.gz"); //tar.gz file name (directory location and name)

                    using (var oStream = File.Create(tgzName))                          //creating a tar.gz file to which we are storing to
                    using (var gStream = new GZipOutputStream(oStream))                 //the stream filter for writing compressed data
                    using (var tarArchive = TarArchive.CreateOutputTarArchive(gStream)) //archiving the data from one file to another
                    {
                        tarArchive.RootPath = Path.GetDirectoryName(newFileName);

                        var entry = TarEntry.CreateEntryFromFile(newFileName);
                        entry.Name = Path.GetFileName(newFileName);

                        tarArchive.WriteEntry(entry, true);
                    }

                    fInfo.IsReadOnly = false;
                    File.Delete(newFileName);

                    _currentMonth = currentDate.ToString("MMMM"); //once all archiving is done, set a new _currentMonth

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR IN ARCHIVING: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// A helper function that determines if a given directory is writable or not
        /// </summary>
        /// <param name="path">A path, given as a string, of the directory to be tested</param>
        /// <returns>A bool values indicating whether the directory is writable or not</returns>
        private bool dirIsWritable(string path)
        {
            try
            {
                using (FileStream fs = File.Create(Path.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                {
                    //try to create a file in the directory to check if we have write permissions
                    //File.Create(String,Int32,FileOptions)
                    //String: the path of the file we are creating
                    //Int32: size of the buffer
                    //FileOptions: special options that describe how to create/overwrite the file. In this case once we close the file it will delete
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether it is a new month or not
        /// </summary>
        /// <returns>A bool indicating whether the month has changed or not</returns>
        private bool _newMonth()
        {
            if (_currentMonth != DateTime.Now.ToString("MMMM"))
            {
                return true;
            }
            else
                return false;
        }
    }

    public class Logging : ILogger
    {
        private IlogGateway _logAccess;
        //logging objects
        public Logging(IlogGateway logAccessor)
        {
            _logAccess = logAccessor;
        }

        /// <summary>
        /// Function that will take a formatted string as input and send it to our log storage
        /// </summary>
        /// <param name="toLog">The string that will be logged in the data store</param>
        /// <returns>A bool value indicating whether the action was successful</returns>
        public bool Log(string toLog)
        {
            Logs record = new Logs();
            record.Date = DateTime.Now.ToLongDateString();
            record.Time = DateTime.Now.ToString("H:mm:ss");
            record.Message = toLog;
            _logAccess.WriteLog(record);
            return true;
        }

        /// <summary>
        /// Logging funciton to format user log in
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns>A string formated as an account login log</returns>
        public bool LogLogin(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Login Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account deactivation
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns></returns>
        /// <param name="target">The account that is being targeted for deactivation</param>
        /// <returns>A string formated as an account deactivation log</returns>
        public bool LogAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Deactivation Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Target Account: " + target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account enabling 
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns></returns>
        /// <param name="target">The account that is being targeted for enabling</param>
        /// <returns>Returns a string formated as an account enabling log</returns>
        public bool LogAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Enabling Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Target Account: " + target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account promotion
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns></returns>
        /// <param name="promoted">User whom is being promoted</param>
        /// <returns>A string formated as an account promotion log</returns>
        public bool LogAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Promotion Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Account Promoted: " + promoted);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account creation
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action, in this case, the account being created</param>
        /// <returns>A string formatted as an account creation log</returns>
        public bool LogAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Creation Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account deletion
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns>A string formatted as an account deletion log</returns>
        public bool LogAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Deletion Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account name change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="prevName">User's previous name</param>
        /// <param name="newName">User's name to be changed to</param>
        /// <returns>A string formatted as an account name change log</returns>
        public bool LogAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Name Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + "Changed From: " + prevName + ": Changed To:" + newName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account email change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="prevEmail">User's previous email</param>
        /// <param name="newEmail">User's email to be changed to</param>
        /// <returns>A string formatted as an account email change log</returns>
        public bool LogAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Name Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + "Changed From: " + prevEmail + ": Changed To:" + newEmail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account password change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <returns>A string formatted as an account password change log</returns>
        public bool LogAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Password Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account food flag change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="added">Any food flags that have been added by the user</param>
        /// <param name="removed">Any food flags that have been removed by the user</param>
        /// <returns>A string formatted as an account food flag change</returns>
        public bool LogAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Flag Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Added Flags: " + added + " Removed Flags: " + removed);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account data request log
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="sendTo">The email that the data is to be sent</param>
        /// <returns>A string formatted as an account data request log</returns>
        public bool LogAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Data Request Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Send To: " + sendTo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account AMR change
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="from">User's previous AMR values</param>
        /// <param name="to">User's new AMR values</param>
        /// <returns>A string formatted as an account amr change</returns>
        public bool LogAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": AMR Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " AMR Changed From: " + from + " AMR Changed To: " + to);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format an account creating a review
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="product">The product the review is for</param>
        /// <param name="rating">The rating the user gave the product</param>
        /// <param name="text">The written review of the product</param>
        /// <returns>A string formatted to be a review log</returns>
        public bool LogReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Review Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product + " Rating: " + rating + " Review: ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account history additions
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="product">The product that is being added to the user's history</param>
        /// <param name="index">The index that this product is being inserted to of the user's history</param>
        /// <returns>A string formatted as an account history log</returns>
        public bool LogHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": History Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product + " Index In History: " + index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to format account scan upload
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="product">The product that has been scanned and uploaded</param>
        /// <returns>A string formatted as a user scan upload</returns>
        public bool LogScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Scan Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logging funciton to create a generic log
        /// </summary>
        /// <param name="category">The category of which the action is defined</param>
        /// <param name="pageName">The page that the action is called from</param>
        /// <param name="isSuccess">Whether the action being attempted succeeded or not</param>
        /// <param name="failCase">The fail case/error/exception if the action failed</param>
        /// <param name="user">User whom called for the action</param>
        /// <param name="info">Any info that is relavent to the generic log</param>
        /// <returns>Returns a string formatted as a generic log</returns>
        public bool LogGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Action Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " " + info);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}