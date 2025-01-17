﻿using System.IO;
using Microsoft.Extensions.Configuration;


namespace StudentInformationSystem.Utils
{
    public class DbConnUtil
    {
        public static IConfiguration _iconfiguration;

        static DbConnUtil()
        {
            GetAppSettingsFile();
        }

        public static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())//sets the base path where configuration file is located
                .AddJsonFile("appSettings.json");
            _iconfiguration = builder.Build();
        }
       public static string GetConnString()
        {
            
            return _iconfiguration.
                GetConnectionString("LocalConnectionString");
        }

    }
}
