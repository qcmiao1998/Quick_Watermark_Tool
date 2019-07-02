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
        
        public int MaxOutputImageWidth { get; }
        public int MaxOutputImageHeight { get; }
        public string WatermarkFilename { get; }
        public float MaxWatermarkScaleWidth { get; }
        public float MaxWatermarkScaleHeight { get; }
        public float MaxWatermarkOpaeity { get; }
        public int WatermarkOffset { get; }
        public string DefactOutputformat { get; }
        public bool OpenFiledialogOnStartup { get; }
        public string Authorname { get; }
        public string CopyRight { get; }
        public string WatermarkPosition { get; }
        public string OutputSuffix { get; }
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
        public bool GetConfig(string key, bool defaultvalue)
        {
            try
            {
                bool result = bool.Parse(iconfig[key]);

                return result;
            }
            catch { }
            return defaultvalue;
        }


    }
}
