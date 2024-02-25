using CliFx;

namespace JonTurkCli;

public class Program
{
    public static async Task<int> Main()
    {
        CreateJonTurkRootPathIfNotExists();

        return await new CliApplicationBuilder()
            .SetExecutableName("jonturk")
            .SetTitle("JonTurk CLI")
            .SetDescription("A command line tool that allows you to save, list and run the frequently used CLI commands by you.")
            .AddCommandsFromThisAssembly()
            .Build()
            .RunAsync();
    }

    private static void CreateJonTurkRootPathIfNotExists()
    {
        if (Directory.Exists(CliConsts.JonTurkRootFolderPath))
        {
            return;
        }

        Directory.CreateDirectory(CliConsts.JonTurkRootFolderPath);
    }
}