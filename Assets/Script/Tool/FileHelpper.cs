using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Tool
{

    /// <summary>
    /// Handle File
    /// </summary>
    public static class FileHelpper
    {

        public static bool Exsist(string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// 复制文件到路径下
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(dir1))
            {
                File.Copy(dir1, dir2, true);
            }
            Debug.Log("复制：" + dir1);
        }

        /// <summary>
        /// 删除路径下文件
        /// </summary>
        /// <param name="path"></param>
        public static void Delet(string path)
        {
            // 创建对文件对象。
            FileInfo fi = new FileInfo(path);
            // 创建文件
            FileStream fs = fi.Create();
            // 根据需要修改文件，然后关闭文件。
            fs.Close();
            // 删除该文件。
            fi.Delete();
            Debug.Log("删除：" + path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varFromDirectory">文件夹内</param>
        /// <param name="varToDirectory">文件夹</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            //FuZhiWenJian(varFromDirectory, varToDirectory);
            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }


        public static void GetAllFiles(List<string> files, string dir)
        {
            string[] fls = Directory.GetFiles(dir);
            foreach (string fl in fls)
            {
                files.Add(fl);
            }
            //获取子文件夹
            string[] subDirs = Directory.GetDirectories(dir);
            foreach (string subDir in subDirs)
            {
                GetAllFiles(files, subDir);
            }
        }
        public static void CleanDirectory(string dir)
        {
            foreach (string subdir in Directory.GetDirectories(dir))
            {
                Directory.Delete(subdir, true);
            }

            foreach (string subFile in Directory.GetFiles(dir))
            {
                File.Delete(subFile);
            }
        }

        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
            }
        }
    }

}