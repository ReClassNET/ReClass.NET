# ReClass.NET
This is a port of ReClass to the .NET platform with additional features.

![](https://abload.de/img/main4hsbj.jpg)

## Features
- Support for x86 / x64
- File import for ReClass 2007-2016 and ReClass QT
- Memory Nodes
 - Hex 8 / 16 / 32 / 64
 - Int 8 / 16 / 32 / 64
 - UInt 8 / 16 / 32 / 64
 - Bits ![](https://abload.de/img/bitsnhlql.jpg)
 - Float / Double
 - Vector 2 / 3 / 4
 - Matrix 3x3 / 3x4 / 4x4
 - UTF8/16/32 Text and pointer to text
 - Class Arrays and array of pointers to classes
 - VTable
 - Function Pointer
- Display types from Debug Symbols (*.pdb)
- Display Runtime Type Informations (RTTI)
- Control the remote process: start / stop / kill
- Process Selection Dialog with filtering
- Memory Viewer
- Code Generator (C++ / C#)
- Plugin Support
 - Plugins can be written in different languages (example: C++, C++/CLI, C#)
 - Plugins can provide custom methods to access an other process (example: use a driver)
 - Plugins can interact with the ReClass.NET windows
 - Plugins can provide node infos which will be displayed (example: class informations for Frostbite games)
 - Plugins can implement custom nodes with load/save and code generation support

## Plugins
- [Sample Plugins](https://github.com/KN4CK3R/ReClass.NET-SamplePlugin)
- [Frostbite Plugin](https://github.com/KN4CK3R/ReClass.NET-FrostbitePlugin)

To install a plugin just copy it in the "Plugins" folder.
If you want to develop your own plugin just learn from the code of the [Sample Plugins](https://github.com/KN4CK3R/ReClass.NET-SamplePlugin) and [Frostbite Plugin](https://github.com/KN4CK3R/ReClass.NET-FrostbitePlugin) repositories. If you have developed a nice plugin, leave me a message and I will add it to the list above.

## Installation
Just download the [latest version](https://github.com/KN4CK3R/ReClass.NET/releases) and start the x86 / x64 version.

## Compiling
If you want to compile ReClass.NET just fork the repository and open the ReClass.NET.sln file.

## Screenshots
Process Selection
![](https://abload.de/img/processgya2k.jpg)

Memory Viewer
![](https://abload.de/img/memoryviewerb4y1s.jpg)

Code Generator
![](https://abload.de/img/codegeneratorqdat2.jpg)
![](https://abload.de/img/codegenerator24qzce.jpg)

Plugins
![](https://abload.de/img/plugin1mda4r.jpg)
![](https://abload.de/img/plugin25dxk1.jpg)

Settings
![](https://abload.de/img/settings8sz4b.jpg)