using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace QuickWatermarkTool.Models
{
    class Config
    {
        private IConfiguration config;
        public Config()
        {
            config  = new ConfigurationBuilder().AddJsonFile("appSettings.json",optional:false,reloadOnChange:true).Build();
        }

        public int MaxOutputPicWidth => int.Parse(config["MaxOutputPicWidth"]);

    }
}
