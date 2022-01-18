using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace Console_Runner
{
    public class Archiving
    {
        private Thread _archiveThread;
        private string _currentMonth;
        private string _archiveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); //dir location of where archives are to be stored.
        private string _archiveFolder;

        public Archiving()
        {
            _currentMonth = DateTime.Now.ToString("MMMM");
            _archiveFolder = Path.Combine(_archiveDirectory, "Log Archive");
        }

        //will start the thread
        public void archiveStartThread()
        {
            ThreadStart archiver = new ThreadStart(_archiveActivate);
            _archiveThread = new Thread(archiver);
            _archiveThread.Start();
        }

        //activates the threads function
        private void _archiveActivate()
        {
            //get working on the minute
            string current = DateTime.Now.ToString("s"); //2009-06-15T13:45:30
            string offset = current.Substring(current.Length - 2);
            int numSecOff = Int32.Parse(offset);

            Thread.Sleep(1000 * (60 - numSecOff));

            //get working on the hour
            current = DateTime.Now.ToString("s");
            offset = current.Substring(current.Length - 5, current.Length - 3);
            int numHourOff = Int32.Parse(offset);

            Thread.Sleep(1000 * 60 * (60 - numHourOff));

            while (true)
            {
                this._checkToArchive();
                Thread.Sleep(1000 * 60 * 60); //check every hour on the hour
            }
            this.archiveStopThread();
        }

        //will stop the thread
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

        //code that handles checking if we need to archive the current logs
        private bool _checkToArchive()
        {
            /**Intended to only be ran if a "Log Archive" directory does not exist. If so, create a directory that will hold archived files.
             * NOTE: Upon Archive object being created this code is ran. HOWEVER, if for whatever reason if someone on the machine were to alter this file,
             *       a deletion or file name change, this code will create the new file.
             */
            if (!File.Exists(_archiveFolder))
            {
                Directory.CreateDirectory(Path.Combine(_archiveDirectory, "Log Archive"));
                Console.WriteLine(Path.Combine(_archiveDirectory, "Log Archive"));
            }

            try
            {
                if (_newMonth())
                {
                    DateTime currentDate = DateTime.Now;

                    /**
                     * 
                     * CODE FOR PULLING LOGS OLDER THAN 30 DAY FROM DATABASE
                     * 
                     */

                    String tgzName = Path.Combine(_archiveFolder, _currentMonth + ".tar.gz"); //tar.gz file name (directory location and name)
                    String fileName = Path.Combine(Environment.CurrentDirectory, "logs.txt"); //file to be compressed (directory location and name)

                    using (var oStream = File.Create(tgzName))                          //creating a tar.gz file to which we are storing to
                    using (var gStream = new GZipOutputStream(oStream))                 //the stream filter for writing compressed data
                    using (var tarArchive = TarArchive.CreateOutputTarArchive(gStream)) //archiving the data from one file to another
                    {
                        tarArchive.RootPath = Path.GetDirectoryName(fileName);

                        var entry = TarEntry.CreateEntryFromFile(fileName);
                        entry.Name = Path.GetFileName(fileName);

                        tarArchive.WriteEntry(entry, true);
                    }

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

        //checks to see if the current month has changed
        private bool _newMonth()
        {
            if (_currentMonth != DateTime.Now.ToString("MMMM"))
            {
                return true;
            }
            else
                return false;
        }

        private int _findCutoff(string[] text, DateTime curr)
        {
            double diff = Int32.MaxValue;
            int cutoff = 1;
            while (diff > 30)
            {
                diff = curr.Subtract(DateTime.Parse(text[cutoff].Substring(0, 10))).TotalDays;
                if (diff < 30)
                    return cutoff;
                cutoff++;
            }
            return cutoff;
        }
    }

    public class Logging
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "Logs.txt"); //path to logs file

        //logging objects
        public Logging()
        {
            Console.WriteLine(filePath);
            try
            {
                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        Console.WriteLine("Log file created.");
                        sw.WriteLine("-------Start of logs-------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        //base logging function that will write to the log.txt file. Will append logging information to the end of current date and time.
        public bool log(string toLog)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " " + toLog);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //formats string for user login to send to log()
        public bool logLogin(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Login Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account deactivation to send to log()
        public bool logAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Deactivation Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Target Account: " + target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account enabling to send to log()
        public bool logAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Enabling Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Target Account: " + target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account promoting to send to log()
        public bool logAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Promotion Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Account Promoted: " + promoted);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account creation to send to log()
        public bool logAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Account Creation Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account deletion to send to log()
        public bool logAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Account Deletion Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account name change to send to log()
        public bool logAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Account Name Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + "Changed From: " + prevName + ": Changed To:" + newName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account email change to send to log()
        public bool logAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Account Name Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + "Changed From: " + prevEmail + ": Changed To:" + newEmail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account password change to send to log()
        public bool logAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Account Password Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account food flag changes to send to log()
        public bool logAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Account Flag Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Added Flags: " + added + " Removed Flags: " + removed);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account data request to send to log()
        public bool logAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Data Request Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Send To: " + sendTo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account AMR changes to send to log()
        public bool logAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": AMR Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " AMR Changed From: " + from + " AMR Changed To: " + to);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for reviews to send to log()
        public bool logReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Review Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product + " Rating: " + rating + " Review: ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for histroy changes to send to log()
        public bool logHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": History Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product + " Index In History: " + index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for scan additions to send to log()
        public bool logScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Scan Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for a generic logging event to send to log()
        public bool logGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info)
        {
            try
            {
                log("Catagory: " + category + " " + pageName + ": Action Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
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
