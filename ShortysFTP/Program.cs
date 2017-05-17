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
            string localDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
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

                        string path = sftp.WorkingDirectory;

                        GetDirectoryTree(sftp, path);

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

                        var localFiles = Directory.GetFiles(localDirectory);
                        var localDirs = Directory.GetDirectories(localDirectory);

                        foreach (var dir in localFiles)
                        {
                            Console.WriteLine(dir.Replace(localDirectory, ""));
                        }

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

        private static void GetDirectoryTree(SftpClient sftp, string path)
        {
            string hiddenFiles = "."; //Dont list system files and etc            

            List<SftpFile> directories = sftp.ListDirectory(path).ToList();
            
            foreach (var dir in directories)
            {
                if (!dir.Name.StartsWith(hiddenFiles))
                {
                    Console.WriteLine(dir.Name);

                    if (dir.IsDirectory)
                    {
                        List<SftpFile> subDirectories = sftp.ListDirectory(dir.FullName.TrimEnd()).ToList();
                        foreach (var sDir in subDirectories)
                        {
                            if (!sDir.Name.StartsWith(hiddenFiles))
                            {
                                Console.WriteLine(sDir.Name);

                                if (sDir.IsDirectory)
                                {
                                    GetDirectoryTree(sftp, sDir.FullName);
                                }
                            }
                        }

                    }
                }                    
            }
            

        }
    }
}
