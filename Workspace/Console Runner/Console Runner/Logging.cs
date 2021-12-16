using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Console_Runner
{
    public class Archiving
    {
        private Thread _archiveThread;
        private string _currentMonth;

        public Archiving()
        {
            _currentMonth = DateTime.Now.ToString("MMMM");
            this._checkToArchive();
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
            string current = DateTime.Now.ToString("s");
            string offset = current.Substring(current.Length - 2);
            int numSecOff = Int32.Parse(offset);

            Thread.Sleep(1000 * numSecOff);

            //get working on the hour
            current = DateTime.Now.ToString("s");
            offset = current.Substring(current.Length - 5, current.Length - 3);
            int numHourOff = Int32.Parse(offset);

            Thread.Sleep(1000 * 60 * numHourOff);

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
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Log Archive.zip")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Log Archive"));
                ZipFile.CreateFromDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Log Archive"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Log Archive.zip"));
            }

            Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Log Archive.zip"));

            try
            {
                if (_newMonth())
                {
                    DateTime currentDate = DateTime.Now;
                    string[] lines = System.IO.File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "Logs.txt"));

                    int cuttoff = _findCutoff(lines, currentDate);

                    using (FileStream zipToOpen = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Log Archive.zip")
                        , FileMode.Open))
                    {
                        using (ZipArchive zipArchive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                        {
                            ZipArchiveEntry newFile = zipArchive.CreateEntry(_currentMonth + " Logs.txt");
                            using (StreamWriter sw = new StreamWriter(newFile.Open()))
                            {
                                Console.WriteLine("Archive Log file created.");
                                sw.WriteLine("-------Start of logs-------");
                                for (int i = 1; i < cuttoff; i++)
                                {
                                    sw.WriteLine(lines[i]);
                                }
                            }
                        }
                    }

                    File.Delete(Path.Combine(Environment.CurrentDirectory, "Logs.txt"));
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, " Logs.txt")))
                    {
                        sw.WriteLine("-------Start of logs-------");
                        for (int i = cuttoff; i < lines.Length; i++)
                        {
                            sw.WriteLine(lines[i]);
                        }
                    }

                    File.Move(Path.Combine(Environment.CurrentDirectory, "Logs.txt"), Path.Combine(Environment.CurrentDirectory, "Log Storage"));

                    _currentMonth = currentDate.ToString("MMMM");

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
