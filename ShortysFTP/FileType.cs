using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;


namespace ShortysFTP
{
    class FileType
    {
        public static void Download(SftpClient sftp, string localDirectory, string fileName)
        {
            string localFileName = fileName;

            try
            {
                using (Stream file = File.OpenWrite(localDirectory + localFileName))
                {
                    sftp.DownloadFile(sftp.WorkingDirectory + "/" + fileName, file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void Upload()
    }
}
