﻿using System.Text.Json;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JonTurkCli.Exceptions;
using JonTurkCli.Models;
using Spectre.Console;

namespace JonTurkCli.Commands;

[Command(Description = "Saves the related command with the name, so all of the commands can be listed and run later on.")]
public class SaveCommand : ICommand
{
    [CommandOption("name", 'n', IsRequired = true)]
    public required string Name { get; set; }

    [CommandOption("command", 'c', IsRequired = true)]
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
                                                    "You can list all saved commands with the 'jonturk-cli list' command!");
            }

            commandSaveLines.Commands.Add(new CommandSaveLineModel(Name, Command));

            await File.WriteAllTextAsync(saveFilePath, JsonSerializer.Serialize(commandSaveLines));
        }
        catch (JsonException ex)
        {
            File.Delete(saveFilePath);

            AnsiConsole.Console.WriteLine($"{saveFilePath} file was in a invalid state and therefore it has been deleted. " +
                                          $"You can run the command again, if you want to save the command.");
        }
        catch (DuplicateCommandException ex)
        {
            AnsiConsole.Console.WriteLine(ex.Message);
        }
        catch (Exception ex) when (ex is not DuplicateCommandException)
        {
            //ignore...
        }
    }

    private static void CreateFileIfNotExists(string filePath)
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

    private void NormalizeCommandOptions()
    {
        Name = Name.Trim('"');
        Command = Command.Trim('"');
    }
}