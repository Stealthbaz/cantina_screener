using cantinaScreenerConsole.InputProcessing;
using System;

namespace cantinaScreenerConsole
{
    class Program
    {

        /**
         *  This program allows a user to load JSON from a web address, 
         *  Then it allows you to search that JSON for specific objects within the JSON.         
         **/
        static void Main(string[] args)
        {
            Console.WriteLine("Hello and Welcome to the JSON Search Application.");

            bool done = false;
            string input = "";
            Controller controller = new Controller();
            controller.PrintInstructions();

            while (!done)
            {
                input = Console.ReadLine();
                done = controller.Process(input);
            }
        }
    }
}
