using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lucide.Build.Helpers;

namespace Lucide.Build.Tasks;

public class FetchLucideInfo : ILucideTask
{
    public async Task<bool> RunAsync(string[] args)
    {
        using var client = LucideHelpers.CreateHttpClient();
        var version = args.FirstOrDefault("latest");

        var fileDir = LucideHelpers.GetDirectoryPath(Path.Combine(Environment.CurrentDirectory, "dist"));
        var fileName = Path.Combine(fileDir, "info.json");
        var fileBytes = await client.GetByteArrayAsync($"https://unpkg.com/lucide-static@{version}/font/info.json").ConfigureAwait(false);
        await File.WriteAllBytesAsync(fileName, fileBytes).ConfigureAwait(false);

        return true;
    }
}

