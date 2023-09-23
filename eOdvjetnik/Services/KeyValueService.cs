using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Services
{
    public class KeyValueService
    {
        private Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        public void SetValue(string key, string value)
        {
            keyValuePairs[key] = value;
        }

        public string GetValue(string key)
        {
            return keyValuePairs.TryGetValue(key, out var value) ? value : null;
        }
    }

}
