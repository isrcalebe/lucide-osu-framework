using Lucide.OsuFramework.Tests;
using osu.Framework;

using var host = Host.GetSuitableDesktopHost("lucide-visual-tests", new HostOptions
{
    PortableInstallation = true
});
using var game = new LucideTestBrowser();

host.Run(game);
