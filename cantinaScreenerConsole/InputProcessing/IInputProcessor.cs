namespace cantinaScreenerConsole.InputProcessing
{   
    public interface IInputProcessor
    {
        bool Process(string input);
        void PrintInstructions();
    }
}
