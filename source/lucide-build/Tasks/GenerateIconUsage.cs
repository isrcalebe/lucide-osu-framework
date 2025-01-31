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

        addGetIconUsageFn();

        beginNestedClass("All");

        foreach (var icon in info.RootElement.EnumerateObject())
        {
            var iconNamePascal = icon.Name.Replace("-", "_").Pascalize();

            classBuilder.AppendLine();
            classBuilder.AppendLine("        /// <summary>");
            classBuilder.AppendLine($"        /// The <see href=\"https://lucide.dev/icons/{icon.Name}\"><c>{icon.Name}</c></see> icon.");
            classBuilder.AppendLine("        /// </summary>");
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
        classBuilder.AppendLine("    /// <summary>");
        classBuilder.AppendLine("    /// A dictionary of all Lucide icons and their unicode values.");
        classBuilder.AppendLine("    /// </summary>");
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

    private void addGetIconUsageFn()
    {
        classBuilder.AppendLine();
        classBuilder.AppendLine("    /// <summary>");
        classBuilder.AppendLine("    /// Get an icon usage by unicode value.");
        classBuilder.AppendLine("    /// </summary>");
        classBuilder.AppendLine("    /// <param name=\"unicode\">The unicode value of the icon.</param>");
        classBuilder.AppendLine("    /// <returns>An <see cref=\"IconUsage\"/> instance.</returns>");
        classBuilder.AppendLine($"    public static IconUsage Get(int unicode) => new IconUsage((char)unicode, \"Lucide\").With(weight: \"Regular\");");
        classBuilder.AppendLine();
    }

    private void beginNestedClass(string className)
    {
        classBuilder.AppendLine("    /// <summary>");
        classBuilder.AppendLine($"    /// A class containing all Lucide icons as <see cref=\"IconUsage\"/> properties.");
        classBuilder.AppendLine("    /// </summary>");
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
