namespace AsyncDemo;

public sealed class GitHubService(HttpClient httpClient)
{
    public async Task<string> DownloadWebPageAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException($"Ошибка при загрузке {url}: запрос был отменён или истек таймаут.", ex);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            throw new HttpRequestException($"Ошибка при загрузке {url}: доступ запрещён (rate limit).", ex);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Ошибка при загрузке {url}: {ex.Message}", ex);
        }
    }
}