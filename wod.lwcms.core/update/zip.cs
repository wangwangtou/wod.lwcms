using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace wod.lwcms.update
{
    public interface izip
    {
        Stream ZipPath(string path);
        void UnZipPath(string path, Stream zipFile);
        void CopyTo(string sourcePath, string targetPath);
    }

    public class zip : izip
    {
        #region izip 成员

        public Stream ZipPath(string path)
        {
            MemoryStream ms = new MemoryStream();
            ZipOutputStream s = new ZipOutputStream(ms);
            ZipFiles(path, s, "");
            s.Finish();
            return ms;
        }

        private void ZipFiles(string path, ZipOutputStream s,string parentName)
        {
            string[] filenames = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            s.SetLevel(9); // 压缩级别 0-9
            byte[] buffer = new byte[4096]; 
            foreach (string file in filenames)
            {
                ZipEntry entry = new ZipEntry(parentName + Path.GetFileName(file));
                entry.DateTime = DateTime.Now;
                s.PutNextEntry(entry);
                using (FileStream fs = File.OpenRead(file))
                {
                    int size;
                    while (true)
                    {
                        size = fs.Read(buffer, 0, buffer.Length);
                        if (size > 0)
                        {
                            s.Write(buffer, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            foreach (string subPath in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                ZipFiles(subPath, s, parentName + new DirectoryInfo(subPath).Name + "\\");
            }
        }

        public void UnZipPath(string path, Stream zipFile)
        {
            ZipInputStream s = new ZipInputStream(zipFile);
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                if (theEntry.IsDirectory)
                    continue;
                string fileName = Path.Combine(path, theEntry.Name);
                string directoryName = new FileInfo(fileName).Directory.FullName;

                // create directory
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (fileName != String.Empty)
                {
                    using (FileStream streamWriter = File.Create(fileName))
                    {
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void CopyTo(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string[] filenames = Directory.GetFiles(sourcePath, "*", SearchOption.TopDirectoryOnly);
            foreach (var filename in filenames)
            {
                string targetfile = Path.Combine(targetPath, Path.GetFileName(filename));
                if (File.Exists(targetfile))
                {
                    try
                    {
                        File.Delete(targetfile);
                    }
                    catch (Exception)
                    {
                    }
                }
                File.Copy(filename, targetfile);
            }

            foreach (string subPath in Directory.GetDirectories(sourcePath, "*", SearchOption.TopDirectoryOnly))
            {
                string subTargetPath = Path.Combine(targetPath, new DirectoryInfo(subPath).Name);
                CopyTo(subPath, subTargetPath);
            }
        }

        #endregion
    }
}
