using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JonTurkCli.Models;
using Spectre.Console;

namespace JonTurkCli.Commands;

[Command(Description = "Runs the specified command.")]
public class RunCommand : ICommand
{
    [CommandOption("name", 'n', IsRequired = true, Description = "Name of the command")]
    public required string Name { get; set; }
    
    [CommandOption("path", 'p', Description = "Working directory where the command will be run.")]
    public string? WorkingDirectory { get; init; }
    
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var saveFilePath = CliConsts.JonTurkSaveFilePath;
        if (!File.Exists(saveFilePath))
        {
            AnsiConsole.MarkupLine($"[red]Could not find the specified command with the name '{Name}' in the saved commands list.[/]");
            return;
        }

        try
        {
            NormalizeCommandOptions();

            var fileContent = await File.ReadAllTextAsync(saveFilePath);

            var commandSaveLines = JsonSerializer.Deserialize<CommandSaveLinesModel>(fileContent);
            if (commandSaveLines == null || !commandSaveLines.Commands.Any() || !commandSaveLines.Commands.Any(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                AnsiConsole.MarkupLine($"[red]Could not find the specified command with the name '{Name}' in the saved commands list.[/]");
                return;
            }
            
            var command = commandSaveLines.Commands.First(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)).Command;

            AnsiConsole.MarkupLine($"Running the command: [green]'{command}'[/]");
            AnsiConsole.WriteLine();
            
            await AnsiConsole.Status()
                .StartAsync("The command is being processed...", _ =>
                {
                    CommandRunner.Run(command, out var output, WorkingDirectory);
                    AnsiConsole.WriteLine(output);
                    
                    return Task.CompletedTask;
                });
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Could not run the command![/]");
        }
    }

    private void NormalizeCommandOptions()
    {
        Name = Name.Trim('"');
    }
}