# ShellSharp

## Overview

ShellSharp is a basic shell implemented in C#. It supports a Read-Eval-Print Loop (REPL) and includes several built-in commands. The shell can execute external programs and handle missing commands gracefully.

## Features

- **Print a prompt**: Displays a prompt to the user.
- **Handle missing commands**: Provides feedback when a command is not found.
- **REPL**: Continuously reads, evaluates, and prints user input.
- **Built-in commands**:
  - `exit`: Exits the shell.
  - `echo`: Prints the provided arguments to the console.
  - `type`: Displays information about built-in commands and executable files.

## Getting Started

### Prerequisites

- .NET SDK installed on your machine.

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/ShellSharp.git
    cd ShellSharp
    ```

2. Build the project:
    ```sh
    dotnet build
    ```

3. Run the shell:
    ```sh
    dotnet run
    ```

## Usage

### Commands

- **Prompt**: The shell displays a prompt (`$`) where you can enter commands.
- **Missing Commands**: If a command is not found, the shell will display an error message.
- **REPL**: The shell will continuously read and execute commands until you exit.

### Built-in Commands

- **exit**: Exits the shell.
    ```sh
    $ exit
    ```

- **echo**: Prints the provided arguments to the console.
    ```sh
    $ echo Hello, World!
    ```

- **type**: Displays information about built-in commands and executable files.
    - To display the type of an executable file:
        ```sh
        $ type <executable>
        ```

### Running Programs

You can run any executable program available in your system's PATH.
```sh
$ notepad
```

## Example

```sh
$ echo Hello, World!
Hello, World!
$ type type
type is a shell builtin
$ type notepad
notepad is C:\Program Files\notepad.EXE
$ exit
```

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the 2-Clause BSD License.

