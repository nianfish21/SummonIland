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
        /// �����ļ���·����
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
            Debug.Log("���ƣ�" + dir1);
        }

        /// <summary>
        /// ɾ��·�����ļ�
        /// </summary>
        /// <param name="path"></param>
        public static void Delet(string path)
        {
            // �������ļ�����
            FileInfo fi = new FileInfo(path);
            // �����ļ�
            FileStream fs = fi.Create();
            // ������Ҫ�޸��ļ���Ȼ��ر��ļ���
            fs.Close();
            // ɾ�����ļ���
            fi.Delete();
            Debug.Log("ɾ����" + path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varFromDirectory">�ļ�����</param>
        /// <param name="varToDirectory">�ļ���</param>
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
            //��ȡ���ļ���
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
                throw new Exception("��Ŀ¼���ܿ�������Ŀ¼��");
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