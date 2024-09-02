using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Matomo.Maui
{
    public class SimpleStorage
    {
        private const string GROUP_NAME = "matomodata";
        private static object _syncRoot = new object();
        private static SimpleStorage _instance;

        public static SimpleStorage Instance
        {
            get
            {
                lock (_syncRoot)
                    if (_instance == null)
                        _instance = new SimpleStorage();
                return _instance;
            }
        }

        private string _filename;
        private Dictionary<string, object> _data;

        public bool HasKey(string key)
        {
            return Preferences.Default.ContainsKey(GetKey(key));
        }

        public string Get(string key)
        {
            return Preferences.Get(GetKey(key), string.Empty);
        }

        public T Get<T>(string key)
        {
            var json = Preferences.Get(GetKey(key), string.Empty);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (!HasKey(key))
            {
                return defaultValue;
            }

            return Get<T>(key);
        }

        public void Put<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            Preferences.Default.Set(GetKey(key), json);
        }

        private string GetKey(string nonGroupedKey)
        {
            return GROUP_NAME + nonGroupedKey;
        }
    }
}

