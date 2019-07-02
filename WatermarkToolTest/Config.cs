using NUnit.Framework;
using QuickWatermarkTool.Models;

namespace Tests
{
    public class ConfigTest
    {
        Config config;

        [SetUp]
        public void Setup()
        {
            config = new Config();
        }

        [Test]
        public void GetInt()
        {
            config.GetConfig<int>("111", 111);
        }
    }
}