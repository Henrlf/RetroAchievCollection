using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.RetroAchievements.Services;

public abstract class BaseService
{
    private static readonly HttpClient _httpClient = new();
    private const string BaseUrl  = "https://retroachievements.org/API";
    
    protected string ApiKey {get;}
    protected string ApiUsername {get;}

    protected BaseService(ConfigurationModel configurationModel)
    {
        ApiKey = configurationModel.ApiKey;
        ApiUsername = configurationModel.UserName;
    }
    
    public string getUrl(string suffix)
    {
        return Path.Combine(BaseUrl, suffix);
    }
    
    protected async Task<string> GetAsync(string endpoint)
    {
        Validate();
        
        var response = await _httpClient.GetAsync(getUrl(endpoint));
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiUsername))
        {
            throw new NullReferenceException("Username or API Key is invalid!");
        }
    }
}