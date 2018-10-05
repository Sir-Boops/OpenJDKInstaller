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
            var JAVA_FOLDER = new DirectoryInfo(Directory.GetParent(Environment.GetEnvironmentVariable("JAVA_HOME").ToString()).ToString());
            if (JAVA_FOLDER.Exists)
            {
                JAVA_FOLDER.Delete(true);
            }

            // Remove the JAVA_HOME Env var
            //Environment.SetEnvironmentVariable("JAVA_HOME", null, EnvironmentVariableTarget.Machine);

            // Remove Java from the System PATH
            String[] PATH = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine).Split(';');
            String NEW_PATH = "";


            for (int i = 0; i < PATH.Length; i++)
            {

                if (!PATH[i].ToLower().Contains(JAVA_FOLDER.ToString().ToLower()))
                {
                    int I_CHECK = (i + 1);
                    if (I_CHECK >= PATH.Length )
                    {
                        NEW_PATH += (PATH[i]);
                    } else
                    {
                        NEW_PATH += (PATH[i] + ";");
                    }

                }
            }

            Console.WriteLine(Environment.GetEnvironmentVariable("PATH"));
            Console.Read();
            Environment.SetEnvironmentVariable("HOME_TEST", NEW_PATH, EnvironmentVariableTarget.Machine);

        }
    }
}
