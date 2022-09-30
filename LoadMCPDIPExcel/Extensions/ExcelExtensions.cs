using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LoadMCPDIPExcel.Extensions
{
    public static class ExcelExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static string AllFirstChacUpper(this string originalStr)
        {
            string[] OriginalStringList = originalStr.Split(' ');
            List<string> ChangedStringList = new List<string>();
            foreach (string s in OriginalStringList)
            {
                ChangedStringList.Add(char.ToUpper(s[0]) + s.Substring(1).ToLower());
            }
            return string.Join(" ", ChangedStringList);
        }
        public static string MemberPreference(this string originalStr)
        {
            if (!string.IsNullOrEmpty(originalStr))
            {
                if (originalStr.ToUpper().Contains("MEMBER") && originalStr.ToUpper().Contains("PREFERENCE"))
                {
                    return "Member's preference";
                }
                return originalStr;
            }
            return null;
        }
        public static string GrievanceTypeTrim(this string originalStr)
        {
            if (!string.IsNullOrEmpty(originalStr))
            {
                if (originalStr.Contains("|"))
                {
                    string[] tempArray = originalStr.Split('|');
                    for (int i = 0; i < tempArray.Length; i++) tempArray[i] = tempArray[i].Trim();
                    return string.Join("|", tempArray);
                }
                else
                {
                    return originalStr;
                }
            }
            return null;
        }
    }
}
