using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JonTurkCli.Models;
using Spectre.Console;

namespace JonTurkCli.Commands;

[Command(Description = "Run the specified command.")]
public class RunCommand : ICommand
{
    [CommandOption("name", 'n', IsRequired = true)]
    public required string Name { get; set; }
    
    [CommandOption("path", 'p', Description = "Working directory were the command will be run.")]
    public string? WorkingDirectory { get; set; }
    
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
            Name = Name.Trim('"');

            var fileContent = await File.ReadAllTextAsync(saveFilePath);

            var commandSaveLines = JsonSerializer.Deserialize<CommandSaveLinesModel>(fileContent);
            if (commandSaveLines == null || !commandSaveLines.Commands.Any() || !commandSaveLines.Commands.Any(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                AnsiConsole.MarkupLine($"[red]Could not find the specified command with the name '{Name}' in the saved commands list.[/]");
                return;
            }
            
            var command = commandSaveLines.Commands.First(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)).Command;

            await AnsiConsole.Status()
                .StartAsync($"Running the command: [green]'{command}'[/]", context =>
                {
                    CommandRunner.Run(command, out var output, WorkingDirectory);
            
                    AnsiConsole.WriteLine(output);
                    
                    return Task.CompletedTask;
                });
        }
        catch
        {
            AnsiConsole.MarkupLine("Could not run the command!");
        }
    }
}