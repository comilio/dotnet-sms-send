# Comilio .NET SMS Send

.NET client library to send SMS using Comilio SMS Gateway

To use this library, you must have a valid account on https://www.comilio.it.

**Please note** SMS messages sent with this library will be deducted by your Comilio account credits.

For any questions, please contact us at tech@comilio.it

# How to send a message using C#

```csharp
var sms = new SmsMessage();
    sms.Authenticate("your_username", "your_password")
        .SetRecipients(new string[] { "+393400000000" })
        .Send("Hello World!");
```

# Installation

## NuGet (recommended)

Install it via NuGet (https://nuget.org/).

* Run `nuget install Comilio.SMS`
* See script example https://github.com/comilio/dotnet-sms-send/blob/master/examples/Comilio.Examples.SendSms/Program.cs


## Manual installation

You can simply clone the repository into your project and use the classes contained in src/ directory.

Please check the examples directory here: https://github.com/comilio/dotnet-sms-send/tree/master/examples

# More info

You can check out our website https://www.comilio.it or contact us.

# Contributing

If you wish to contribute to this project, please feel free to send us pull request. We'll be happy to check them out!
