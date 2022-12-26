using System;

namespace Events
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Events.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}