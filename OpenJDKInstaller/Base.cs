using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenJDKInstaller
{
    class Base
    {
        static void Main(string[] args)
        {

            // Define OpenJDK versions
            List<String> OPENJDK_LIST = new List<String>();
            OPENJDK_LIST.Add("10");
            OPENJDK_LIST.Add("11");

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
            foreach (String ver in OPENJDK_LIST)
            {
                SUPPORTED_JDK_VER += (ver + " ");
            }

            // Request what version is to be installed
            Console.WriteLine("Hi, What version of OpenJDK would you like to Install?");
            Console.WriteLine("Current Options are: " + SUPPORTED_JDK_VER);
            Console.WriteLine("");

            String REQUESTED_VERSION = Console.ReadLine();

            // Make sure the user requested something sane
            if (!OPENJDK_LIST.Contains(REQUESTED_VERSION))
            {
                Console.WriteLine("You have to choose a supported version!");
                Console.WriteLine("Press enter to close");
                Console.Read();
                Environment.Exit(0);
            }

            new Install(REQUESTED_VERSION);

        }
    }
}
