using System.Text;
using System.Text.Json;
using ERP.Shared.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Shared.Responses;

public class LocalizationResponseMiddleware
{
    private readonly RequestDelegate _next;
    public LocalizationResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IStringLocalizer<Resource> localizer)
    {
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (!string.IsNullOrWhiteSpace(responseText) && context.Response.ContentType != null && context.Response.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var json = JsonSerializer.Deserialize<JsonElement>(responseText);
                if (json.ValueKind == JsonValueKind.Object)
                {
                    using var doc = JsonDocument.Parse(responseText);
                    var root = doc.RootElement;
                    // Attempt to detect ApiResponse shape
                    if (root.TryGetProperty("isSuccess", out _))
                    {
                        var localized = LocalizeApiResponse(root, localizer);
                        responseText = JsonSerializer.Serialize(localized);
                    }
                }
            }
            catch
            {
                // ignore and pass-through on serialization issues
            }
        }

        var modifiedBytes = Encoding.UTF8.GetBytes(responseText);
        context.Response.Body = originalBodyStream;
        await context.Response.Body.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
    }

    private static Dictionary<string, object?> LocalizeApiResponse(JsonElement root, IStringLocalizer<Resource> localizer)
    {
        var dict = new Dictionary<string, object?>();
        foreach (var prop in root.EnumerateObject())
        {
            dict[prop.Name] = prop.Value.ValueKind switch
            {
                JsonValueKind.String => prop.Value.GetString(),
                JsonValueKind.Number => prop.Value.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => JsonSerializer.Deserialize<object>(prop.Value.GetRawText())
            };
        }

        // Localize Success
        if (root.TryGetProperty("success", out var successElement) && successElement.ValueKind == JsonValueKind.Object)
        {
            var key = successElement.TryGetProperty("messageKey", out var k) ? k.GetString() : null;
            var args = ExtractArgs(successElement);
            if (!string.IsNullOrWhiteSpace(key))
            {
                dict["successMessage"] = string.Format(localizer[key!].Value, args);
            }
            dict.Remove("success");
        }

        // Localize Errors (array)
        if (root.TryGetProperty("errors", out var errorsElement) && errorsElement.ValueKind == JsonValueKind.Array)
        {
            var messages = new List<string>();
            foreach (var err in errorsElement.EnumerateArray())
            {
                if (err.ValueKind != JsonValueKind.Object) continue;
                var key = err.TryGetProperty("messageKey", out var k) ? k.GetString() : null;
                var args = ExtractArgs(err);
                if (!string.IsNullOrWhiteSpace(key))
                {
                    messages.Add(string.Format(localizer[key!].Value, args));
                }
            }
            if (messages.Count > 0)
                dict["errorMessages"] = messages;
            dict.Remove("errors");
        }

        return dict;
    }

    private static object[] ExtractArgs(JsonElement element)
    {
        if (element.TryGetProperty("args", out var argsElement) && argsElement.ValueKind == JsonValueKind.Array)
        {
            var args = new List<object>();
            foreach (var a in argsElement.EnumerateArray())
            {
                switch (a.ValueKind)
                {
                    case JsonValueKind.String:
                        args.Add(a.GetString()!);
                        break;
                    case JsonValueKind.Number:
                        if (a.TryGetInt64(out var l)) args.Add(l);
                        else if (a.TryGetDouble(out var d)) args.Add(d);
                        break;
                    case JsonValueKind.True:
                        args.Add(true);
                        break;
                    case JsonValueKind.False:
                        args.Add(false);
                        break;
                    case JsonValueKind.Null:
                        args.Add(null!);
                        break;
                    default:
                        args.Add(JsonSerializer.Deserialize<object>(a.GetRawText())!);
                        break;
                }
            }
            return args.ToArray();
        }
        return Array.Empty<object>();
    }
}

