﻿
public class Appsettings
{
    public string ApiName { get; set; }
    public Connectionstring ConnectionString { get; set; }
    public Configurations Configurations { get; set; }
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
}

public class Connectionstring
{
    public string AppProgrammingInt { get; set; }
}

public class Configurations
{
    public string AgentoConnectCuentaMovimiento { get; set; }
    public string AgentoConnectPersonaCuenta { get; set; }
    public Security Security { get; set; }
}

public class Security
{
    public Secretkeys SecretKeys { get; set; }
    public Ipratelimiting IpRateLimiting { get; set; }
    public string CorsAlows { get; set; }
}

public class Secretkeys
{
    public string GateWaySecretKey { get; set; }
}

public class Ipratelimiting
{
    public bool EnableEndpointRateLimiting { get; set; }
    public bool StackBlockedRequests { get; set; }
    public string RealIpHeader { get; set; }
    public string ClientIdHeader { get; set; }
    public int HttpStatusCode { get; set; }
    public Generalrule[] GeneralRules { get; set; }
}

public class Generalrule
{
    public string Endpoint { get; set; }
    public string Period { get; set; }
    public int Limit { get; set; }
}

public class Logging
{
    public Loglevel LogLevel { get; set; }
}

public class Loglevel
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class Applicationinsights
{
    public string ConnectionString { get; set; }
}

public class Azuread
{
    public string Instance { get; set; }
    public string Domain { get; set; }
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string CallbackPath { get; set; }
    public string Scopes { get; set; }
}
