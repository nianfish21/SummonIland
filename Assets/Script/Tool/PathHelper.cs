using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    
    /// <summary>
    /// handle path string
    /// </summary>
    public static class PathHelpper
    {

        /// <summary>
        /// \ => //
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string PathReplace(this string self)
        {
            self = self.Replace("/", "\\");
            return self;
        }
        /// <summary>
        /// \ => //
        /// </summary>
        /// <param name="self"></param>
        /// <param name="backlevel"></param>
        /// <returns></returns>
        public static string PathBackLevel(this string self, int backlevel)
        {
            self = self.Replace("/", "\\");
            for (int i = 0; i < backlevel; i++)
            {
                self = self.Remove(self.LastIndexOf("\\"), self.Length - self.LastIndexOf("\\"));
                //Debug.Log(self);
            }
            return self;
        }

        /// <summary>
        /// 从获取文件名字.类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string PathGetFileName(this string self)
        {
            self = self.Replace("/", "\\");
            if (self.LastIndexOf("\\") > -1)
                self = self.Substring(self.LastIndexOf("\\") + 1, self.Length - 1 - self.LastIndexOf("\\"));
            else
                Debug.Log("字符串非路径");
            return self;
        }

        /// <summary>
        /// 从获取文件名字.类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string PathGetFileName_NoType(this string self)
        {
            self = self.PathGetFileName();
            if (self.LastIndexOf('.') > -1)
                self = self.Remove(self.LastIndexOf('.'), self.Length - self.LastIndexOf('.'));
            else
                Debug.Log("字符串非文件");
            return self;
        }

        /// <summary>
        /// 带文件类型的名字获取文件类型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string PathGetFileType(this string self)
        {
            if (self.LastIndexOf("\\") != -1)
                self = self.PathGetFileName();
            if (self.LastIndexOf('.') == -1) return "";
            self = self.Substring(self.LastIndexOf("."), self.Length - self.LastIndexOf("."));
            return self;
        }

    }
}