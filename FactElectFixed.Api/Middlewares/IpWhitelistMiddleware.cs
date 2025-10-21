using System.Net;

namespace FactElectFixed.Api.Middlewares;

public class IpWhitelistMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
        {
            // Get the remote IP address of the client
            IPAddress? remoteIp = context.Connection.RemoteIpAddress;

            // Fetch the allowed IPs from configuration
            string[]? allowedIPs = configuration.GetSection("AllowedIPs").Get<string[]>();
            
            // Check if the remote IP is in the allowed IP list
            if (!IPAddress.IsLoopback(remoteIp!) && !allowedIPs!.Contains(remoteIp!.ToString()))
            {
                // If the IP is not allowed, return a forbidden response
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync($"Access denied from IP: {remoteIp}");
                return;
            }

            // Proceed to the next middleware in the pipeline if the IP is allowed
            await next(context);
        }
}
