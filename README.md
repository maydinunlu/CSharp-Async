# CSharp-Async
Resoure: [IAmTimCorey: C# Async / Await - Make your app more responsive and faster with asynchronous programming](https://www.youtube.com/watch?v=2moh18sh5p4)

## C# Code
```c#
public class WebSiteDataModel
{
    public string Url;
    public string Data;

    public override string ToString()
    {
        return $"Url: {Url}, Downloaded Data Length: {Data.Length}";
    }
}
```

```c#
public class WebSiteDownloader
{
    public void RunDownload(List<string> urlList)
    {
        foreach (var url in urlList)
        {
            WebSiteDataModel result = DownloadWebSite(url);
            Console.WriteLine($"{result}");
        }
    }

    public async Task RunDownloadAsync(List<string> urlList)
    {
        foreach (var url in urlList)
        {
            WebSiteDataModel result = await Task.Run(() => DownloadWebSite(url));
            Console.WriteLine($"{result}");
        }
    }

    public async Task RunDownloadParallelAsync(List<string> urlList)
    {
        List<Task<WebSiteDataModel>> taskList = new List<Task<WebSiteDataModel>>();

        foreach (var url in urlList)
        {
            taskList.Add(Task.Run(() => DownloadWebSite(url)));
        }

        var results = await Task.WhenAll(taskList);
        foreach (var result in results)
        {
            Console.WriteLine($"{result}");
        }
    }


    private WebSiteDataModel DownloadWebSite(string url)
    {
        var dataModel = new WebSiteDataModel();
        var client = new WebClient();

        dataModel.Url = url;
        dataModel.Data = client.DownloadString(url);

        return dataModel;
    }
}
```

---------------------------------------

## Run: Normal
```c#
private static void StartDownloadNormal()
{
    WebSiteDownloader downloader = new WebSiteDownloader();

    var watch = Stopwatch.StartNew();
    Console.WriteLine("\nStart Download: Normal");
    downloader.RunDownload(_urlList);
    watch.Stop();

    var elapsedTime = watch.ElapsedMilliseconds;
    Console.WriteLine($"Total Elapsed Time: {elapsedTime}");
}
```

## Console Output: Normal
```console
Start Download: Normal
Url: https://www.google.com, Downloaded Data Length: 48675
Url: https://www.yahoo.com, Downloaded Data Length: 135864
Url: https://www.microsoft.com, Downloaded Data Length: 168954
Url: https://www.stackoverflow.com, Downloaded Data Length: 121353
Total Elapsed Time: 3161
```

---------------------------------------

## Run: Async
```c#
private static async void StartDownloadAsync()
{
    WebSiteDownloader downloader = new WebSiteDownloader();

    var watch = Stopwatch.StartNew();
    Console.WriteLine("\nStart Download: Async");
    await downloader.RunDownloadAsync(_urlList);
    watch.Stop();

    var elapsedTime = watch.ElapsedMilliseconds;
    Console.WriteLine($"Total Elapsed Time: {elapsedTime}");
}
```

## Console Output: Async
### Elapsed time is almost same with Normal, but UI is responsive
```console
Start Download: Async
Url: https://www.google.com, Downloaded Data Length: 48684
Url: https://www.yahoo.com, Downloaded Data Length: 135864
Url: https://www.microsoft.com, Downloaded Data Length: 168956
Url: https://www.stackoverflow.com, Downloaded Data Length: 121353
Total Elapsed Time: 2478
```

---------------------------------------

## Run: ParallelAsync
```c#
private static async void StartDownloadParallelAsync()
{
    WebSiteDownloader downloader = new WebSiteDownloader();

    var watch = Stopwatch.StartNew();
    Console.WriteLine("\nStart Download: ParallelAsync");
    await downloader.RunDownloadParallelAsync(_urlList);
    watch.Stop();

    var elapsedTime = watch.ElapsedMilliseconds;
    Console.WriteLine($"Total Elapsed Time: {elapsedTime}");
}
```

## Console Output: Async
### Elapsed time is smaller than the others, because all the download requests are parallel
```console
Start Download: ParallelAsync
Url: https://www.google.com, Downloaded Data Length: 48711
Url: https://www.yahoo.com, Downloaded Data Length: 135864
Url: https://www.microsoft.com, Downloaded Data Length: 168956
Url: https://www.stackoverflow.com, Downloaded Data Length: 121353
Total Elapsed Time: 894
```
