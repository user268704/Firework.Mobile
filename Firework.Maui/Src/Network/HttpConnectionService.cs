namespace Firework.Maui.Network;

/*public class HttpConnectionService : IConnectionService, IDisposable
{
    private readonly IInstructionService _instructionService;
    private HttpClient _httpClient;
    private static ConnectionInfo _connectionInfo;

    private HttpClient HttpClient
    {
        get
        {
            var baseAddress = Preferences.Get(PreferencesDefault.HOST, "http://192.168.50.32:5062");
            var serverTimeout  = Preferences.Get(PreferencesDefault.DEFAULT_TIMEOUT, 10);

            baseAddress = CreateUrl(baseAddress);

            if (_httpClient == null)
            {
                _httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(serverTimeout),
                    BaseAddress = new(baseAddress)
                };

                return _httpClient;
            }


            if (_httpClient.BaseAddress != new Uri(baseAddress))
            {
                _httpClient.BaseAddress = new Uri(baseAddress);
            }


            return _httpClient;
        }
        set => _httpClient = value;
    }

    public HttpConnectionService(IInstructionService instructionService)
    {
        _instructionService = instructionService;
    }
    
    public bool Ping()
    {
        try
        {
            var pingResult = HttpClient.GetAsync("/ping").Result;

            return pingResult.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public async Task<HttpResult> SendCommandAsync(InstructionInfo command)
    {
        try
        {
            var httpContent = new StringContent(_instructionService.ToString(command), Encoding.UTF8);

            HttpResponseMessage getRequestResult;

            try
            {
                getRequestResult = await HttpClient.PostAsync("/command", httpContent);
            }
            catch (HttpRequestException e)
            {
                return new HttpResult()
                {
                    Data = "Ошибка подключения, проверьте хост",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            catch (WebException e)
            {
                return new HttpResult()
                {
                    Data = "Ошибка подключения, проверьте хост",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }


            var data = await getRequestResult.Content.ReadAsStringAsync();

            SetConnectionInfo(new ConnectionInfo
            {
                DateConnected = DateTime.Now,
                ConnectedDeviceInfo = new ConnectedDeviceInfo
                {
                    IsConnected = getRequestResult.IsSuccessStatusCode,
                }
            });

            var result = JsonSerializer.Deserialize<HttpResult>(data);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw e;
        }
    }

    public HttpResult SendCommand(InstructionInfo command)
    {
        var httpContent = new StringContent(_instructionService.ToString(command), Encoding.UTF8);
        HttpResponseMessage getRequestResult;

        try
        {
            getRequestResult = HttpClient.PostAsync("/command", httpContent).Result;
        }
        catch (AggregateException e)
        {
            return new HttpResult
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = "Ошибка подключения, проверьте хост"
            };
        }
        catch (WebException e)
        {
            return new HttpResult()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = "Ошибка подключения, проверьте хост"
            };
        }
        catch (HttpRequestException e)
        {
            return new HttpResult()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = "Ошибка подключения, проверьте хост"
            };
        }


        var data = getRequestResult.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<HttpResult>(data);
        
        return result;
    }

    public bool IsCanConnected(string host)
    {
        if (string.IsNullOrEmpty(host))
            return false;

        host = CreateUrl(host);

        var serverTimeout  = Preferences.Get(PreferencesDefault.DEFAULT_TIMEOUT, 10);

        using HttpClient client = new()
        {
            BaseAddress = new Uri(host),
            Timeout = TimeSpan.FromSeconds(serverTimeout)
        };
        
        try
        {
            var result = client.GetAsync("/ping").Result;
            var ping = result.Content.ReadAsStringAsync().Result;

            var data = JsonSerializer.Deserialize<HttpResult>(ping);

            return data.StatusCode == HttpStatusCode.OK && data.Data.Equals("pong");
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public ConnectionInfo GetCurrentConnectionInfo()
    {
        return _connectionInfo;
    }

    public void RestartConnection()
    {
        throw new NotImplementedException();
    }

    public void SetConnectionInfo(ConnectionInfo connectionInfo)
    {
        _connectionInfo = connectionInfo;
    }

    private string CreateUrl(string host)
    {
        if (!host.StartsWith("http://") &&
            !host.StartsWith("https://"))
        {
            host = "http://" + host;
        }

        return host;
    }

    public ConnectionInfo Empty =>
        new()
        {
            DateConnected = DateTime.MinValue,
            ConnectedDeviceInfo = new ConnectedDeviceInfo
            {
                HostIp = IPEndPoint.Parse("0.0.0.0"),
                HostName = string.Empty,
                IsConnected = false,
                IsLocalConnected = null,
            }
        };

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}*/