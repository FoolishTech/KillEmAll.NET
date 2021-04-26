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
                //Env.InitEnv();
                KillEmAll foo = new KillEmAll();
                foo.doWork();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main] Exception: {0}", ex.Message);
            }
            pressAnyKeyToExit();
        }
    }



}
