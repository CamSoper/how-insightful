# How to instrument your ASP.NET Core application for Application Insights

## Server-side instrumentation

1. Activate the extension or monitor
2. Install NuGet Package  -  .UseApplicationInsights()?

## Client-side instrumentation - 

Make the following changes to _Layout.cshtml.

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

## Snapshot Debugging

Add these using directives to Startup.cs:

```
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights.SnapshotCollector;
```

Add this block to Startup.ConfigureServices:

```
// Configure SnapshotCollector from application settings
services.Configure<SnapshotCollectorConfiguration>(Configuration.GetSection(nameof(SnapshotCollectorConfiguration)));

// Add SnapshotCollector telemetry processor.
services.AddSingleton<ITelemetryProcessorFactory>(sp => new SnapshotCollectorTelemetryProcessorFactory(sp));
```

Add this class to Startup class:

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

Add this configuration to appsettings.json:

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