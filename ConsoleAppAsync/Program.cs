using System.Diagnostics;
using AsyncDemo;

namespace ConsoleAppAsync;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("===== Часть 2: Асинхронное программирование =====");

        var httpClient = new HttpClient
        {
            DefaultRequestHeaders = { { "User-Agent", "CSharp-Demo-App" } },
        };
        var service = new GitHubService(httpClient);

        await Task1_UnderstandingTask(service);
        await Task2_UsingAwait(service);
        await Task3_ParallelExecution(service);
        await Task4_Cancellation(service);
        await Task5_TaskRace(service);

        Console.WriteLine("\n===== Завершено =====");
    }

    // Задача 1: Понимаем что такое Task
    private static Task Task1_UnderstandingTask(GitHubService service)
    {
        Console.WriteLine("\n--- Задача 1: Понимаем что такое Task ---");

        const string url = "https://api.github.com/users/octocat";

        // Вызываем метод БЕЗ await - получаем Task, не строку
        var task = service.DownloadWebPageAsync(url);

        Console.WriteLine($"Тип результата: {task.GetType().Name}");
        Console.WriteLine($"Status: {task.Status}");
        Console.WriteLine($"IsCompleted: {task.IsCompleted}");

        // ВАЖНО: task.Result блокирует поток - не делайте так в реальном коде!
        // Мы показываем это только для демонстрации разницы
        Console.WriteLine("\nВызываем task.Result (программа зависнет до завершения)...");
        var content = task.Result;
        Console.WriteLine($"Получена строка длиной: {content.Length} символов");

        Console.WriteLine("\nВывод: Task - это 'обещание' будущего результата!");

        return Task.CompletedTask;
    }

    // Задача 2: Используем await
    private static async Task Task2_UsingAwait(GitHubService service)
    {
        Console.WriteLine("\n--- Задача 2: Используем await ---");

        const string url = "https://api.github.com/users/octocat";

        var stopwatch = Stopwatch.StartNew();

        try
        {
            // await "распаковывает" Task и получает реальный результат
            var content = await service.DownloadWebPageAsync(url);

            stopwatch.Stop();

            Console.WriteLine($"Загружено {content.Length} символов");
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine("\nВывод: await получает реальный результат, программа не блокируется!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // Задача 3: Параллельное выполнение
    private static async Task Task3_ParallelExecution(GitHubService service)
    {
        Console.WriteLine("\n--- Задача 3: Параллельное выполнение ---");

        var urls = new[]
        {
            "https://api.github.com/users/torvalds",
            "https://api.github.com/users/gaearon",
            "https://api.github.com/users/tj",
            "https://api.github.com/users/sindresorhus",
        };

        // Вариант 1: Последовательно (один за другим)
        Console.WriteLine("\nВариант 1 - Последовательная загрузка:");
        var sequentialStopwatch = Stopwatch.StartNew();

        try
        {
            foreach (var url in urls)
            {
                await service.DownloadWebPageAsync(url);
            }

            sequentialStopwatch.Stop();
            Console.WriteLine($"Последовательное выполнение: {sequentialStopwatch.ElapsedMilliseconds} мс");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // Вариант 2: Параллельно (все одновременно)
        Console.WriteLine("\nВариант 2 - Параллельная загрузка:");
        var parallelStopwatch = Stopwatch.StartNew();

        try
        {
            // Создаём Task'и для всех URL
            var tasks = urls.Select(url => service.DownloadWebPageAsync(url));

            // Task.WhenAll ждёт завершения ВСЕХ задач
            await Task.WhenAll(tasks);

            parallelStopwatch.Stop();
            Console.WriteLine($"Параллельное выполнение: {parallelStopwatch.ElapsedMilliseconds} мс");

            Console.WriteLine($"\nУскорение: ~{sequentialStopwatch.ElapsedMilliseconds / Math.Max(1, parallelStopwatch.ElapsedMilliseconds)}x");
            Console.WriteLine("Вывод: Параллельное выполнение намного быстрее!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // Задача 4: Отмена операций
    private static async Task Task4_Cancellation(GitHubService service)
    {
        Console.WriteLine("\n--- Задача 4: Отмена операций ---");

        var users = new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
        var urls = users.Select(u => $"https://api.github.com/users/{u}").ToArray();

        const int cancellationDelayMs = 1000;

        Console.WriteLine($"Начинаем загрузку {urls.Length} пользователей...");
        Console.WriteLine($"Отмена через {cancellationDelayMs} мс");

        // Создаём CancellationTokenSource с таймаутом
        using var cts = new CancellationTokenSource(cancellationDelayMs);

        try
        {
            foreach (var url in urls)
            {
                // Проверяем, не была ли запрошена отмена
                cts.Token.ThrowIfCancellationRequested();

                try
                {
                    await service.DownloadWebPageAsync(url, cts.Token);
                    Console.WriteLine($"✓ Загружено: {url}");
                }
                catch (HttpRequestException ex) when (
                    ex.Message.Contains("403", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("rate limit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"⚠ Rate limit для {url}, пропускаем...");
                }

                await Task.Yield();
            }

            Console.WriteLine("✓ Все загрузки завершены!");
        }
        catch (OperationCanceledException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n⚠ Операция была отменена!");
            Console.ResetColor();
            Console.WriteLine("Вывод: CancellationToken позволяет остановить длительные операции!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // Задача 5: Гонка задач (Task.WhenAny)
    private static async Task Task5_TaskRace(GitHubService service)
    {
        Console.WriteLine("\n--- Задача 5: Гонка задач (Task.WhenAny) ---");

        var apis = new Dictionary<string, string>
        {
            { "GitHub", "https://api.github.com/users/octocat" },
            { "REST Countries", "https://restcountries.com/v3.1/name/usa" },
            { "World Time", "https://worldtimeapi.org/api/timezone/America/New_York" },
        };

        Console.WriteLine("Запускаем гонку задач из 3 разных API...");
        Console.WriteLine("Побеждает тот, кто ответит первым!\n");

        using var cts = new CancellationTokenSource();

        try
        {
            // Создаём задачи для всех API
            var downloadTasks = apis.Select(async kvp =>
            {
                try
                {
                    await service.DownloadWebPageAsync(kvp.Value, cts.Token);
                    return (ApiName: kvp.Key, Success: true);
                }
                catch (OperationCanceledException)
                {
                    return (ApiName: (string?)null, Success: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠ {kvp.Key} завершился с ошибкой: {ex.Message}");
                    return (ApiName: (string?)null, Success: false);
                }
            }).ToList();

            // Ищем первого успешного победителя
            while (downloadTasks.Any())
            {
                // Task.WhenAny возвращает первый завершённый Task
                var completedTask = await Task.WhenAny(downloadTasks);
                var result = await completedTask;

                if (result.Success && result.ApiName != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n🏆 Победитель: {result.ApiName}");
                    Console.ResetColor();

                    // Отменяем остальные задачи
                    await cts.CancelAsync();
                    Console.WriteLine("\nВывод: WhenAny возвращает первый завершённый Task!");
                    Console.WriteLine("WhenAll ждёт все задачи, WhenAny - только первую!");
                    return;
                }

                downloadTasks.Remove(completedTask);
            }

            Console.WriteLine("⚠ Ни один API не ответил успешно");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в гонке задач: {ex.Message}");
        }
    }
}