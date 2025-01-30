using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Lucide.Build.Helpers;

namespace Lucide.Build.Tasks;

public class GenerateIconUsage : ILucideTask
{
    private StringBuilder classBuilder = LucideHelpers.CreateClassBuilder("LucideIcons");

    public async Task<bool> RunAsync(string[] args)
    {
        var info = await LucideHelpers.GetLucideInfoAsync().ConfigureAwait(false);

        beginDictionary();

        foreach (var icon in info.RootElement.EnumerateObject())
        {
            var value = icon.Value.GetProperty("encodedCode").GetString();
            if (value != null)
            {
                var unicode = value.Replace("\\", string.Empty).ToUpperInvariant();

                addDictionaryEntry(icon.Name, $"0x{unicode}");
            }
            else
                Console.WriteLine($"No unicode for {icon.Name}");
        }

        endDictionary();

        addGetIconUsaGenericgeFn();

        beginNestedClass("All");

        foreach (var icon in info.RootElement.EnumerateObject())
        {
            var iconNamePascal = icon.Name.Replace("-", "_").Pascalize();

            classBuilder.AppendLine();
            classBuilder.AppendLine($"        public static IconUsage {iconNamePascal} => Get(LucideIcons.IconsUnicode[\"{icon.Name}\"]);");
        }

        endNestedClass();

        endClass();

        // write file
        var fileDir = LucideHelpers.GetDirectoryPath(Path.Combine(args[0]));
        var fileName = Path.Combine(fileDir, "LucideIcons.cs");

        await File.WriteAllTextAsync(fileName, classBuilder.ToString()).ConfigureAwait(false);

        return true;
    }

    private void beginDictionary()
    {
        classBuilder.AppendLine("    public static IReadOnlyDictionary<string, int> IconsUnicode = new Dictionary<string, int>");
        classBuilder.AppendLine("    {");
    }

    private void addDictionaryEntry(string name, string unicode)
    {
        classBuilder.AppendLine($"        {{ \"{name}\", {unicode} }},");
    }

    private void endDictionary()
    {
        classBuilder.AppendLine("    };");
    }

    private void addGetIconUsaGenericgeFn()
    {
        classBuilder.AppendLine();
        classBuilder.AppendLine($"    public static IconUsage Get(int unicode) => new IconUsage((char)unicode, \"Lucide\").With(weight: \"Regular\");");
        classBuilder.AppendLine();
    }

    private void beginNestedClass(string className)
    {
        classBuilder.AppendLine($"    public static class {className}");
        classBuilder.AppendLine("    {");
    }

    private void endNestedClass()
    {
        classBuilder.AppendLine("    }");
    }

    private void endClass()
    {
        classBuilder.AppendLine("}");
    }
}
