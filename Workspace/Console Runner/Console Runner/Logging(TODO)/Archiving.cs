using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //will start the thread
        public void archiveStartThread()
        {
            ThreadStart archiver = new ThreadStart(_archiveActivate);
            _archiveThread = new Thread(archiver);
            _archiveThread.IsBackground = true;
            _archiveThread.Start();
        }

        //activates the threads function
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
            /*Intended to only be ran if a "Log Archive" directory does not exist. If so, create a directory that will hold archived files.
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
    }
}
