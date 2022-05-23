using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Common
{
    public interface IConfig
    {
        /// <summary>
        /// Retrieves the default page size count
        /// </summary>
        int DefaultPageSize { get; }
        string DocumentRoot { get; }
    }

    public class Config : IConfig
    {
        /// <summary>
        /// Retrieves the default page size count
        /// </summary>
        public int DefaultPageSize
        {
            get
            {
                int ret;
                if (int.TryParse(ConfigurationManager.AppSettings.Get("DefaultPageSize"), out ret))
                    return ret;
                return 25;
            }
        }

        public string DocumentRoot
        {
            get
            {
                string path = ConfigurationManager.AppSettings.Get("DocumentRoot");
                if (string.IsNullOrEmpty(path))
                    return "~/App_Data/";
                return path;
            }
        }

        public string ProfileImagePath
        {
            get
            {
                string path = ConfigurationManager.AppSettings.Get("ProfileImagePath");
                if (string.IsNullOrEmpty(path))
                    return "~/App_Data/ProfileImagePath/";
                return path;
            }
        }

        public string NomineeImagePath
        {
            get
            {
                string path = ConfigurationManager.AppSettings.Get("NomineeImagePath");
                if (string.IsNullOrEmpty(path))
                    return "~/App_Data/NomineeImagePath/";
                return path;
            }
        }

        public string FingerPrintImagePath
        {
            get
            {
                string path = ConfigurationManager.AppSettings.Get("FingerPrintImagePath");
                if (string.IsNullOrEmpty(path))
                    return "~/App_Data/FingerPrintImagePath/";
                return path;
            }
        }

        public string MedicalFilesPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings.Get("MedicalFilesPath");
                if (string.IsNullOrEmpty(path))
                    return "~/App_Data/MedicalFilesPath/";
                return path;
            }
        }

        public string CsvFilesPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings.Get("CsvFilesPath");
                if (string.IsNullOrEmpty(path))
                    return "~/App_Data/CsvFilesPath/";
                return path;
            }
        }
    }
}
