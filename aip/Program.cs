using OpenAI;
using OpenAI.Completions;
using OpenAI.Edits;
using OpenAI.Images;
using OpenAI.Models;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Net;
using System.IO;

namespace aip
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            string Apikey = System.IO.File.ReadAllText("ApiKey.txt");
            Console.WriteLine("-------------------------------------------------Blackbird 6 - 23/02/12-------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("End with proper punctuation");
            Console.WriteLine("Press Enter");
            var program = Console.ReadLine();
            Console.Clear();
            var current = "";
            var api = new OpenAIClient(new OpenAIAuthentication(Apikey));
            if (program == "0")
            {
                while (true)
                {
                    var input = Console.ReadLine();
                    var edit_input = input + ":";
                    current = current + input;
                    //var Giben_input = current;

                    //var read= Console.ReadLine();
                    await foreach (var token in api.CompletionsEndpoint.StreamCompletionEnumerableAsync(current, maxTokens: 300, temperature: 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1, model: Model.Davinci))
                    {
                        Console.Write(token);
                        var help = token;
                        current = current + help;
                    }
                    Console.WriteLine();
                }

            }
            else if (program == "2")
            {
                int name = 0;
                foreach (string line in System.IO.File.ReadLines("readtext.txt"))
                {
                    System.Console.WriteLine(line);
                    var imagegen = await api.ImagesEndPoint.GenerateImageAsync(line.ToString(), 1, ImageSize.Small);

                    foreach (var result in imagegen)
                    {
                        using (var client = new HttpClient())
                        {
                            using (var s = client.GetStreamAsync(result))
                            {
                                using (var fs = new FileStream("output/"+name+".jpg", FileMode.Create))
                                {
                                    //File.Move("output/image.jpg", line);
                                    s.Result.CopyTo(fs);
                                }
                            }

                        }
                        name++;

                    }
                }
            }
            else
            {
                //var image_input = 0;
                var image_input = Console.ReadLine();
                Console.WriteLine("");
                // result == file://path/to/image.png
            }

            }
        }
    }