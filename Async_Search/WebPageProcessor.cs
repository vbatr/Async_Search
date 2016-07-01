using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Async_Search
{
    public class WebPageProcessor
    {
        /// <summary>
        /// Searches and displays formatted result for given search pattern
        /// </summary>
        /// <param name="webPageUri">Requested web page uri</param>
        /// <param name="searchPattern">Search pattern</param>
        /// <returns></returns>
        public async Task FindHtmlPageElementsAsync(string webPageUri, string searchPattern)
        {
            if (string.IsNullOrEmpty(webPageUri))
                throw new ArgumentNullException("webPageUri");
            if (string.IsNullOrEmpty(webPageUri))
                throw new ArgumentNullException("searchPattern");

            try
            {
                //get html page asynchronously
                var htmlString = await GetHtmlPageAsync(webPageUri);

                if (string.IsNullOrEmpty(htmlString))
                    return;

                //Find html elements and show formatted result
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);

                //as all required data is placed inside table, try to find all tr with search condition
                var foundRowsList =
                    htmlDocument.DocumentNode.SelectNodes("//tr")
                        .Where(n => n.InnerHtml.Contains(searchPattern))
                        .ToList();

                if (!foundRowsList.Any())
                {
                    Console.WriteLine("No results found for given search pattern: {0}", searchPattern);
                }
                else
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("***Search results***");
                    foreach (var row in foundRowsList)
                    {
                        var formatedResult = new StringBuilder();

                        foreach (var child in row.ChildNodes)
                        {
                            formatedResult.Append(child.InnerText);
                            formatedResult.Append("\t");
                        }
                        Console.WriteLine(formatedResult.ToString());
                    }
                    Console.WriteLine(Environment.NewLine);
                }
            }
            catch
            {
                Console.WriteLine("Processing of search results were stopped");
            }
        }

        /// <summary>
        /// Gets html page asynchronously
        /// </summary>
        /// <param name="webPageUri">Requested web page uri</param>
        /// <returns></returns>
        public async Task<string> GetHtmlPageAsync(string webPageUri)
        {
            if (string.IsNullOrEmpty(webPageUri))
                throw new ArgumentNullException("webPageUri");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Task<string> httpClientTask = client.GetStringAsync(webPageUri);

                    //We can do additional processing here

                    string result = await httpClientTask;

                    return result;
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Requested page not found");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            return string.Empty;
        }
    }
}
