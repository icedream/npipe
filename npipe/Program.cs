using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO.Pipes;

namespace npipe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 2)
            { Usage(); return; }

            switch (args.First().ToLower())
            {
                case "server":
                    {
                        try
                        {
                            NamedPipeServerStream pipe = new NamedPipeServerStream(args.Last(), PipeDirection.Out, 1);
                            pipe.WaitForConnection();
                            using (var stdin = Console.OpenStandardInput(32))
                            {
                                stdin.CopyTo(pipe, 32);
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                    break;
                case "client":
                    {
                        try
                        {
                            NamedPipeClientStream pipe = new NamedPipeClientStream(".", args.Last(), PipeDirection.In);
                            pipe.Connect();
                            using (var stdout = Console.OpenStandardOutput(32))
                            {
                                pipe.CopyTo(stdout, 32);
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                    break;
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\t" + Process.GetCurrentProcess().ProcessName + " <mode> <pipename>");
            Console.WriteLine();
            Console.WriteLine("Modes:");
            Console.WriteLine("\tserver\tMakes data available via a pipe. Writes to pipe from stdin.");
            Console.WriteLine("\tclient\tMakes data available from a pipe. Reads from pipe to stdout.");
        }
    }
}
