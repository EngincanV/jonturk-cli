namespace JonTurkCli;

public static class CliConsts
{
    public static readonly string JonTurkRootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".jonturk");

    public static readonly string JonTurkSaveFilePath = Path.Combine(JonTurkRootFolderPath, "commands.json");
    
    public static readonly string JonTurkScriptsFolderPath = Path.Combine(JonTurkRootFolderPath, "scripts");
}