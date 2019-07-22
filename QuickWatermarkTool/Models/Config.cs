using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuickWatermarkTool.Models
{
    public class Config
    {
        public static Config config;
        private IConfiguration iconfig;
        private JObject jconfig;
        private string configFilename;
        public Config(string filename = "config.json")
        {
            configFilename = filename;
            iconfig = new ConfigurationBuilder().AddJsonFile(configFilename, optional: false, reloadOnChange: true).Build();
            TextReader configFileReader = new StreamReader(configFilename);
            jconfig = JObject.Parse(configFileReader.ReadToEnd());
            jconfig.PropertyChanged += Jconfig_Changed;
        }

        private void Jconfig_Changed(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            using (TextWriter configFileWriter = new StreamWriter(configFilename, false))
            {
                configFileWriter.WriteLine(jconfig.ToString());
            }
        }

        public int MaxOutputImageWidth
        {
            get => GetConfig("MaxOutputImageWidth", 2000);
            set => jconfig["MaxOutputImageWidth"] = value;
        }

        public int MaxOutputImageHeight
        {
            get => GetConfig("MaxOutputImageHeight", 2000);
            set => jconfig["MaxOutputImageHeight"] = value;
        }

        public string WatermarkFilename
        {
            get => GetConfig("WatermarkFilename", "watermark.png");
            set => jconfig["WatermarkFilename"] = value;
        }

        public float MaxWatermarkScaleWidth
        {
            get => GetConfig("MaxWatermarkScaleWidth", (float) 0.1);
            set => jconfig["MaxWatermarkScaleWidth"] = value;
        }

        public float MaxWatermarkScaleHeight
        {
            get => GetConfig("MaxWatermarkScaleHeight", (float) 0.1);
            set => jconfig["MaxWatermarkScaleHeight"] = value;
        }

        public float WatermarkOpacity
        {
            get => GetConfig("WatermarkOpacity", (float) 1.0);
            set => jconfig["WatermarkOpacity"] = value;
        }

        public int WatermarkOffsetWidth
        {
            get => GetConfig("WatermarkOffsetWidth", 0);
            set => jconfig["WatermarkOffsetWidth"] = value;
        }

        public int WatermarkOffsetHeight
        {
            get => GetConfig("WatermarkOffsetHeight", 0);
            set => jconfig["WatermarkOffsetHeight"] = value;
        }

        public Photo.Format DefaultOutputFormat {
            get {
                string rs = iconfig["DefaultOutputFormat"];
                try
                {
                    return Enum.Parse<Photo.Format>(rs);
                }
                catch
                {
                    return Photo.Format.jpg;
                }
            }
            set => jconfig["DefaultOutputFormat"] = Enum.GetName(typeof(Photo.Format), value);
        }
        public bool OpenFileDialogOnStartup
        {
            get => GetConfig("OpenFileDialogOnStartup", true);
            set => jconfig["OpenFileDialogOnStartup"] = value;
        }

        public string AuthorName
        {
            get => GetConfig("AuthorName", "");
            set => jconfig["AuthorName"] = value;
        }

        public string Copyright
        {
            get => GetConfig("Copyright", "");
            set => jconfig["Copyright"] = value;
        }

        public Photo.WatermarkPosition WatermarkPosition
        {
            get
            {
                string rt = iconfig["WatermarkPosition"];
                try
                {
                    return Enum.Parse<Photo.WatermarkPosition>(rt, true);
                }
                catch
                {
                    return Photo.WatermarkPosition.Center;
                }
            }
            set => jconfig["WatermarkPosition"] = Enum.GetName(typeof(Photo.WatermarkPosition), value);
        }

        public string OutputSuffix
        {
            get => GetConfig("OutputSuffix", "_watermarked");
            set => jconfig["OutputSuffix"] = value;
        }

        public string GetConfig(string key, string defaultvalue)
        {
            try
            {
                string result = iconfig[key];
                if (!string.IsNullOrEmpty(result))
                    return result;
                else
                    throw new ArgumentNullException();
            }
            catch
            {
                return defaultvalue;
            }
        }
        public int GetConfig(string key, int defaultvalue)
        {
            try
            {
                int result = int.Parse(iconfig[key]);
                return result;
            }
            catch
            {
                return defaultvalue;
            }
        }
        public float GetConfig(string key, float defaultvalue)
        {
            try
            {
                float result = float.Parse(iconfig[key]);
                return result;
            }
            catch
            {
                return defaultvalue;
            }
        }
        public bool GetConfig(string key, bool defaultvalue)
        {
            try
            {
                bool result = bool.Parse(iconfig[key]);
                return result;
            }
            catch
            {
                return defaultvalue;
            }
        }


    }
}
