# WFAppLogger
WinForm Application Logger example

C# WinForm Application example that uses [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1) for logging. 
This demonstrates a uniform logging methodology when using Entity Framework Core or other .NET Core libraries in a WinForm Application.
This example uses both [ConsoleLogger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.consoleloggerextensions.addconsole?view=dotnet-plat-ext-3.1) and [FileLogger](https://github.com/adams85/filelogger), and uses [Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1) for creating Logger and Form classes.

Most of the online example code available relied on settings in a separate `appsettings.json` file or only provided file logging through a secondary framework extension such as [NLog](https://github.com/NLog/NLog.Extensions.Logging) or [SeriLog](https://github.com/serilog/serilog-extensions-logging-file).
In this example, the logging settings utilize standard Microsoft WinForm application settings as XML in the `App.config` file (below) and the lightweight [FileLogger](https://github.com/adams85/filelogger).
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <configSections>
        <sectionGroup name="userSettings" type="System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="WFAppLogger.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" allowExeDefinition="MachineToLocalUser" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <!-- some sections omitted for brevity ... !-->
  <userSettings>
    <WFAppLogger.Properties.Settings>
      <setting name="DefaultLogMessage" serializeAs="String">
        <value>Sample log message</value>
      </setting>
      <setting name="LoggingConfig" serializeAs="Xml">
        <value>
          <Root>
            <Logging>
              <LogLevel>
                <Default>Debug</Default>
                <WFAppLogger>Trace</WFAppLogger>
                <Microsoft>Warning</Microsoft>
                <System>Warning</System>
              </LogLevel>
              <Console>
                <IncludeScopes>true</IncludeScopes>
              </Console>
              <File>
                <IncludeScopes>true</IncludeScopes>
                <!-- Log files will be written to %TEMP%[\%BasePath%]\<appname>-<startdate>-<counter>.log -->
                <!--<BasePath>Logs</BasePath>-->
                <Files>
                  <File>
                    <Path>&lt;appname&gt;-&lt;startdate:yyyyMMdd-HHmmss&gt;-&lt;counter:000&gt;.log</Path>
                    <MaxFileSize>100000</MaxFileSize>
                  </File>
                </Files>
              </File>
            </Logging>
          </Root>
        </value>
      </setting>
    </WFAppLogger.Properties.Settings>
  </userSettings>
</configuration>
```

The example also provides custom log file Path settings:
|**Path Template Parameter**|**Notes**|
|---|---|
|**appname**|Application Name (e.g. **WFAppLogger**)|
|**startdate**|The [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime.now) that file logging was started. It can include a standard .NET format string to be passed to [DateTime.ToString](https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tostring#System_DateTime_ToString_System_String_) (e.g. "&lt;appname&gt;-&lt;startdate:yyyyMMdd-HHmmss&gt;-&lt;counter:000&gt;.log" will create a log file such as "WFAppLogger-20200410-105108-001.log")|
