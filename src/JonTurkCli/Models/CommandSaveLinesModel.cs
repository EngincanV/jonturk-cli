namespace JonTurkCli.Models;

public class CommandSaveLinesModel
{
    public List<CommandSaveLineModel> Commands { get; set; }

    public CommandSaveLinesModel()
    {
        Commands = new();
    }
}