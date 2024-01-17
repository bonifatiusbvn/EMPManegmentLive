using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.Common
{
  public static class Common
    {
        public const string AscDirection = "asc";
        public const string DescDirection = "desc";
        public const string PriceCurrency = "USD";
        public const int pageSize = 20;
        public const string _dateformat = "MM/dd/yy";
        public const string _dateLongMonthFormat = "dd MMM ";
        public const string _singledateLongMonthFormat = "dd MMM,";
        public const string _shortYear = "yy";
        public const string _timeformat = "h:mm";//
        public const string _dayNamewithtimeformat = "ddd, h:mm";//
        public const string _datelongMonthwithDashformat = "dd-MMM-yy";
        //public const string _longDateTimeFormat = "MMM dd,yy h:mm ";//
        public const string _longDatetimeWithDaynameformat = " yy - ddd, h:mm ";//
        public const string _ampmFormat = "tt";
        public const string _shortdaynameFormat = "ddd";
        public const string _PointsofInterest = "Points of Interest";
        public const string _Shopping = "Shopping";
        public const string _BarsAndNightLife = "Bars & NightLife";
        public const string _Sights = "Sights";
        public const string _DayTrips = "Day Trips";
        public const string _ThingsToDo = "Things To Do";
        public const string _MyPlaces = "My Places";
        public static string[] validationFileExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        public static int FileSize = 2000000;

        public static string GetKeySalt()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var KeySALT = builder.Build().GetSection("KeySALT").Value;
            return KeySALT;
        }

        public static string EncryptStrSALT(string PlainText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PlainText))
                {
                    return string.Empty;
                }

                var KeySALT = GetKeySalt();

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                string password = KeySALT;
                byte[] plainText = System.Text.Encoding.Unicode.GetBytes(PlainText);
                byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(password, salt);
                ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainText, 0, plainText.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string encryptedData = Convert.ToBase64String(cipherBytes);
                return encryptedData;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DycryptStrSALT(string EncryptedText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EncryptedText))
                {
                    return string.Empty;
                }

                var KeySALT = GetKeySalt();

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                string password = KeySALT;
                string decryptedData;

                byte[] encryptedData = Convert.FromBase64String(EncryptedText.Replace(' ', '+'));
                byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(password, salt);
                ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(encryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainText = new byte[encryptedData.Length];
                int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                return decryptedData;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
    
}
