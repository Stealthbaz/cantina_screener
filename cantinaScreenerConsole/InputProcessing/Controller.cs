
using cantinaScreenerConsole.Model;
using Newtonsoft.Json.Linq;
using System;

namespace cantinaScreenerConsole.InputProcessing
{

    /**
     * Main Control object of the program.
     * This class has references to sub-classes which control input handling.
     * 
     * 
     */
    public class Controller
    {
        //Handle to the current user input/output processor
        private IInputProcessor currentInputProcessor;

        //The sub-processing classes that handle user input / output
        private MainMenuProcessor mainMenuProcessor;
        private UrlProcessor urlProcessor;
        private SearchProcessor searchProcessor;

        //the loaded JSON we will work with
        private JSONModel model;
        public JSONModel Model { get => model; set => model = value; }

        public Controller()
        {
            model = new JSONModel();
            mainMenuProcessor = new MainMenuProcessor(this, model);
            urlProcessor = new UrlProcessor(this, model);
            searchProcessor = new SearchProcessor(this, model);        
            currentInputProcessor = mainMenuProcessor;
        }

        public bool Process(string input)
        {
            if (input == "h")
            {
                currentInputProcessor.PrintInstructions();
                return false;
            }
            else
            {
               return currentInputProcessor.Process(input);               
            }
        }

        public void PrintInstructions()
        {
            currentInputProcessor.PrintInstructions();
        }


        public void SubProcessingComplete()
        {
            EnterMainMenuProcessor();
        }

        public void EnterMainMenuProcessor()
        {
            currentInputProcessor = mainMenuProcessor;
            currentInputProcessor.PrintInstructions();
        }

        public void EnterURLProcess()
        {
            currentInputProcessor = urlProcessor;
            currentInputProcessor.PrintInstructions();
        }

        public void EnterSearchProcess()
        {
            currentInputProcessor = searchProcessor;
            currentInputProcessor.PrintInstructions();
        }


    }
}
