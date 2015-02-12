﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MixERP.Net.Common.Helpers;
using Serilog;
using Image = System.Drawing.Image;

namespace MixERP.Net.Common
{
    public static class Conversion
    {
        public static byte[] ConvertImageToByteArray(Image imageToConvert, ImageFormat formatOfImage)
        {
            if (imageToConvert == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, formatOfImage);
                return ms.ToArray();
            }
        }

        public static Uri GetBackEndUrl(HttpContext context, string relativePath)
        {
            string administrationDirectoryName = WebConfigurationManager.AppSettings["AdministrationDirectoryName"];

            if (context != null)
            {
                if (!string.IsNullOrWhiteSpace(administrationDirectoryName))
                {
                    string lang;

                    if ((context.Session == null) || (context.Session["lang"] == null || string.IsNullOrWhiteSpace(context.Session["lang"] as string)))
                    {
                        lang = "en-US";
                    }
                    else
                    {
                        lang = context.Session["lang"] as string;
                    }

                    CultureInfo culture = new CultureInfo(lang);
                    if (culture.TwoLetterISOLanguageName == "iv")
                    {
                        culture = new CultureInfo("en-US");
                    }

                    string virtualDirectory = context.Request.ApplicationPath;
                    bool isSecure = context.Request.IsSecureConnection;
                    string domain = context.Request.Url.DnsSafeHost;
                    int port = context.Request.Url.Port;
                    string path;

                    if (virtualDirectory == "/")
                    {
                        path = string.Format(CultureInfo.InvariantCulture, "{0}:{1}/{2}/{3}/{4}/", domain, port.ToString(CultureInfo.InvariantCulture), administrationDirectoryName, culture.TwoLetterISOLanguageName, relativePath);
                    }
                    else
                    {
                        path = string.Format(CultureInfo.InvariantCulture, "{0}:{1}{2}/{3}/{4}/{5}/", domain, port.ToString(CultureInfo.InvariantCulture), virtualDirectory, administrationDirectoryName, culture.TwoLetterISOLanguageName, relativePath);
                    }

                    if (isSecure)
                    {
                        path = "https://" + path;
                    }
                    else
                    {
                        path = "http://" + path;
                    }

                    return new Uri(path, UriKind.Absolute);
                }
            }
            return new Uri("", UriKind.Relative);
        }

        public static DateTime GetLocalDateTime(string timeZone, DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
        }

        public static string GetLocalDateTimeString(string timeZone, DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime time = TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
            return String.Format(LocalizationHelper.GetCurrentUICulture(), "{0} {1} {2}", time.ToLongDateString(), time.ToLongTimeString(), zone.DisplayName);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string HashSha512Hex(string password, string salt)
        {
            if (password == null)
            {
                return null;
            }

            if (salt == null)
            {
                return null;
            }

            using (SHA512CryptoServiceProvider provider = new SHA512CryptoServiceProvider())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(password + salt))).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
            }
        }

        public static bool IsEmptyDate(DateTime date)
        {
            if (date.Equals(DateTime.MinValue))
            {
                return true;
            }

            return false;
        }

        public static bool IsNumeric(string value)
        {
            double number;
            return double.TryParse(value, NumberStyles.Any, CurrentSession.GetCulture(), out number);
        }

        public static string MapPathReverse(string fullServerPath)
        {
            if (string.IsNullOrWhiteSpace(fullServerPath))
            {
                return null;
            }
            string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            if (string.IsNullOrWhiteSpace(physicalApplicationPath))
            {
                return null;
            }

            return @"~/" + fullServerPath.Replace(physicalApplicationPath, String.Empty).Replace(@"\", "/");
        }

        public static string ResolveUrl(string url)
        {
            Page page = HttpContext.Current.Handler as Page;

            if (page == null)
            {
                return url;
            }

            return (page).ResolveUrl(url);
        }

        public static bool TryCastBoolean(object value)
        {
            bool retVal = false;

            if (value != null)
            {
                if (value is string)
                {
                    if (value.ToString().ToUpperInvariant().Equals("YES"))
                    {
                        return true;
                    }

                    if (value.ToString().ToUpperInvariant().Equals("TRUE"))
                    {
                        return true;
                    }
                }

                if (bool.TryParse(value.ToString(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static byte[] TryCastByteArray(Image image)
        {
            if (image == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static DateTime TryCastDate(object value)
        {
            try
            {
                if (value == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(value, CurrentSession.GetCulture());
            }
            catch (FormatException)
            {
                Log.Debug("Invalid date format: {Value}.", value);
            }
            catch (InvalidCastException)
            {
                Log.Debug("Could not cast {Value} to date.", value);
            }

            return DateTime.MinValue;
        }

        public static decimal TryCastDecimal(object value)
        {
            decimal retVal = 0;

            if (value != null)
            {
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                CultureInfo culture = CurrentSession.GetCulture();

                if (decimal.TryParse(numberToParse, NumberStyles.Any, culture, out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static double TryCastDouble(object value)
        {
            double retVal = 0;

            if (value != null)
            {
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (double.TryParse(numberToParse, NumberStyles.Any, CurrentSession.GetCulture(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static int TryCastInteger(object value)
        {
            int retVal = 0;

            if (value != null)
            {
                if (value is bool)
                {
                    if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    {
                        return 1;
                    }
                }

                string numberToParse = value.ToString();

                if (int.TryParse(numberToParse, NumberStyles.Any, CurrentSession.GetCulture(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static long TryCastLong(object value)
        {
            long retVal = 0;

            if (value != null)
            {
                string numberToParse = value.ToString();

                if (long.TryParse(numberToParse, NumberStyles.Any, CurrentSession.GetCulture(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static DateTime? TryCastNullableDate(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }

            return Convert.ToDateTime(value, CurrentSession.GetCulture());
        }

        public static short TryCastShort(object value)
        {
            short retVal = 0;

            if (value != null)
            {
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (short.TryParse(numberToParse, NumberStyles.Any, CurrentSession.GetCulture(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static float TryCastSingle(object value)
        {
            float retVal = 0;

            if (value != null)
            {
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (float.TryParse(numberToParse, NumberStyles.Any, CurrentSession.GetCulture(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        //    return number;
        //}
        public static string TryCastString(object value)
        {
            if (value != null)
            {
                if (value is bool)
                {
                    if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    {
                        return "true";
                    }

                    return "false";
                }

                if (value == DBNull.Value)
                {
                    return string.Empty;
                }

                string retVal = value.ToString();
                return retVal;
            }

            return string.Empty;
        }

        public static Unit TryCastUnit(object value)
        {
            if (value == null)
            {
                return new Unit();
            }

            return Unit.Parse(value.ToString(), CurrentSession.GetCulture());
        }
    }
}