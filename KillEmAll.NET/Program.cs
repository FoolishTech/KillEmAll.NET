using System;

namespace KillEmAll.NET
{
    class Program
    {
        static void pressAnyKeyToExit()
        {
            Console.WriteLine("");
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            try
            {
                KillEmAll foo = new KillEmAll();
                foo.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main] Exception: {0}", ex.Message);
            }
            pressAnyKeyToExit();
        }
    }



}
