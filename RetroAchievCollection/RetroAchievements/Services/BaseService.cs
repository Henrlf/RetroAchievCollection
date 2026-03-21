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

        try
        {
            var url = getUrl(endpoint);
            RetroAchievCollection.Services.BaseService.SaveLog("request.log", url);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            RetroAchievCollection.Services.BaseService.SaveLog("request.log", content);
            
            return content;
        }
        catch (Exception e)
        {
            RetroAchievCollection.Services.BaseService.SaveError(e.ToString());
            throw;
        }
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiUsername))
        {
            throw new NullReferenceException("Username or API Key is invalid!");
        }
    }
}