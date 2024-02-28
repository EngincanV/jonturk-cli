﻿using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JonTurkCli.Exceptions;
using JonTurkCli.Models;
using Spectre.Console;

namespace JonTurkCli.Commands;

[Command("save", Description = "Saves the related command with the name.")]
public class SaveCommand : ICommand
{
    [CommandOption("name", 'n', IsRequired = true, Description = "Name of the command")]
    public required string Name { get; set; }

    [CommandOption("command", 'c', IsRequired = true, Description = "Command (with arguments)")]
    public required string Command { get; set; }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var saveFilePath = CliConsts.JonTurkSaveFilePath;

        CreateFileIfNotExists(saveFilePath);

        try
        {
            var fileContent = await File.ReadAllTextAsync(saveFilePath);

            var commandSaveLines = JsonSerializer.Deserialize<CommandSaveLinesModel>(fileContent) 
                                   ?? new CommandSaveLinesModel();

            NormalizeCommandOptions();
            
            if (commandSaveLines.Commands.Any(command => command.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateCommandException($"There is already a saved command with the '{Name}' keyword, " +
                                                    "please change the command name and then retry to save the command." +
                                                    "You can list all saved commands with the 'jonturk list' command!");
            }

            commandSaveLines.Commands.Add(new CommandSaveLineModel(Name, Command));

            await File.WriteAllTextAsync(saveFilePath, JsonSerializer.Serialize(commandSaveLines));
            
            await CreatePs1FileIfNotExistsAsync(Command);
            
            AnsiConsole.MarkupLine($"[green]The command with the name '{Name}' is successfully saved![/]");
        }
        catch (JsonException ex)
        {
            File.Delete(saveFilePath);

            AnsiConsole.MarkupLine($"[red]'{saveFilePath}' file was in an invalid state and therefore it has been deleted. " +
                                           $"You can run the command again if you want to save the command.[/]");
        }
        catch (DuplicateCommandException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        catch (Exception ex) when (ex is not DuplicateCommandException)
        {
            //ignore...
        }
    }

    private static void CreateFileIfNotExists(string filePath, bool isBlank = false)
    {
        if (File.Exists(filePath))
        {
            return;
        }

        using (var writer = new StreamWriter(filePath))
        {
            writer.Write("{}");
        }
    }
    
    private static void AddEnvironmentPathIfNotExists(string directoryPath)
    {
        if(!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var environmentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? string.Empty;
        var paths = environmentPath.Split(";");
        
        if (paths.Contains(directoryPath))
        {
            return;
        }

        if (!environmentPath.EndsWith(';'))
        {
            environmentPath += ";";
        }
        
        environmentPath += $"{directoryPath}";
        
        Environment.SetEnvironmentVariable("PATH", environmentPath, EnvironmentVariableTarget.User);
    }
    
    private async Task CreatePs1FileIfNotExistsAsync(string content)
    {
        try
        {
            AddEnvironmentPathIfNotExists(CliConsts.JonTurkScriptsFolderPath);
            await using var writer = new StreamWriter(Path.Combine(CliConsts.JonTurkScriptsFolderPath, $"{Name}.ps1"));
            await writer.WriteAsync(content);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Could not create the ps1 file[/]");
        }
    }
    
    private void NormalizeCommandOptions()
    {
        Name = Name.Trim('"');
        Command = Command.Trim('"');
    }
}