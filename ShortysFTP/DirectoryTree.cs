using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace ShortysFTP
{
    class DirectoryTree
    {
        public static void List(SftpClient sftp, string path)
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
                                    List(sftp, sDir.FullName);
                                }
                            }
                        }

                    }
                }
            }


        }

        //Taken from: https://docs.microsoft.com/en-us/dotnet/articles/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree
        public static void List(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                // log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    Console.WriteLine(fi.FullName);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    List(dirInfo);
                }
            }
        }
    }
}
