using cantinaScreenerConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cantinaScreenerConsole.InputProcessing
{
    public class MainMenuProcessor : IInputProcessor
    {
        Controller controller;
        JSONModel model;
        
        public MainMenuProcessor(Controller aController, JSONModel aModel)
        {
            model = aModel;
            controller = aController;            
        }

        bool IInputProcessor.Process(string input)
        {
            switch (input)
            {
                case "x":
                case "exit":
                    {
                        Console.WriteLine("Thank You For Playing");
                        return true;
                    }
                case "p":
                    {
                        string jsonString = model.ToString();
                        if (jsonString != null)
                        {
                            Console.WriteLine(jsonString);
                        }

                        ((IInputProcessor)this).PrintInstructions();
                        return false;
                    }
                case "l":
                case "load":
                    {
                        controller.EnterURLProcess();
                        return false;
                    }
                case "s":
                case "search":
                    {
                        controller.EnterSearchProcess();
                        return false;
                    }
                default:
                    {
                        Console.WriteLine("Unknown Input");
                        ((IInputProcessor)this).PrintInstructions();
                        return false;
                    }

            }
        }

        void IInputProcessor.PrintInstructions()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Main Menu");
            Console.WriteLine("------------------------");
            Console.WriteLine("l  -  load json file");
            Console.WriteLine("p  -  print currently loaded json file");
            Console.WriteLine("s  -  enter search mode for currently loaded json file");
            Console.WriteLine("x  -  exit");
            Console.WriteLine("------------------------");
            Console.Write(">");
        }
    }
}
