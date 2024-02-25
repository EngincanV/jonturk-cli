using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JonTurkCli;

public static class CommandRunner
{
    public static void Run(string commandWithArguments, out string output, string? workingDirectory = null)
    {
        using (var process = new Process())
        {
            process.StartInfo = new ProcessStartInfo(GetShellCommand())
            {
                Arguments = GetArguments(commandWithArguments),
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            if (!string.IsNullOrWhiteSpace(workingDirectory))
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

            process.Start();
            
            using (var standardOutput = process.StandardOutput)
            {
                using (var standardError = process.StandardError)
                {
                    output = standardOutput.ReadToEnd();
                    output += standardError.ReadToEnd();
                }
            }

            process.WaitForExit();
        }
    }

    private static string GetArguments(string commandWithArgs)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "-c \"" + commandWithArgs + "\"";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "/C \"" + commandWithArgs + "\"";
        }

        return string.Empty;
    }
    
    private static string GetShellCommand()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "cmd.exe";
        }
        
        //Linux or OSX
        if (File.Exists("/bin/bash"))
        {
            return "/bin/bash";
        }

        if (File.Exists("/bin/sh"))
        {
            return "/bin/sh";
        }

        throw new Exception("Could not determine shell command for this OS!");
    }
}