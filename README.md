# Restless Toolkit

Restless Toolkit is a C# solution that consists of custom controls and utility classes that may be used in a WPF application.
It is an offshoot of the [Restless Tools](https://github.com/victor-david/restless-tools) project, which
is no longer under active development. Going forward, **Restless Toolkit** will provide some of the same
controls, improved ones, while removing obsolete functionality, and combining other functionality into a single package.

There are two .dll projects (available as nuget packages) and one .exe project:

## Restless.Toolkit

[![Nuget](https://img.shields.io/nuget/v/Restless.Toolkit.svg?style=flat-square)](https://www.nuget.org/packages/Restless.Toolkit/) 

This .dll project contains custom controls, MVVM base classes, and other helper classes used
for presentation. Its target frameworks are **netcoreapp3.1** and **net461**. Custom controls in this package include:
  * AppWindow - An application window with custom title bar, borders and icons, and the ability to have the the application's main menu inside the title bar.
  * TabControl - A custom tab control that supports re-ordering tabs via drag and drop, ability to set tab height and a consistent width,
and the ability to maintain the status of tabs during tab switch.
  * DataGrid - A custom data grid control that enables more complex sorting and other features.
  * More...

## Restless.Toolkit.Core 
[![Nuget](https://img.shields.io/nuget/v/Restless.Toolkit.Core.svg?style=flat-square)](https://www.nuget.org/packages/Restless.Toolkit.Core/)

This .dll project contains classes that aren't part of presentation, such as base classes
for Sqlite databases. Its target frameworks are **netstandard2.0** and **netstandard2.1**.

## Restless.App.Toolkit

This is a sample application to demo the custom tab control and others. It targets **netcoreapp3.1**.
