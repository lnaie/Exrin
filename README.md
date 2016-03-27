
![Exrin](http://exrin.azurewebsites.net/wp-content/uploads/2016/03/exrin_128.png)

# Introduction

Exrin is a Xamarin Forms Framework designed to enable teams to develop consistent, reliable and highly performant mobile apps. Exrin lets you put more focus on how the app will look and how the user will interact, while Exrin takes care of handling the users intent.

# Backstory

I always found it hard to isolate and test ViewModels and hard to keep developers consistently accounting for all scenarios on each user action. Think of things such as which thread to run this on, when should I notify the user that the app is currently doing something, handle exceptions, handle timeouts, how should I log an event and how should I track application insights in a reliable fashion.

This is why Exrin is created and there are only 3 rules needed to be followed by each developer to ensure it is consistent across the app.

1. Always use IViewModelExecute for every Command and/or user action
2. Always use IModelExecute for every method on the Model.
3. Unit Test every IOperation created.

The framework enforces the rest.

# Getting Started

**Install the Nuget Package**

[Exrin Nuget Package](https://www.nuget.org/packages/Exrin/)


**Read the Docs**

[Getting Started](http://docs.exrin.net/)

**Sample App**

[Tesla App](https://github.com/adamped/Tesla-Mobile-App) - A basic example showing the complete setup and usage of Exrin.

# Frequently Asked Questions

1. Is this another MVVM Framework?
Yes and no. You can use this with any other MVVM Framework, or use what is available in Exrin.

2. What IoC does it use?
None, you inject your own IoC and DI Framework when configuring your app. After using a few on different project, I prefer AutoFac.

3. Does this have any dependencies?
No. That was a critical design point. Exrin does not depend on anything. Having a package that locks you into another package version has become increasing frustrating and Exrin will not be a part of that.

4. Why this framework?
It's not going to be for everyone. It is opinionated and requires some effort in project setup. The benefits however include consistency and easy testability for your mobile app. The benefits are greatly enhanced if 2 or more developers are on the project.

# Support

Please visit [Contact](http://xamarinhelp.com/contact/)

# License

[MIT License](https://github.com/adamped/exrin/blob/master/LICENSE) © Adam Pedley
