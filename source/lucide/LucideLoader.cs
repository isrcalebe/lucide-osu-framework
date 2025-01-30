using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;

namespace Lucide.OsuFramework;

public static class LucideLoader
{
    /// <summary>
    /// Add the Lucide resource assembly to a resource store.
    /// </summary>
    /// <param name="self">The resource store to add the Lucide assembly to.</param>
    public static void AddLucideResourceAssembly(this ResourceStore<byte[]> self)
    {
        self.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(LucideLoader).Assembly), "Resources"));
    }

    public static void AddLucideFonts(this FontStore self, ResourceStore<byte[]> store, IResourceStore<TextureUpload> textureLoaderStore)
    {
        self.AddTextureSource(new RawCachingGlyphStore(store, "Fonts/Lucide-Regular", textureLoaderStore));
    }
}
