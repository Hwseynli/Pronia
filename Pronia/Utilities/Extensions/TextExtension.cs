using System.Text;
using System.Text.RegularExpressions;

namespace Pronia.Utilities.Extensions
{
	public static class TextExtension
	{
		public static string Capitalize(this string name)
		{
            name = name.Trim();
            string[] arr = name.Split(" ");
            StringBuilder newstr = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != " ")
                {
                    arr[i] = Char.ToUpper(arr[i][0]) + arr[i].Substring(1).ToLower() + "";
                    newstr.Append(arr[i]);
                }
            }
            if (arr.Length < 2)
            {
                name = newstr.ToString();
                return name;
            }
            else
            {
                return null;
            }
        }
        public static bool IsEmail(this string email)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (regex.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

