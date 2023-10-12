using Google.Protobuf.Collections;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text;
using System.Security.Cryptography;

namespace eOdvjetnik.Services
{
    public class TrecaSreca
    {
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "preferences.xml");


        public static void CreateXmlFile()
        {
            if (!File.Exists(FilePath))
            {
                XDocument xmlDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Preferences")
                );

                xmlDoc.Save(FilePath);
                Debug.WriteLine(FilePath);
            }
        }

        public static void Set(string key, string value)
        {
            XDocument xmlDoc = XDocument.Load(FilePath);

            XElement preferenceElement = xmlDoc.Root
                .Elements("Preference")
                .FirstOrDefault(p => p.Attribute("Key").Value == key);

            if (preferenceElement != null)
            {
                // If the key already exists, update its value
                preferenceElement.SetAttributeValue("Value", value);
            }
            else
            {
                // If the key doesn't exist, add a new key-value pair
                XElement newPreferenceElement = new XElement("Preference",
                    new XAttribute("Key", key),
                    new XAttribute("Value", value)
                );
                xmlDoc.Root.Add(newPreferenceElement);
            }
            xmlDoc.Save(FilePath);
        }

        public static string Get(string key)
        {
            XDocument xmlDoc = XDocument.Load(FilePath);
            XElement preferenceElement = xmlDoc.Root
                .Elements("Preference")
                .FirstOrDefault(p => p.Attribute("Key").Value == key);

            if (preferenceElement != null)
            {
                return preferenceElement.Attribute("Value").Value;
            }
            else
            {
                return null; // Key not found
            }
        }

        public static void Update(string key, string newValue)
        {
            XDocument xmlDoc = XDocument.Load(FilePath);

            XElement preferenceElement = xmlDoc.Root
                .Elements("Preference")
                .FirstOrDefault(p => p.Attribute("Key").Value == key);

            if (preferenceElement != null)
            {
                preferenceElement.SetAttributeValue("Value", newValue);
                xmlDoc.Save(FilePath);
            }
        }

        public static void Remove(string key)
        {
            XDocument xmlDoc = XDocument.Load(FilePath);

            XElement preferenceElement = xmlDoc.Root
                .Elements("Preference")
                .FirstOrDefault(p => p.Attribute("Key").Value == key);

            if (preferenceElement != null)
            {
                preferenceElement.Remove();
                xmlDoc.Save(FilePath);
            }
        }

        public static void Clear()
        {
            XDocument xmlDoc = XDocument.Load(FilePath);
            xmlDoc.Root.Elements().Remove();
            xmlDoc.Save(FilePath);
        }


        private static byte[] GenerateRandomKey()
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            return aes.Key;
        }

        private static byte[] GenerateRandomIV()
        {
            using var aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }



        public static void init()
        {
            string original = "Hello, world!";
            byte[] key = GenerateRandomKey();
            byte[] iv = GenerateRandomIV();

            var salt = "xJAfN9BuVRf+KvPpXxb/QCbqhPfqiB09RDPWyFcJGkZEvMZi+mSnP74xsmEmAdgirrnrxjJ/hrW2cz4HnqnPQkcUg=";






            Set("license", Convert.ToBase64String(key));
            Set("activation_key", Convert.ToBase64String(iv));


            byte[] key2 = Convert.FromBase64String(Get("license"));
            byte[] iv2 = Convert.FromBase64String(Get("activation_key"));

            /*
             * 
             * 
        sb.AppendLine($"Model: {Microsoft.Maui.Devices.DeviceInfo.Current.Model}");
        sb.AppendLine($"Manufacturer: {Microsoft.Maui.Devices.DeviceInfo.Current.Manufacturer}");
        sb.AppendLine($"Name: {Microsoft.Maui.Devices.DeviceInfo.Current.Name}");
        sb.AppendLine($"OS Version: {Microsoft.Maui.Devices.DeviceInfo.Current.VersionString}");
        sb.AppendLine($"Idiom: {Microsoft.Maui.Devices.DeviceInfo.Current.Idiom}");
        sb.AppendLine($"Platform: {Microsoft.Maui.Devices.DeviceInfo.Current.Platform}");

        Model: X570 AORUS ELITE
        Manufacturer: Gigabyte Technology Co., Ltd.
        Name: DESKTOP-NGMDCMT
        OS Version: 10.0.19045.3570
        Idiom: Desktop
        Platform: WinUI
             *
             *
             */







            //Primjer ispod
            byte[] encrypted = EncryptString(original, key, iv);
            string decrypted = DecryptString(encrypted, key, iv);

            Debug.WriteLine("************************************************************");
            Debug.WriteLine($"Original: {original}");
            Debug.WriteLine($"Encrypted b64: {Convert.ToBase64String(encrypted)}");
            Debug.WriteLine($"Decrypted: {decrypted}");
            Debug.WriteLine("************************************************************");
        }


        private static byte[] EncryptString(string plaintext, byte[] key, byte[] iv){
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor();
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
            return encryptedBytes;
        }

        private static string DecryptString(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            string plaintext = Encoding.UTF8.GetString(decryptedBytes);
            return plaintext;
        }

    }
}
