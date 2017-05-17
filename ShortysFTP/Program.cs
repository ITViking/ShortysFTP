using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;

namespace ShortysFTP
{
    class Program
    {
        static void Main(string[] args)
        {            
            var host = "10.192.138.169"; //IP adress
            int port = 22;
            var user = "phil"; //USername on host
            var pass = "Scout4Life!"; //Password for User
            string localDirectory = @"C:\DEV"; //path to a local folder 
            string fileName = "";

            using (SftpClient sftp = new SftpClient(host, port, user, pass))
            {
                sftp.Connect();

                do //Keep alive for testing
                {
                    while (true) //Keep alive for testing
                    {
                        //Remote location
                        Console.WriteLine("== Remote ==");

                        string remotePath = sftp.WorkingDirectory;

                        DirectoryTree.List(sftp, remotePath);

                        Console.WriteLine("");
                        Console.WriteLine("what file do you want to download?");
                        Console.WriteLine(fileName);
                        string fileToDownload = Console.ReadLine();
                        Console.Clear();

                        fileName = fileToDownload;

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

                        //Local users directory
                        Console.WriteLine("== Local ==");

                        //TO DO: get Local directory tree

                        DirectoryInfo localPath = new DirectoryInfo(localDirectory);

                        DirectoryTree.List(localPath);
                                                                                              
                        Console.WriteLine("");
                        Console.WriteLine("what file do you want to upload?");
                        Console.WriteLine(fileName);
                        string fileToUpload = Console.ReadLine();
                        Console.Clear();

                        string remoteFileName = fileToUpload;

                        using (Stream file = File.OpenRead(localDirectory + remoteFileName))
                        {
                            sftp.UploadFile(file, localDirectory + fileToUpload);
                        }

                    } //Keep alive for testing

                } while (Console.ReadKey(true).Key == ConsoleKey.Escape); //Keep alive for testing
            }
            Console.ReadKey();
        }
    }
}
