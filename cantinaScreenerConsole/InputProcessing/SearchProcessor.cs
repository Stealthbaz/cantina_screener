using cantinaScreenerConsole.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace cantinaScreenerConsole.InputProcessing
{
    public class SearchProcessor : IInputProcessor
    {
        Controller controller;
        JSONModel model;

        public SearchProcessor (Controller aController, JSONModel aModel)
        {
            controller = aController;
            model = aModel;
        }
        

        public void PrintInstructions()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Searching Within Loaded JSON");            
            Console.WriteLine("------------------------");
            Console.WriteLine("x - main menu");
            Console.WriteLine("ex - examples");
            Console.WriteLine("------------------------");
            Console.Write(">");            
        }

        public void PrintExamples()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Examples");
            Console.WriteLine("------------------------");
            Console.WriteLine("Search by class:  Input  |  Button | Box");
            Console.WriteLine("Search by identifier:  Button#apply");
            Console.WriteLine("Search by className:  StackView.container");                        
            Console.WriteLine("------------------------");
            Console.Write(">");
        }

        public bool Process(string input)
        {
            if (input == "x")
            {
                controller.SubProcessingComplete();
                return false;
            }
            else if (input == "ex")
            {
                PrintExamples();
                return false;
            }           
            else
            {

                IEnumerable<JToken> results = null;
                try
                {
                    results = model.SearchJSON(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                

                int count = 0;
                if (results != null)
                {
                    foreach (JToken token in results)
                    {
                        Console.WriteLine(token.ToString());
                        Console.WriteLine("------------------------");
                        count++;
                    }
                }
                Console.WriteLine(string.Format("{0} results", count));


                return false;
            }
        }
    }
}