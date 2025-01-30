using System;
using System.Linq;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace Lucide.OsuFramework.Tests.Visual;

public partial class TestSceneLucideIcons : LucideTestScene
{
    private SpriteIcon? spriteIcon;
    private FillFlowContainer? info;

    private SpriteText? iconClassNameText, iconUnicodeText, iconSizeText;

    private BasicButton? previousButton, nextButton;

    private int currentIndex = 0;

    private BasicHexColourPicker? colourPicker;

    [BackgroundDependencyLoader]
    private void load()
    {
        AddRange(
        [
            info = new FillFlowContainer
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Both,
                Children =
                [
                    iconClassNameText = new SpriteText(),
                    iconUnicodeText = new SpriteText(),
                    iconSizeText = new SpriteText(),
                ]
            },
            colourPicker = new BasicHexColourPicker
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
            },
            previousButton = new BasicButton
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Margin = new MarginPadding { Left = 10 },
                Text = "Previous",
                Action = previousIcon,
                Size = new Vector2(100, 50)
            },
            nextButton = new BasicButton
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 10 },
                Text = "Next",
                Action = nextIcon,
                Size = new Vector2(100, 50)
            },
            spriteIcon = new SpriteIcon
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(24),
            }
        ]);

        var icon = LucideIcons.IconsUnicode.Values.ElementAt(currentIndex);
        var name = LucideIcons.IconsUnicode.Keys.ElementAt(currentIndex);

        iconClassNameText!.Text = $"Icon Class Name: {name}";
        iconUnicodeText!.Text = $"Icon Unicode: 0x{icon:X}";

        spriteIcon!.Icon = LucideIcons.Get(icon);

        iconSizeText!.Text = $"Icon Size: {spriteIcon.Size}";

        colourPicker.Current.ValueChanged += e => spriteIcon.Colour = e.NewValue;

        AddSliderStep("Icon Size", 16, 48, 24, size =>
        {
            spriteIcon.Size = new Vector2(size);
            iconSizeText.Text = $"Icon Size: {size}";
        });
    }

    private void nextIcon()
    {
        if (currentIndex < LucideIcons.IconsUnicode.Count - 1)
            currentIndex++;
        else
            currentIndex = 0;

        var icon = LucideIcons.IconsUnicode.Values.ElementAt(currentIndex);
        var name = LucideIcons.IconsUnicode.Keys.ElementAt(currentIndex);

        iconClassNameText!.Text = $"Icon Class Name: {name}";
        iconUnicodeText!.Text = $"Icon Unicode: 0x{icon:X}";

        spriteIcon!.Icon = LucideIcons.Get(icon);

        iconSizeText!.Text = $"Icon Size: {spriteIcon.Size}";
    }

    private void previousIcon()
    {
        if (currentIndex > 0)
            currentIndex--;
        else
            currentIndex = LucideIcons.IconsUnicode.Count - 1;

        var icon = LucideIcons.IconsUnicode.Values.ElementAt(currentIndex);
        var name = LucideIcons.IconsUnicode.Keys.ElementAt(currentIndex);

        iconClassNameText!.Text = $"Icon Class Name: {name}";
        iconUnicodeText!.Text = $"Icon Unicode: 0x{icon:X}";

        spriteIcon!.Icon = LucideIcons.Get(icon);

        iconSizeText!.Text = $"Icon Size: {spriteIcon.Size}";
    }

    private void testIcon(IconUsage icon)
    {
        spriteIcon!.Icon = icon;
    }
}
