using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureBlobStorageSample
{
    public static class AccountManager
    {
        public static string config = "../../SetupFiles/keys.config";
        public static string setup = "../../SetupFiles/Setup.ps1";
        public static string GetAzureAccount()
        {
            string ConnectionInfo = string.Empty;
            if (!File.Exists(config))
            {
                Runspace space = RunspaceFactory.CreateRunspace();
                space.Open();
                Pipeline pipeline = space.CreatePipeline();
                pipeline.Commands.AddScript(File.ReadAllText(setup));
                var connectionInfo = pipeline.Invoke();
                Thread.Sleep(250);
            }
            if (File.Exists(config))
            {
                ConnectionInfo = File.ReadAllText(config);
            }
            else
                throw new Exception("Could not set up account...See setup problems in Solution files");
            return ConnectionInfo;
        }
    }
}
