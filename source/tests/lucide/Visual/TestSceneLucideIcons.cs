using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Framework.Threading;

namespace Lucide.OsuFramework.Tests.Visual;

public partial class TestSceneLucideIcons : LucideTestScene
{
    private ScheduledDelegate? scheduledDelegate;

    [Test]
    public void TestPreloadIcon()
    {
        SpriteIcon? icon = null;

        AddStep("create icon", () => icon = new SpriteIcon
        {
            Size = new Vector2(20),
            Icon = LucideIcons.All.Anchor
        });
        AddStep("preload icon", () => LoadComponent(icon));
        AddStep("change icon", () => icon!.Icon = LucideIcons.All.Archive);
        AddStep("add icon", () => Add(icon));
    }

    [Test]
    public void TestOneIconAtTime()
    {
        FillFlowContainer flow = null!;
        Icon[] icons = null!;

        var i = 0;

        AddStep("prepare test", () =>
        {
            i = 0;
            icons = [.. getAllIcons()];
            scheduledDelegate?.Cancel();

            Child = new TooltipContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    new BasicScrollContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = flow = new FillFlowContainer
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(5),
                            Direction = FillDirection.Full
                        }
                    }
                ]
            };
        });

        AddStep("start adding icons", () =>
        {
            scheduledDelegate = Scheduler.AddDelayed(() =>
            {
                flow.Add(icons[i++]);

                if (++i > icons.Length - 1)
                    scheduledDelegate?.Cancel();
            }, 5, true);
        });
    }

    [Test]
    public void TestLoadAllIcons()
    {
        Box background = null!;
        FillFlowContainer flow = null!;

        AddStep("prepare test", () =>
        {
            scheduledDelegate?.Cancel();
            Child = new TooltipContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    background = new Box
                    {
                        Colour = Colour4.Teal,
                        RelativeSizeAxes = Axes.Both,
                    },
                    new BasicScrollContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = flow = new FillFlowContainer
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(5),
                            Direction = FillDirection.Full,
                        },
                    }
                ]
            };

            var categories = typeof(LucideIcons).GetNestedTypes();

            foreach (var category in categories)
            {
                flow.Add(new SpriteText
                {
                    Text = category.Name,
                    Scale = new Vector2(4),
                    RelativeSizeAxes = Axes.X,
                    Padding = new MarginPadding(10),
                });

                foreach (var icon in getAllIconsForCategory(category))
                    flow.Add(icon);
            }
        });
        AddStep("toggle shadows", () => flow.Children.OfType<Icon>().ForEach(i => i.SpriteIcon.Shadow = !i.SpriteIcon.Shadow));
        AddStep("change icons", () => flow.Children.OfType<Icon>().ForEach(i => i.SpriteIcon.Icon = new IconUsage((char)(i.SpriteIcon.Icon.Icon + 1))));
        AddStep("white background", () => background.FadeColour(Colour4.White, 200));
        AddStep("move shadow offset", () => flow.Children.OfType<Icon>().ForEach(i => i.SpriteIcon.ShadowOffset += Vector2.One));
        AddStep("change shadow colour", () => flow.Children.OfType<Icon>().ForEach(i => i.SpriteIcon.ShadowColour = Colour4.Pink));
        AddStep("add new icon with colour and offset", () =>
            flow.Add(new Icon("LucideIcons.All.Handshake", LucideIcons.All.Handshake)
            {
                SpriteIcon = { Shadow = true, ShadowColour = Colour4.Orange, ShadowOffset = new Vector2(5, 1) }
            }));
    }

    private static IEnumerable<Icon> getAllIcons()
    {
        var categories = typeof(LucideIcons).GetNestedType("All") ?? throw new InvalidOperationException("Could not find All class in LucideIcons.");

        foreach (var property in categories.GetProperties(BindingFlags.Public | BindingFlags.Static))
        {
            var value = property.GetValue(null);
            Debug.Assert(value != null);

            yield return new Icon($"{nameof(LucideIcons)}.{property.Name}", (IconUsage)value);
        }
    }

    private static IEnumerable<Icon> getAllIconsForCategory(Type category)
    {
        foreach (var p in category.GetProperties(BindingFlags.Public | BindingFlags.Static))
        {
            object? propValue = p.GetValue(null);
            Debug.Assert(propValue != null);

            yield return new Icon($"{nameof(LucideIcons)}.{category.Name}.{p.Name}", (IconUsage)propValue);
        }
    }

    private partial class Icon : Container, IHasTooltip
    {
        public LocalisableString TooltipText { get; }

        public SpriteIcon SpriteIcon { get; }

        public Icon(string name, IconUsage icon)
        {
            TooltipText = name;
            AutoSizeAxes = Axes.Both;

            Child = SpriteIcon = new SpriteIcon
            {
                Icon = icon,
                Size = new Vector2(60),
            };
        }
    }
}
