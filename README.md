
![Exrin](http://exrin.azurewebsites.net/wp-content/uploads/2016/03/exrin_128.png)

[![Build Status](https://travis-ci.org/exrin/Exrin.svg?branch=master)](https://travis-ci.org/exrin/Exrin)
[![Nuget](https://img.shields.io/nuget/v/Exrin.svg?style=flat-square)](https://www.nuget.org/packages/Exrin) 

# Introduction

Exrin is an extended Xamarin Forms MVVM Framework designed to enable teams to develop consistent, reliable and highly performant mobile apps. Exrin lets you put more focus on how the app will look and function, while Exrin takes care of handling the rest.

# Benefits

1. Easily isolate and unit test Commands in ViewModels
2. Advanced NavigationService that takes care of all the plumbing.
3. Enforces consistency, which is of more benefit when multiple developers are working on the project.
4. Handles complex threading scenarios.
5. Flexible. Allows you to choose your own Dependency Injection Framework.

# Getting Started

**Install the Nuget Package**

[Exrin Nuget Package](https://www.nuget.org/packages/Exrin/)

**Read the Docs**

[Getting Started](http://docs.exrin.net/)

**Sample App**
[Quick Sample](https://github.com/exrin/ExrinSample) - A very basic quick start sample.
[Tesla App](https://github.com/adamped/Tesla-Mobile-App) - Shows more advanced usage of Exrin with TabbedPages and MasterDetailPage.

# Frequently Asked Questions

1. Is this another MVVM Framework?
Yes and no. It contains all the MVVM helpers you would need however you can use another MVVM framework with Exrin if you choose.

2. What IoC does it use?
None, you inject your own IoC and DI Framework when configuring your app. After using a few on different projects, I prefer AutoFac.

3. Does Exrin have any dependencies?
No. That was a critical design point. Exrin does not depend on anything. Having a package that locks you into another package version has become increasing frustrating and Exrin will not be a part of that.

4. Why this framework?
It's not going to be for everyone. It is opinionated and requires a bit of effort in project setup. The benefits however include consistency and easy testability for your mobile app. The benefits are greatly enhanced if 2 or more developers are on the project.

# Support

Please visit [Contact](http://xamarinhelp.com/contact/)

# License

[MIT License](https://github.com/adamped/exrin/blob/master/LICENSE) © Adam Pedley
