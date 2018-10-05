using System;
using System.Collections.Generic;
using System.Linq;
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
            if (Environment.GetEnvironmentVariable("JAVA_HOME").Length > 0)
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
        }
    }
}
