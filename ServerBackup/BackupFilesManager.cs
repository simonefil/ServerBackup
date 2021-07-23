using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ServerBackup
{
    public class BackupFilesManager
    {
        private const string backupTempPath = "";
        private const string backupTargetPath = "";

        public BackupFilesManager() { }

        public void CompressBackupFilesAndMove()
        {
            if (Directory.Exists(backupTempPath))
            {
                ZipFile.CreateFromDirectory(backupTempPath, backupTargetPath + DateTime.Today + ".zip");
            }
        }
    }
}
