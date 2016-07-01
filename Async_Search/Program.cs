using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Async_Search
{
    class Program
    {
        static void Main(string[] args)
        {
            WebPageProcessor webPageProcessor = new WebPageProcessor();
            webPageProcessor.FindHtmlPageElementsAsync("http://www.w3schools.com/bootstrap/bootstrap_ref_js_carousel.asp", ".carousel-inner");

            //simulating long running work in Main. Interaction with UI for example
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main(): processing...");
                //add some dalay
                Thread.Sleep(200);
            }

            Console.ReadLine();
        }
    }
}
