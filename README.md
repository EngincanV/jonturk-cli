# JonTurk CLI

<a href="https://www.nuget.org/packages/JonTurkCli"><img src="https://img.shields.io/nuget/v/JonTurkCli?logo=nuget" alt="JonTurk CLI on Nuget" /></a>

A command line tool that allows you to save, list and run the frequently used CLI commands. It's a dotnet tool and can be accessed via `jonturk` **CLI** command, after you followed the installation instructions below.

> Love the JonTurk CLI? Then, please **give a star**(⭐)! If you find any bug, or want to have additional features, don't hesitate to create an issue. Thanks in advance!

## Installation

You can install the CLI tool with by running the following command:

```bash
dotnet tool install -g JonTurk
```

## Usage

You can execute the `jonturk --help` command in the terminal to list all commands with their possible options:

```bash
jonturk --help
```

Here are list of options those currently supported:

* `ListCommand` (Lists the saved commands)
* `RemoveCommand` (Removes the specified command from the saved commands)
* `RunCommand` (Runs the specified command)
* `SaveCommand` (Saves the related command with the name)
