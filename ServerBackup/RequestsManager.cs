using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBackup
{
    public class RequestsManager
    {

        private const string StartBackupUri = "https://uri/startbackup";
        private const string BackupStatusUri = "https://uri/backupstatus";

        public RequestsManager()
        {
            bool success = false;
            EventLog eventLog = new EventLog("Application");
            eventLog.Source = "ServerBackup";

            try
            {
                if (this.StartBackupRequest())
                    success = this.CheckBackupStatus();

                // other code to compress the file
            }
            catch (Exception e)
            {
                eventLog.WriteEntry(e.Message, EventLogEntryType.Error, 101, 1);
            }
            finally
            {
                this.STFU();
            }

            eventLog.Dispose();
        }

        private bool StartBackupRequest()
        {
            bool success;
            HttpWebRequest request;
            HttpWebResponse response;

            request = (HttpWebRequest)WebRequest.Create(StartBackupUri);
            response = (HttpWebResponse)request.GetResponse();

            success = response.StatusCode == HttpStatusCode.OK;

            return success;
        }

        private bool CheckBackupStatus()
        {
            bool success = false;
            DateTime startTime = DateTime.Now;
            DateTime now = DateTime.Now;
            HttpWebRequest request;
            HttpWebResponse response;

            while ((now - startTime).TotalHours < 2)
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 30));

                request = (HttpWebRequest)WebRequest.Create(BackupStatusUri);
                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    success = true;
                    break;
                }
            }

            return success;
        }

        private void STFU()
        {
            ProcessStartInfo process = new ProcessStartInfo("shutdown", "/s /t 0");
            process.CreateNoWindow = true;
            process.UseShellExecute = false;
            Process.Start(process);
        }
    }
}
