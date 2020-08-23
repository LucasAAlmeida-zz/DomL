using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Utils
{
    public class Util
    {
        public static bool IsEqualString(string string1, string string2)
        {
            string rExp = @"[^\w\d]";
            var string1Limpa = Regex.Replace(string1, rExp, "").ToLower().Replace("the", "");
            var string2Limpa = Regex.Replace(string2, rExp, "").ToLower().Replace("the", "");
            return string1Limpa == string2Limpa;
        }

        public static string CleanString(string value)
        {
            return Regex.Replace(value, @"[^\w\d]", "").ToLower().Replace("the", "");
        }

        public static string GetFormatedDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        public static void CreateDirectory(string filePath)
        {
            var fi = new FileInfo(filePath);
            if (fi.Directory != null && !fi.Directory.Exists && fi.DirectoryName != null) {
                Directory.CreateDirectory(fi.DirectoryName);
            }
        }
    }
}
