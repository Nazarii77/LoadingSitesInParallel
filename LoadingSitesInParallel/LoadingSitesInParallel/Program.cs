using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoadingSitesInParallel
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Loading sites");
            //var test = Console.ReadLine();

            var swSites = new Stopwatch();


            swSites.Start();
            string testSite1 = await GetHTMLfromURL("http://google.com").ConfigureAwait(false);
            string testSite3 = await GetHTMLfromURL("https://github.com/").ConfigureAwait(false);
            string testSite4 = await GetHTMLfromURL("https://stackoverflow.com/").ConfigureAwait(false);
            swSites.Stop();

            TimeSpan timeSpanSites = swSites.Elapsed;
            Console.WriteLine("Sites loaded 1");
            Console.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpanSites.Hours, timeSpanSites.Minutes, timeSpanSites.Seconds, timeSpanSites.Milliseconds);
            //var testSites = Console.ReadLine();

            var swSitesSecondRun = new Stopwatch();
            swSitesSecondRun.Start();
            string testSite1SecondRun = await GetHTMLfromURL("http://google.com").ConfigureAwait(false);
            string testSite3SecondRun = await GetHTMLfromURL("https://github.com/").ConfigureAwait(false);
            string testSite4SecondRun = await GetHTMLfromURL("https://stackoverflow.com/").ConfigureAwait(false);
            swSites.Stop();

            TimeSpan timeSpanSitesSecondRun = swSites.Elapsed;
            Console.WriteLine("Sites loaded 2");
            Console.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpanSitesSecondRun.Hours, timeSpanSitesSecondRun.Minutes, timeSpanSitesSecondRun.Seconds, timeSpanSitesSecondRun.Milliseconds);
            //var testSitesSecondRun = Console.ReadLine();

            var swSites2 = new Stopwatch();
            swSites2.Start();
            string[] sites = { "http://google.com", "https://github.com/", "https://stackoverflow.com/" };
            var messages = new ConcurrentBag<string>();
            Parallel.ForEach(sites, async site =>
            messages.Add(await GetHTMLfromURL(site).ConfigureAwait(false))
                );
            var resultParallelSitesLoading = string.Join(Environment.NewLine, messages);
            swSites2.Stop();

            TimeSpan timeSpanSites2 = swSites2.Elapsed;
            Console.WriteLine("Sites loaded 3");
            Console.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpanSites2.Hours, timeSpanSites2.Minutes, timeSpanSites2.Seconds, timeSpanSites2.Milliseconds);
            // var testSites2 = Console.ReadLine();


            var swSites3 = new Stopwatch();
            swSites3.Start();
            string[] sites3 = { "http://google.com", "https://github.com/", "https://stackoverflow.com/" };
            var messages3 = new ConcurrentBag<string>();
            Parallel.ForEach(Partitioner.Create(0, sites3.Count()), async (range) =>
            {
                messages3.Add(await GetHTMLfromURL(sites3[range.Item1]).ConfigureAwait(false));
            });
            var resultParallelSitesLoading3 = string.Join(Environment.NewLine, messages);
            swSites3.Stop();

            TimeSpan timeSpanSites3 = swSites3.Elapsed;
            Console.WriteLine("Sites loaded 4");
            Console.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpanSites3.Hours, timeSpanSites3.Minutes, timeSpanSites3.Seconds, timeSpanSites3.Milliseconds);
            var test = messages3;
            var testSites3 = Console.ReadLine();

            //    Loading sites
            //    Sites loaded 1
            //    Time: 0h 0m 1s 818ms
            //    Sites loaded 2
            //    Time: 0h 0m 1s 818ms
            //    Sites loaded 3
            //    Time: 0h 0m 0s 665ms
            //    Sites loaded 4
            //    Time: 0h 0m 0s 462ms
        }
        public static async Task<string> GetHTMLfromURL(string urlAddress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            Thread.Sleep(200);
            return data.ToString();
        }

    }
}
