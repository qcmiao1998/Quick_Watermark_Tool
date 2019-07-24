# Quick Watermark Tool

Quick Watermark Tool is a .Net Core based image watermarking tool which supports Windows, Linux and Mac OSX.

## Function

This tool provides an easy way to get photos watermarked. It has great advantages in personal blogs, Wechat Official Accounts, etc.

This tool has both graphic and command line user interface. GUI will not show if args passed from command line.

The command line usage is:
```
QuickWatermarkTool <files> [-o|--out <outputpath>] [-c|--config <configfile>]  
```

The default watermark image is watermark.png under base folder of program. The comfig file is config.json, you can modify settings in GUI or edit the config file directly. By exchange the config file, it is easy to keep watermark style consistent in organizations.

## Contributing

The original program is [DXPRESS-ECNU/dxpress-watermark](https://github.com/DXPRESS-ECNU/dxpress-watermark) developed by [Q. Miao](https://github.com/qcmiao1998).

This repo is maintained by [Q. Miao](https://github.com/qcmiao1998), and the first version thanks to the contribution from [T. Wang](10175300149@stu.ecnu.edu.cn) and [J. Li](https://github.com/RicardojlLi).
