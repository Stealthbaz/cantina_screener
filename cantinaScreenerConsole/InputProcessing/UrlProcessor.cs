using cantinaScreenerConsole.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace cantinaScreenerConsole.InputProcessing
{
    public class UrlProcessor : IInputProcessor
    {
        private const string defaultURL = "https://raw.githubusercontent.com/jdolan/quetoo/master/src/cgame/default/ui/settings/SystemViewController.json";

        private Controller controller;
        private JSONModel model;
        public UrlProcessor(Controller aController, JSONModel aModel)
        {
            controller = aController;
            model = aModel;

        }

        public void PrintInstructions()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Load JSON");
            Console.WriteLine("------------------------");
            Console.WriteLine("Type the URL of the JSON file you wish to import, OR enter a menu item below:");
            Console.WriteLine("m - return to main menu");
            Console.WriteLine("d - use default url (https://raw.githubusercontent.com/jdolan/quetoo/.../SystemViewController.json)");
            Console.WriteLine("p - print currently loaded json");
            Console.WriteLine("------------------------");
            Console.Write(">");
        }

        public bool Process(string input)
        {
            if (input == "x" || input == "m")
            {
                controller.SubProcessingComplete();
                return false;
            }
            else if (input == "p")
            {
                string jsonString = model.ToString();
                if (jsonString != null)
                {
                    Console.WriteLine(jsonString);
                }

                PrintInstructions();
                return false;
            }
            else
            {
                if (input == "d")
                    input = defaultURL;               

                try
                {
                    model.LoadJSONFromURL(input);
                    Console.WriteLine("-------------------------");
                    Console.WriteLine(string.Format("Json Loaded from {0}", input));
                    Console.WriteLine("-------------------------");
                    controller.SubProcessingComplete();
                    return false;
                }
                catch (Exception e)
                {
                    PrintErrorText(input, e);                    
                }

                PrintErrorText(input, null);
                return false;
            }            
        }

        private void PrintErrorText(string input, Exception exception)
        {
            Console.WriteLine(string.Format("Unable to Load JSON from {0}.  Please enter another URL", input));
            if (exception != null)
            {
                Console.WriteLine(exception.ToString());
            }
            Console.Write(">");
        }
}
}