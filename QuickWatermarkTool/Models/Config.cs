using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace QuickWatermarkTool.Models
{
    public class Config
    {
        public static Config config;
        private IConfiguration iconfig;
        public Config()
        {
            iconfig = new ConfigurationBuilder().AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }
        public string GetConfig(string key, string defaultvalue)
        {
            try
            {
                string result = iconfig[key];
                if (result != String.Empty)
                    return result;
            }
            catch { }
            return defaultvalue;
        }
        public int GetConfig(string key, int defaultvalue)
        {
            try
            {
                int result = int.Parse(iconfig[key]);
                if (result != 0)
                    return result;
            }
            catch { }
            return defaultvalue;
        }
        public float GetConfig(string key, float defaultvalue)
        {
            try
            {
                float result = float.Parse(iconfig[key]);
                if (result != 0)
                    return result;
            }
            catch { }
            return defaultvalue;
        }
        public Boolean GetConfig(string key, Boolean defaultvalue)
        {
            try
            {
               Boolean result = Boolean.Parse(iconfig[key]);
               return result;
            }
            catch { }
            return defaultvalue;
        }
        public int MaxOutputImageWidth { get {
                if(int.TryParse(iconfig["MaxOutputImageWidth"], out int r))
                return r;
                return 1000;
            } }
        public int MaxOutputImageHeight
        {
            get
            {
                if (int.TryParse(iconfig["MaxOutputImageHeight"], out int r))
                    return r;
                return 1000;
            }
        }


    }
}
