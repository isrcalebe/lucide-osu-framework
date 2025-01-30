using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lucide.Build.Tasks;

namespace Lucide.Build;

public static class Program
{
    private static Dictionary<string, ILucideTask> tasks = new()
    {
        { "fetch-info", new FetchLucideInfo() },
        { "generate-icon-usage", new GenerateIconUsage() }
    };

    public static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No task specified.");
            return;
        }

        if (!tasks.TryGetValue(args[0], out var task))
        {
            Console.WriteLine("Unknown task: " + args[0]);
            return;
        }

        await task.RunAsync(args[1..]).ConfigureAwait(false);
    }
}
