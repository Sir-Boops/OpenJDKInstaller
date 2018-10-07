using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenJDKInstaller
{
    class Install
    {
        public Install(String REQUESTED_VER)
        {
            // Check to See if there is a version already installed
            // And if it is remove it
            if (Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine) != null)
            {
                // Java in already installed so remove the old System envs
                Console.WriteLine("Warning found already installed Java version");
                Console.WriteLine("This will be deleted if you continue...Are you sure?");
                String INPUT = Console.ReadLine();

                if (!INPUT.ToLower().Equals("y"))
                {
                    Console.WriteLine("Press enter to close");
                    Console.Read();
                    Environment.Exit(0);
                }
                new UnInstall();
            }

            // Grab the JSON for the requested version
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://git.sergal.org/Sir-Boops/OpenJDKJSON/raw/branch/master/" + REQUESTED_VER + ".json");
            StreamReader reader = new StreamReader(stream);
            String OPENJDK_JSON_RAW = reader.ReadToEnd();
            OpenJDKSubJSON OPENJDK_SUB_JSON = JsonConvert.DeserializeObject<OpenJDKSubJSON>(OPENJDK_JSON_RAW);

            // Get the filename
            String[] URL_ARR = OPENJDK_SUB_JSON.url.Split('/');
            String FileName = URL_ARR[URL_ARR.Length - 1];

            // Download the rquested version
            WebClient webClient = new WebClient();
            webClient.DownloadFile(OPENJDK_SUB_JSON.url, Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\" + FileName));

            // Calc the sha256 of the downloaded file!
            SHA256 mySHA256 = SHA256Managed.Create();
            FileStream DL_FILE = new FileStream(Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\" + FileName), FileMode.Open);
            DL_FILE.Position = 0;
            byte[] hashValue = mySHA256.ComputeHash(DL_FILE);
            String COMP_HASH = BitConverter.ToString(hashValue).Replace("-", String.Empty).ToLower();
            DL_FILE.Close();

            if (!COMP_HASH.Equals(OPENJDK_SUB_JSON.sha256sum))
            {
                Console.WriteLine("Error downloading and verifying file");
                Console.WriteLine("Press enter to close");
                Console.Read();
                Environment.Exit(0);
            }

            // Create the Java Folder and extract the archive
            String JAVA_DIR_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Java\";
            System.IO.Directory.CreateDirectory(JAVA_DIR_PATH);
            ZipFile.ExtractToDirectory(Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\" + FileName), JAVA_DIR_PATH);

            // Create the ENV Vars
            var directories = Directory.GetDirectories(JAVA_DIR_PATH);

            String JAVA_HOME = (directories[0]);
            String JAVA_PATH = (@"%JAVA_HOME%\bin");


            // Set the System Vars

            Environment.SetEnvironmentVariable("JAVA_HOME", JAVA_HOME, EnvironmentVariableTarget.Machine);

            String ENV_PATH = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            String PATH_RAW = Registry.LocalMachine.CreateSubKey(ENV_PATH).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();
            String NEW_PATH = (PATH_RAW + ";" + JAVA_PATH);
            Registry.LocalMachine.CreateSubKey(ENV_PATH).SetValue("Path", NEW_PATH);
        }
    }

    public class OpenJDKSubJSON
    {
        public String url { get; set; }
        public String sha256sum { get; set; }
    }

}
