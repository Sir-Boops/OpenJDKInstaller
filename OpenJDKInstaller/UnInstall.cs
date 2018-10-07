using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace OpenJDKInstaller
{
    class UnInstall
    {
        public UnInstall()
        {

            // Delete the Java Folder
            var JAVA_FOLDER = new DirectoryInfo(Directory.GetParent(Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine).ToString()).ToString());
            if (JAVA_FOLDER.Exists)
            {
                JAVA_FOLDER.Delete(true);
            }

            // Remove the JAVA_HOME Env var
            Environment.SetEnvironmentVariable("JAVA_HOME", null, EnvironmentVariableTarget.Machine);

            // Remove Java from the System PATH
            String ENV_PATH = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            String PATH_RAW = Registry.LocalMachine.CreateSubKey(ENV_PATH).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();
            String[] PATH_ARR = PATH_RAW.Split(';');
            String NEW_PATH = "";

            for (int i = 0; i < PATH_ARR.Length; i++)
            {
                if (!PATH_ARR[i].ToLower().Contains("%java_home%") && !PATH_ARR[i].ToLower().Contains(@"c:\program files\java"))
                {
                    int I_CHECK = (i + 1);
                    if (I_CHECK >= PATH_ARR.Length)
                    {
                        NEW_PATH += PATH_ARR[i];
                    } else
                    {
                        NEW_PATH += (PATH_ARR[i] + ";");
                    }
                }
            }


            if (NEW_PATH.Split(';').Length >= 1)
            {
                Registry.LocalMachine.CreateSubKey(ENV_PATH).SetValue("Path", NEW_PATH);
            } else
            {
                Console.WriteLine("Error Creating new path try again");
                Console.WriteLine("Press enter to close");
                Console.Read();
                Environment.Exit(0);
            }
        }
    }
}
