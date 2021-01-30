using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace DenoAsync
{
    public class WebSiteDataModel
    {
        public string Url;
        public string Data;

        public override string ToString()
        {
            return $"Url: {Url}, Downloaded Data Length: {Data.Length}";
        }
    }

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

    static class Program
    {
        private static List<string> _urlList = new List<string>()
        {
            "https://www.google.com",
            "https://www.yahoo.com",
            "https://www.microsoft.com",
            "https://www.stackoverflow.com"
        }; 
        
        static void Main(string[] args)
        {
            var selectedStyle = GetInput();
            if (selectedStyle == 1)
            {
                StartDownloadNormal();    
            }
            else if (selectedStyle == 2)
            {
                StartDownloadAsync();    
            }
            else if (selectedStyle == 3)
            {
                StartDownloadParallelAsync();    
            }

            Console.ReadLine();
        }

        private static int GetInput()
        {
            Console.WriteLine("Select Download Style");
            Console.WriteLine("=====================");
            Console.WriteLine("1 -> Normal");
            Console.WriteLine("2 -> Async");
            Console.WriteLine("3 -> ParallelAsync");
            Console.WriteLine("=====================");
            
            Console.Write("Select: -> ");
            int selectedStyle = 1;
            while (true)
            {
                try
                {
                    selectedStyle = Convert.ToInt32(Console.ReadLine());
                    if (selectedStyle == 1 || 
                        selectedStyle == 2 || 
                        selectedStyle == 3)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wrong input! This must be: 1 | 2 | 3");
                    Console.Write("Select: -> ");
                }
            }

            return selectedStyle;
        }
        
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
    }
}