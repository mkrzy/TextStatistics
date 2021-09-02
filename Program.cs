using System;
using System.Net;
using System.Collections.Generic;

namespace TextStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string textPrompt = "What is the source of your text? Type the corresponding number and press enter:\n\n"
                + "1 - Website link (ex. https://www.gutenberg.org/files/45839/45839.txt)\n"
                + "2 - Free text (ex. Here is my text, I am just typing it now!)\n"
                + "3 - File input (ex. C:\\Users\\Public\\TestFolder\\Text.txt)\n";

            List<TextStatistics> textList = new List<TextStatistics>();
            WebClient client = new WebClient();
            bool firstRun = true, waitingForInput = true;

            Console.Clear();
            Console.WriteLine("Hi - welcome to the text statistics calculator. Here you can find the most frequent words \n"
                + "of a given text, along with the longest words, number of words, and number of lines.\n\n"
                + textPrompt
                + "4 - Run with three default links\n"
                + "5 - EXIT\n");

            while(waitingForInput){
                string input = Console.ReadLine();
                string text = "";
                string source = "";
                switch(input)
                {
                    case "1":
                        // Website link
                        Console.WriteLine("What is the URL of the text?");
                        source = Console.ReadLine();
                        if(String.IsNullOrWhiteSpace(source))
                        {
                            break;
                        }
                        text = client.DownloadString(source);
                        break;

                    case "2":
                        // Free text
                        Console.WriteLine("What is the text you wish to enter?");
                        text = Console.ReadLine();
                        break;

                    case "3":
                        // File input
                        Console.WriteLine("What is the filepath of the text?");
                        source = Console.ReadLine();
                        if(String.IsNullOrWhiteSpace(source))
                        {
                            break;
                        }
                        text = System.IO.File.ReadAllText(@source);
                        break;

                    case "4":
                        if(firstRun){
                            // Do default text links
                            textList.Add(new TextStatistics(client.DownloadString("https://www.gutenberg.org/files/2197/2197-0.txt")));
                            textList.Add(new TextStatistics(client.DownloadString("https://www.gutenberg.org/files/2500/2500-0.txt")));
                            textList.Add(new TextStatistics(client.DownloadString("https://www.gutenberg.org/files/1727/1727-0.txt")));
                            
                            // Show individual text statistics
                            foreach(TextStatistics ts in textList){
                                ts.printTextStatistics();
                            }
                        }

                        // Write text statistics for all texts combined
                        Console.WriteLine("Text statistics for all texts combined: ");
                        TextStatistics allTextStatistics = new TextStatistics(textList);
                        allTextStatistics.printTextStatistics();

                        waitingForInput = false;
                        break;

                    case "5":
                        // Exit program
                        Console.WriteLine("Goodbye!\n");
                        waitingForInput = false;
                        break;

                    default:
                        break;
                }

                if(!waitingForInput)
                {
                    break;
                }

                if(!String.IsNullOrWhiteSpace(text))
                {
                    TextStatistics ts = new TextStatistics(text);
                    textList.Add(ts);
                    Console.WriteLine();
                    ts.printTextStatistics();
                    firstRun = false;

                    Console.WriteLine("\nYou can add more text to calculate statistics for!\n"
                        + textPrompt
                        + "4 - I'm all done. Tell me the stats!\n"
                        + "5 - EXIT\n");
                }
                else
                {
                    Console.WriteLine("Please select a menu option.\n");
                }
            }
        }
    }
}
