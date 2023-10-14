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

                    // Generate a random key
                    aesAlg.GenerateKey();

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
                        // If the key already exists, update its value, IV, and key
                        preferenceElement.SetAttributeValue("Value", Convert.ToBase64String(encryptedValue));
                        preferenceElement.SetAttributeValue("IV", Convert.ToBase64String(aesAlg.IV));
                        preferenceElement.SetAttributeValue("AESKey", Convert.ToBase64String(aesAlg.Key)); // Use a different attribute name
                    }
                    else
                    {
                        // If the key doesn't exist, add a new key-value pair
                        XElement newPreferenceElement = new XElement("Preference",
                            new XAttribute("Key", key),
                            new XAttribute("Value", Convert.ToBase64String(encryptedValue)),
                            new XAttribute("IV", Convert.ToBase64String(aesAlg.IV)),
                            new XAttribute("AESKey", Convert.ToBase64String(aesAlg.Key)) // Use a different attribute name
                        );
                        xmlDoc.Root.Add(newPreferenceElement);
                    }
                    xmlDoc.Save(FilePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("U trecoj sreci u setu " + ex.Message);
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
                            // You can throw an exception or return an appropriate error message.
                            return null; // Return null to indicate the error.
                        }

                        // Get the key from the stored data
                        byte[] storedKey = Convert.FromBase64String(preferenceElement.Attribute("AESKey").Value); // Corrected attribute name
                        aesAlg.Key = storedKey;

                        // Decrypt the value
                        byte[] encryptedValue = Convert.FromBase64String(preferenceElement.Attribute("Value").Value);
                        using (var decryptor = aesAlg.CreateDecryptor())
                        {
                            byte[] decryptedValue = decryptor.TransformFinalBlock(encryptedValue, 0, encryptedValue.Length);
                            Debug.WriteLine(Encoding.UTF8.GetString(decryptedValue));
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

    }
}
