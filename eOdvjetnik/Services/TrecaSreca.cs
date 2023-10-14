using Google.Protobuf.Collections;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace eOdvjetnik.Services
{
    public class TrecaSreca
    {
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "preferences.xml");


        public static void CreateXmlFile()
        {
            try
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
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public static void Set(string key, string value)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    // Generate a random IV (Initialization Vector) for each encryption
                    aesAlg.GenerateIV();

                    // Manually define a unique security salt (make sure it's different for each piece of data)
                    byte[] salt = Encoding.UTF8.GetBytes("CFQp4uSd2Dp1oi3SEb5ff5cjwRy9WdJJ");

                    // Trim or expand the salt to match the key size
                    Array.Resize(ref salt, aesAlg.Key.Length);

                    // Set the key using the combined salt and key
                    aesAlg.Key = salt;

                    // Encrypt the value
                    byte[] encryptedValue;
                    using (var encryptor = aesAlg.CreateEncryptor())
                    {
                        byte[] valueBytes = Encoding.UTF8.GetBytes(value);
                        encryptedValue = encryptor.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
                    }

                    XDocument xmlDoc = XDocument.Load(FilePath);

                    XElement preferenceElement = xmlDoc.Root
                        .Elements("Preference")
                        .FirstOrDefault(p => p.Attribute("Key").Value == key);

                    if (preferenceElement != null)
                    {
                        // If the key already exists, update its value and IV
                        preferenceElement.SetAttributeValue("Value", Convert.ToBase64String(encryptedValue));
                        preferenceElement.SetAttributeValue("IV", Convert.ToBase64String(aesAlg.IV));
                    }
                    else
                    {
                        // If the key doesn't exist, add a new key-value pair with IV
                        XElement newPreferenceElement = new XElement("Preference",
                            new XAttribute("Key", key),
                            new XAttribute("Value", Convert.ToBase64String(encryptedValue)),
                            new XAttribute("IV", Convert.ToBase64String(aesAlg.IV))
                        );
                        xmlDoc.Root.Add(newPreferenceElement);
                    }
                    xmlDoc.Save(FilePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static string Get(string key)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    XDocument xmlDoc = XDocument.Load(FilePath);
                    XElement preferenceElement = xmlDoc.Root
                        .Elements("Preference")
                        .FirstOrDefault(p => p.Attribute("Key").Value == key);

                    if (preferenceElement != null)
                    {
                        // Check if the "IV" attribute exists
                        XAttribute ivAttribute = preferenceElement.Attribute("IV");
                        if (ivAttribute != null)
                        {
                            byte[] storedIV = Convert.FromBase64String(ivAttribute.Value);
                            aesAlg.IV = storedIV;
                        }
                        else
                        {
                            // Handle the case where "IV" attribute is missing or null
                            return null; // Return null to indicate the error.
                        }

                        // Manually define the same fixed salt for both encryption and decryption
                        byte[] salt = Encoding.UTF8.GetBytes("CFQp4uSd2Dp1oi3SEb5ff5cjwRy9WdJJ");

                        // Ensure that the key size matches the size of the generated key
                        byte[] keyBytes = new byte[aesAlg.Key.Length];
                        Array.Copy(salt, keyBytes, Math.Min(salt.Length, aesAlg.Key.Length));
                        aesAlg.Key = keyBytes;

                        // Decrypt the value
                        byte[] encryptedValue = Convert.FromBase64String(preferenceElement.Attribute("Value").Value);
                        using (var decryptor = aesAlg.CreateDecryptor())
                        {
                            byte[] decryptedValue = decryptor.TransformFinalBlock(encryptedValue, 0, encryptedValue.Length);
                            return Encoding.UTF8.GetString(decryptedValue);
                        }
                    }
                    else
                    {
                        return null; // Key not found
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Get method: " + ex.Message);
                return null;
            }
        }




        public static void Update(string key, string newValue, bool protectedString = false)
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

    }
}
