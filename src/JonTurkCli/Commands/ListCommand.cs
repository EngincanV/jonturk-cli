using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JonTurkCli.Models;
using Spectre.Console;

namespace JonTurkCli.Commands;

[Command(Description = "Lists the saved commands.")]
public class ListCommand : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var saveFilePath = CliConsts.JonTurkSaveFilePath;
        if (!File.Exists(saveFilePath))
        {
            AnsiConsole.MarkupLine("[red]There is no saved command![/]");
            return;
        }

        try
        {
            var fileContent = await File.ReadAllTextAsync(saveFilePath);

            var commandSaveLines = JsonSerializer.Deserialize<CommandSaveLinesModel>(fileContent);
            if (commandSaveLines == null || !commandSaveLines.Commands.Any())
            {
                AnsiConsole.MarkupLine("[red]There is no saved command![/]");
                return;
            }

            PrintCommandsToConsole(commandSaveLines.Commands);
        }
        catch
        {
            //ignore...
        }
    }

    private static void PrintCommandsToConsole(List<CommandSaveLineModel> commands)
    {
        var table = new Table();

        table.AddColumn("Name");
        table.AddColumn("Command");

        foreach (var (name, command) in commands)
        {
            table.AddRow($"[blue]{name}[/]", $"[green]{command}[/]");
        }

        AnsiConsole.Write(table);
    }
}