using Google.Protobuf.Collections;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

    }
}
