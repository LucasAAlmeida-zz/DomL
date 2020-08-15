using System;

namespace DomL.Business.Utils
{
    public class Util
    {
        public static bool IsEqualTitle(string titulo1, string titulo2)
        {
            string titulo1Limpo = titulo1.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            string titulo2Limpo = titulo2.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").ToLower();
            return titulo1Limpo == titulo2Limpo;
        }

        public static string GetDiaMes(DateTime date)
        {
            return date.Day.ToString("00") + "/" + date.Month.ToString("00");
        }
    }
}
