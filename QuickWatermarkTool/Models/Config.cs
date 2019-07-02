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
        private int a;
        private float b;
        private bool c;
        private string d;

        public int MaxOutputImageWidth => GetConfig("MaxOutputImageWidth", 2000);
        public int MaxOutputImageHeight => GetConfig("MaxOutputImageHeight", 2000);
        public string WatermarkFilename => GetConfig("WatermarkFilename", "watermark.png");
        public float MaxWatermarkScaleWidth => GetConfig("MaxWatermarkScaleWidth", (float)0.1);
        public float MaxWatermarkScaleHeight => GetConfig("MaxWatermarkScaleHeight", (float)0.1);
        public float WatermarkOpacity => GetConfig("WatermarkOpacity", (float)1.0);
        public int WatermarkOffsetWidth => GetConfig("WatermarkOffsetWidth", 0);
        public int WatermarkOffsetHeight => GetConfig("WatermarkOffsetHeight", 0);
        public Photo.Format DefaultOutputformat { get {
                string rs = iconfig["DefaultOutputformat"];
                switch (rs.ToLower())
                {
                    case "png":
                        return Photo.Format.png;
                    case "gif":
                        return Photo.Format.gif;
                    case "jpg":
                    default:
                        return Photo.Format.jpg;
                }
            } }
        public bool OpenFiledialogOnStartup => GetConfig("OpenFiledialogOnStartup", c);
        public string AuthorName => GetConfig("AuthorName", d);
        public string Copyright => GetConfig("Copyright", d);
        public Photo.WatermarkPosition WatermarkPosition { get {
                string rt = iconfig["WatermarkPosition"];
                switch (rt.ToLower())
                {
                    case "LeftTop":
                        return Photo.WatermarkPosition.LeftTop;
                    case "LeftBottom":
                        return Photo.WatermarkPosition.LeftBottom;

                }
        public string OutputSuffix => GetConfig("OutputSuffix", d);
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
               Boolean result = Boolean.Parse(iconfig[key]);
               return result;
            }
            catch { }
            return defaultvalue;
        }


    }
}
