using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fictional_Lamp
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                using CancellationTokenSource source = new();
                TaskFactory factory = new(source.Token);

                await factory.StartNew(() =>
                {
                    MonitorKeypress(source);
                }, source.Token);

                Console.WriteLine("Bye. Thank you for using this app.");
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException tce)
                        Console.WriteLine("Unable to compute mean: {0}",
                                          tce.Message);
                    else
                        Console.WriteLine("Exception: " + e.GetType().Name);
                }
                return -1;
            }            
            return 0;
        }

        public static void MonitorKeypress(CancellationTokenSource source)
        {
            ConsoleKey ck;
            Console.WriteLine();
            do
            {
                Console.Write("Enter a signal. Press Any key to Continue, ESC to Abort: ");
                ck = Console.ReadKey(true).Key;
                Console.Write("\n");
            } while (ck != ConsoleKey.Escape);
            source.Cancel();
        }
    }
}
