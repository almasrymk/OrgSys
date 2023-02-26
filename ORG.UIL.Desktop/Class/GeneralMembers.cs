using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Xml;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Net;
using ORGRepository;
using ORGEntity;

namespace ORG.UIL
{ 
    public delegate object Event_G<t>(params object[] ob) where t : class;
    public delegate void _DoPrint(_OBTypePrint TypePrint, long ID);
    public delegate object _SearchIDID2ID3ID4ID5ID6ID7ID8(long ID, long ID2, long ID3, long ID4, long UnitID, long CurrID, long trnsID, long invID);
    public delegate object _SearchByTextAndID(string TextSearch, long ID);
    public delegate object _SearchByTextAndIDID2ID3(string TextSearch, long ID, long ID2, long ID3);
    public delegate object _SearchByTextText2AndIDID2(string TextSearch, string TextSearch2, long ID, long ID2);
    public delegate object _SearchByID(object _object);
    public delegate object _SearchByIDID2(object _object, object _object2);
    public delegate object _SearchByIDID4(object _object, object _object2, object _object3, object _object4);
    public delegate object _SearchByIDID2ID3(long ID, long ID2, long ID3);
    public delegate object _SearchByIDID2ID3ID4ID5(long ID, long ID2, long ID3, long ID4, long ID5);
    public delegate object _SearchByobject(object _object);
    public delegate object _SearchItems(object _object, long ID, long ID2);
    public delegate object _SelectCategtory(int Id , string textSearch);

    //New Event
    public delegate void _SearchEvent(string textSearch);
    public delegate void _Pagging(string textSearch , int page);
    public delegate void _Select(int Id);
    public delegate object _SelectOb(int Id);
    public delegate void _evet();

    public enum typeEntery { Treasury, Classic, Cashier, ClassicCashier }
    public enum typeTrans { Treasury, Trasnaction, Invoice }
    public enum StoreSide { from, to }
    public enum _OBTypePrint { A4, PaperRoll , PaperRollSplited }
    public enum _ProductionMode { Requist, Production , Distribution }

    public enum _StatusMode { Requist, IssueProduction, Production , Initialize , DoneProduction, Distribution , Donedistribution , CancelRequest , CancelProduction , CancelDistribution }

    class GeneralMembers
    {
        static string _Lang = "en";
        public static string Lang
        {
            get { return _Lang; }
            set { _Lang = value; }
        }
    
        public static UsersApp User { get; set; }

        public static DataTable ConvertToDataTable<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                // HERE IS WHERE THE ERROR IS THROWN FOR NULLABLE TYPES
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
                prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        }

        public static bool Onlydecimal(string txt, char _char)
        {
            decimal x = 0;
            if ((!decimal.TryParse(_char.ToString(), out x) && _char != '\b' && _char != '.') || (_char == '.' && txt.Contains('.')))
                return true;
            return false;
        }

        public static bool OnlyNumber(char _char)
        {
            Int64 x = 0;
            if (!Int64.TryParse(_char.ToString(), out x) && _char != '\b')
                return true;
            return false;
        }

        public static string Round(string Number, int NumberFloat)
        {
            if (Number == "") Number = "0";
            return string.Format("{0:0." + new string('0', NumberFloat) + "}", decimal.Parse(Number));
        }

        public static string GetHardSerial()
        {
            return identifier("Win32_DiskDrive", "Model").Trim() + "#" + identifier("Win32_DiskDrive", "SerialNumber").Trim();
        }

        public static string GetComputerIP()
        {
            String url = "http://bot.whatismyipaddress.com/";
            String result = null;

            try
            {
                WebClient client = new WebClient();
                result = client.DownloadString(url);
                return result;
            }
            catch (Exception ex) { return "127.0.0.1"; }
        }

        public static string GetProcesserID()
        {
            string result = "";

            try
            {
                ManagementObjectCollection mbsList = null;
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                mbsList = mbs.Get();
                foreach (ManagementObject mo in mbsList)
                {
                    result = mo["ProcessorID"].ToString();
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        private static string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";           
            try
            {
                ManagementClass mc = new ManagementClass(wmiClass);
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }            
        }

        public static string EncryptString(string clearText)
        {
            string EncryptionKey = "123456789012345";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptString(string cipherText)
        {
            string EncryptionKey = "123456789012345";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static void Encrypt(XmlDocument Doc, string ElementName, SymmetricAlgorithm Key)
        {
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementName == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (Key == null)
                throw new ArgumentNullException("Alg");

            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementName)[0] as XmlElement;
            if (elementToEncrypt == null)
            {
                throw new XmlException("The specified element was not found");

            }

            EncryptedXml eXml = new EncryptedXml();

            byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, Key, false);

            EncryptedData edElement = new EncryptedData();
            edElement.Type = EncryptedXml.XmlEncElementUrl;

            string encryptionMethod = null;

            if (Key is TripleDES)
            {
                encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
            }
            else if (Key is DES)
            {
                encryptionMethod = EncryptedXml.XmlEncDESUrl;
            }
            if (Key is Rijndael)
            {
                switch (Key.KeySize)
                {
                    case 128:
                        encryptionMethod = EncryptedXml.XmlEncAES128Url;
                        break;
                    case 192:
                        encryptionMethod = EncryptedXml.XmlEncAES192Url;
                        break;
                    case 256:
                        encryptionMethod = EncryptedXml.XmlEncAES256Url;
                        break;
                }
            }
            else
            {
                throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
            }

            edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);
            edElement.CipherData.CipherValue = encryptedElement;
            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
        }

        public static void Decrypt(XmlDocument Doc, SymmetricAlgorithm Alg)//, string KeyName)
        {
            if (Doc == null)
                return;
            if (Alg == null)
                return;

            XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;
            if (encryptedElement == null)
            {
                return;
            }

            EncryptedData edElement = new EncryptedData();
            edElement.LoadXml(encryptedElement);
            EncryptedXml exml = new EncryptedXml();

            byte[] rgbOutput = exml.DecryptData(edElement, Alg);

            exml.ReplaceData(encryptedElement, rgbOutput);
        }

        public static bool OnlyInt(string TxtValue)
        {
            int x = 0;
            return !int.TryParse(TxtValue, out x);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateDatabase()
        {
        }
    }
}
