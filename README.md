# How Insightful! Grok Your ASP.NET Core Web Apps with Azure Monitor

Thanks so much for attending my talk! I hope you found it informative and enjoyable. I've gathered information to help you get started with Application Insights and ASP.NET Core.

## Instrument your application

I've written instructions in this repository for instrumenting your app to gather:

* Server-side telemetry
* Client-side telemetry
* Debug snapshots

### Server-side telemetry

* Activate the extension (Azure) or Application Insights Status Monitor from [Web Platform Installer](https://www.microsoft.com/web/downloads/platform.aspx) (IIS)
* Install NuGet Package - [Microsoft.ApplicationInsights.AspNetCore](https://www.nuget.org/packages/Microsoft.ApplicationInsights.AspNetCore)

### Client-side telemetry 

Make the following changes to `_Layout.cshtml`.

Razor directives (top of page):

```
@using Microsoft.Extensions.Configuration
@inject IConfiguration Configuration
```

Script block (insert as the last element before `</head>`)

```
<script type="text/javascript">
	var appInsights = window.appInsights || function (a) {
		function b(a) { c[a] = function () { var b = arguments; c.queue.push(function () { c[a].apply(c, b) }) } } var c = { config: a }, d = document, e = window; setTimeout(function () { var b = d.createElement("script"); b.src = a.url || "https://az416426.vo.msecnd.net/scripts/a/ai.0.js", d.getElementsByTagName("script")[0].parentNode.appendChild(b) }); try { c.cookie = d.cookie } catch (a) { } c.queue = []; for (var f = ["Event", "Exception", "Metric", "PageView", "Trace", "Dependency"]; f.length;)b("track" + f.pop()); if (b("setAuthenticatedUserContext"), b("clearAuthenticatedUserContext"), b("startTrackEvent"), b("stopTrackEvent"), b("startTrackPage"), b("stopTrackPage"), b("flush"), !a.disableExceptionTracking) { f = "onerror", b("_" + f); var g = e[f]; e[f] = function (a, b, d, e, h) { var i = g && g(a, b, d, e, h); return !0 !== i && c["_" + f](a, b, d, e, h), i } } return c
	}({
		instrumentationKey: "@Configuration.GetSection("ApplicationInsights")["InstrumentationKey"]"
	});

	window.appInsights = appInsights, appInsights.queue && 0 === appInsights.queue.length && appInsights.trackPageView();
</script>
```

### Snapshot Debugging

Install NuGet Package - [Microsoft.ApplicationInsights.SnapshotCollector](https://www.nuget.org/packages/Microsoft.ApplicationInsights.SnapshotCollector/)

Add these using directives to `Startup.cs`:

```
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights.SnapshotCollector;
```

Add this block to `Startup.ConfigureServices`:

```
// Configure SnapshotCollector from application settings
services.Configure<SnapshotCollectorConfiguration>(Configuration.GetSection(nameof(SnapshotCollectorConfiguration)));

// Add SnapshotCollector telemetry processor.
services.AddSingleton<ITelemetryProcessorFactory>(sp => new SnapshotCollectorTelemetryProcessorFactory(sp));
```

Add this private class within the `Startup` class:

```
private class SnapshotCollectorTelemetryProcessorFactory : ITelemetryProcessorFactory
{
	private readonly IServiceProvider _serviceProvider;

	public SnapshotCollectorTelemetryProcessorFactory(IServiceProvider serviceProvider) =>
		_serviceProvider = serviceProvider;

	public ITelemetryProcessor Create(ITelemetryProcessor next)
	{
		var snapshotConfigurationOptions = _serviceProvider.GetService<IOptions<SnapshotCollectorConfiguration>>();
		return new SnapshotCollectorTelemetryProcessor(next, configuration: snapshotConfigurationOptions.Value);
	}
}
```

Add this configuration to `appsettings.json`:

```
  "SnapshotCollectorConfiguration": {
    "IsEnabledInDeveloperMode": false,
    "ThresholdForSnapshotting": 1,
    "MaximumSnapshotsRequired": 3,
    "MaximumCollectionPlanSize": 50,
    "ReconnectInterval": "00:15:00",
    "ProblemCounterResetInterval": "1.00:00:00",
    "SnapshotsPerTenMinutesLimit": 1,
    "SnapshotsPerDayLimit": 30,
    "SnapshotInLowPriorityThread": true,
    "ProvideAnonymousTelemetry": true,
    "FailedRequestLimit": 3
  }
```

## Demos

`.\Demos\MoviePortal_Instrumented\RazorPagesMovie.sln` is the fully instrumented demo. I don't intend on leaving MovieNewsService running, so you'll need to deploy the MovieNewsService API somewhere (Azure?) and change the `NewsService` class (in the Services folder in the RazorPagesMovie project) to point to your deployed service.

Be sure to add your own instrumentation key to `appsettings.json`.
