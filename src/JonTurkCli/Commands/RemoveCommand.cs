using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JonTurkCli.Models;
using Spectre.Console;

namespace JonTurkCli.Commands;

[Command(Description = "Removes the specified command from the saved commands.")]
public class RemoveCommand : ICommand
{
    [CommandOption("name", 'n', IsRequired = true)]
    public required string Name { get; set; }
    
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var saveFilePath = CliConsts.JonTurkSaveFilePath;
        if (!File.Exists(saveFilePath))
        {
            AnsiConsole.MarkupLine("[red]There is not any saved command![/]");
            return;
        }
        
        try
        {
            var fileContent = await File.ReadAllTextAsync(saveFilePath);
            
            NormalizeCommandOptions();

            var commandSaveLines = JsonSerializer.Deserialize<CommandSaveLinesModel>(fileContent);
            if (commandSaveLines == null || !commandSaveLines.Commands.Any() || !commandSaveLines.Commands.Any(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                AnsiConsole.MarkupLine($"[red]Could not find the specified command with the name '{Name}' in the saved commands list.[/]");
                return;
            }

            commandSaveLines.Commands.RemoveAll(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));

            await File.WriteAllTextAsync(saveFilePath, JsonSerializer.Serialize(commandSaveLines));
            
            AnsiConsole.MarkupLine($"[green]The command with the name '{Name}' is successfully deleted![/]");
        }
        catch
        {
            //ignore...
        }
    }

    private void NormalizeCommandOptions()
    {
        Name = Name.Trim('"');
    }
}