global using osu.Framework.Allocation;
global using osu.Framework.Bindables;
global using osu.Framework.Graphics;
global using osu.Framework.Graphics.Primitives;
global using osu.Framework.Logging;
global using osu.Framework.Testing;
global using osu.Framework.Utils;
global using osuTK;

namespace Lucide.OsuFramework.Tests;

public partial class LucideTestGame : osu.Framework.Game
{
    private DependencyContainer? dependencies;

    [BackgroundDependencyLoader]
    private void load()
    {
        Resources.AddLucideResourceAssembly();
        Fonts.AddLucideFonts(Resources, Host.CreateTextureLoaderStore(Resources));
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
}
