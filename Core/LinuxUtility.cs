﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.Core
{
    public class LinuxUtility
    {

        public static bool IsLinux()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))
            {
                return true;//Console.WriteLine("Running on Unix");
            }
            else
            {
                return false; // Console.WriteLine("NOT running on Unix");
            }
        }

        /// <summary>
        /// hacks to fix difference between microsoft .net and mono DateTime.TryParse
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool TryParseDateTime(string dateStr, out DateTime t)
        {
            if (!IsLinux())
                return DateTime.TryParse(dateStr,out t);

            Logger.WriteLine("LinuxUtility.TryParseDateTime('"+dateStr +"')");
            if (Regex.IsMatch(dateStr,@"\w{3}\s*\d{4}\s*"))// "JAN  2009 "
            {
                string str = dateStr.Insert(4, "1").Trim();
                //Console.WriteLine(str);
                 return DateTime.TryParseExact(str, "MMM d yyyy", CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out t);
            }



            return DateTime.TryParse(dateStr,out t);
        }

    }
}
