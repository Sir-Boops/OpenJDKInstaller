using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenJDKInstaller
{
    class Base
    {
        static void Main(string[] args)
        {
            // Fetch OpenJDK json
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://git.sergal.org/Sir-Boops/OpenJDKJSON/raw/branch/master/OpenJDK.json");
            StreamReader reader = new StreamReader(stream);
            String OPENJDK_JSON_RAW = reader.ReadToEnd();
            OpenJDKJSON OPENJDK_JSON = JsonConvert.DeserializeObject<OpenJDKJSON>(OPENJDK_JSON_RAW);

            // Make sure we are on a 64-bit OS
            if (!System.Environment.Is64BitOperatingSystem)
            {
                Console.WriteLine("Sorry, OpenJDK version currently only support 64-bit operating systems!");
                Console.WriteLine("Press enter to close");
                Console.Read();
                Environment.Exit(0);
            }

            // Gen a String of supported openJDK versions
            String SUPPORTED_JDK_VER = "";
            foreach (String ver in OPENJDK_JSON.supported_versions)
            {
                SUPPORTED_JDK_VER += (ver + " ");
            }

            // Request what version is to be installed
            Console.WriteLine("Hi, What version of OpenJDK would you like to Install?");
            Console.WriteLine("Current Options are: " + SUPPORTED_JDK_VER);
            Console.WriteLine("");

            String REQUESTED_VERSION = Console.ReadLine();

            // Make sure the user requested something sane
            if (!OPENJDK_JSON.supported_versions.Contains(REQUESTED_VERSION))
            {
                Console.WriteLine("You have to choose a supported version!");
                Console.WriteLine("Press enter to close");
                Console.Read();
                Environment.Exit(0);
            }

            new Install(REQUESTED_VERSION);

        }
    }

    public class OpenJDKJSON
    {
        public String[] supported_versions { get; set; }
    }

}
