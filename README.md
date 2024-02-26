# JonTurk CLI

<a href="https://www.nuget.org/packages/JonTurkCli"><img src="https://img.shields.io/nuget/v/JonTurkCli?logo=nuget" alt="JonTurk CLI on Nuget" /></a>

A command line tool that allows you to save, list, and run the frequently used CLI commands. It's a dotnet tool and can be accessed via `jonturk` **CLI** command after you follow the installation instructions below.

> Love the JonTurk CLI? Then, please **give a star**(⭐)! If you find any bug or want to have additional features, don't hesitate to create an issue. Thanks in advance!

## Installation

You can install the CLI tool by running the following command:

```bash
dotnet tool install --global JonTurkCli
```

## Usage

You can execute the `jonturk --help` command in the terminal to list all commands with their possible options:

```bash
jonturk --help
```

## Commands

The following commands are currently available:

### Save Command

Saves the related command with the name:

```bash
jonturk save -n "cache-clear" -c "dotnet nuget locals all --clear" 
```

* **-n | --name** (Name of the command)
* **-c | --command** (Command (with arguments))

### List Command

Lists the saved commands in a table view:

```bash
jonturk list
```

### Run Command

Runs the specified command:

```bash
jonturk run -n "cache-clear"
```

* **-n | --name** (Name of the command)
* **-p | --path** (The working directory where the command will be run.)

### Remove Command

Removes the specified command from the saved commands:

```bash
jonturk remove -n "cache-clear"
```

* **-n | --name** (Name of the command)
