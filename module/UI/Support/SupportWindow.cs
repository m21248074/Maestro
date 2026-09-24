using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Services;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Support
{
    public class SupportWindow : StandardWindow
    {
        private static class Layout
        {
            public const int WindowWidth = 420;
            public const int WindowHeight = 326;
            public const int ContentWidth = 390;
            public const int IntroHeight = 84;
            public const int MethodHeight = 68;
            public const int ActionButtonWidth = 112;
        }

        private sealed class SupportFooter : Control
        {
            private const string Message = "每一首被演奏、分享與喜愛的歌曲，對我們都意義重大。";

            protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
            {
                var font = GameService.Content.DefaultFont12;
                var textWidth = (int)Math.Ceiling(font.MeasureString(Message).Width);
                const int heartSize = 14;
                const int gap = 5;
                var totalWidth = textWidth + gap + heartSize;
                var startX = Math.Max(0, (_size.X - totalWidth) / 2);

                spriteBatch.DrawStringOnCtrl(
                    this,
                    Message,
                    font,
                    new Rectangle(startX, 0, textWidth, _size.Y),
                    MaestroTheme.MutedCream);
                spriteBatch.DrawOnCtrl(
                    this,
                    MaestroIcons.Support,
                    new Rectangle(startX + textWidth + gap, (_size.Y - heartSize) / 2, heartSize, heartSize),
                    MaestroTheme.SupportPink);
            }
        }

        private static readonly Logger Logger = Logger.GetLogger<SupportWindow>();
        private static Texture2D _backgroundTexture;

        private readonly StandardButton _copyButton;

        public SupportWindow()
            : base(
                GetBackground(),
                new Rectangle(0, 0, Layout.WindowWidth, Layout.WindowHeight),
                new Rectangle(15, MaestroTheme.WindowContentTopPadding, Layout.ContentWidth, Layout.WindowHeight))
        {
            Title = "支持";
            Subtitle = "Maestro";
            Emblem = Module.Instance.ContentsManager.GetTexture("support-emblem.png");
            SavesPosition = true;
            Id = "SupportWindow_v1";
            CanResize = false;
            Parent = GameService.Graphics.SpriteScreen;

            var currentY = MaestroTheme.PaddingContentTop;

            new Label
            {
                Parent = this,
                Location = new Point(0, currentY),
                Size = new Point(Layout.ContentWidth, 28),
                Font = GameService.Content.DefaultFont16,
                Text = "感謝您使用 Maestro!",
                TextColor = MaestroTheme.CreamWhite,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new Label
            {
                Parent = this,
                Location = new Point(12, currentY + 32),
                Size = new Point(Layout.ContentWidth - 24, 48),
                Font = GameService.Content.DefaultFont14,
                Text = "您的支持將協助資助未來的更新與社群功能。",
                TextColor = MaestroTheme.MutedCream,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            currentY += Layout.IntroHeight + 10;

            CreateMethodLabels(
                currentY,
                "在 Ko-fi 上支持我們",
                "支持未來的更新與社群功能。");

            var koFiButton = new StandardButton
            {
                Parent = this,
                Location = new Point(Layout.ContentWidth - Layout.ActionButtonWidth, currentY + 21),
                Size = new Point(Layout.ActionButtonWidth, MaestroTheme.ActionButtonHeight),
                Text = "開啟 Ko-fi",
                BasicTooltipText = "在瀏覽器中開啟 ko-fi.com/aex"
            };
            koFiButton.Click += OnKoFiClicked;
            currentY += Layout.MethodHeight;

            new Panel
            {
                Parent = this,
                Location = new Point(0, currentY),
                Size = new Point(Layout.ContentWidth, 1),
                BackgroundColor = MaestroTheme.SubtleBorder
            };
            currentY += 10;

            CreateMethodLabels(
                currentY,
                "贈送遊戲內金幣",
                $"激戰 2 帳號: {SupportLinks.GameAccountName}");

            _copyButton = new StandardButton
            {
                Parent = this,
                Location = new Point(Layout.ContentWidth - Layout.ActionButtonWidth, currentY + 21),
                Size = new Point(Layout.ActionButtonWidth, MaestroTheme.ActionButtonHeight),
                Text = "複製帳號",
                BasicTooltipText = $"複製 {SupportLinks.GameAccountName}"
            };
            _copyButton.Click += OnCopyClicked;
            currentY += Layout.MethodHeight + 10;

            new SupportFooter
            {
                Parent = this,
                Location = new Point(0, currentY),
                Size = new Point(Layout.ContentWidth, 34)
            };
        }

        private static Texture2D GetBackground()
        {
            return _backgroundTexture ?? (_backgroundTexture =
                MaestroTheme.CreateWindowBackground(Layout.WindowWidth, Layout.WindowHeight));
        }

        private void CreateMethodLabels(int y, string title, string description)
        {
            new Label
            {
                Parent = this,
                Location = new Point(8, y + 7),
                Size = new Point(Layout.ContentWidth - Layout.ActionButtonWidth - 24, 26),
                Font = GameService.Content.DefaultFont16,
                Text = title,
                TextColor = MaestroTheme.CreamWhite
            };

            new Label
            {
                Parent = this,
                Location = new Point(8, y + 35),
                Size = new Point(Layout.ContentWidth - Layout.ActionButtonWidth - 24, 28),
                Font = GameService.Content.DefaultFont12,
                Text = description,
                TextColor = MaestroTheme.MutedCream
            };
        }

        private void OnKoFiClicked(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(SupportLinks.KoFiUrl)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to open Ko-fi support link");
                ScreenNotification.ShowNotification(
                    "無法開啟 Ko-fi，請在瀏覽器中前往 ko-fi.com/aex。",
                    ScreenNotification.NotificationType.Error);
            }
        }

        private async void OnCopyClicked(object sender, MouseEventArgs e)
        {
            _copyButton.Enabled = false;
            _copyButton.Text = "複製中...";

            try
            {
                var copied = await ClipboardUtil.WindowsClipboardService.SetTextAsync(SupportLinks.GameAccountName);
                if (!copied)
                {
                    ShowCopyFailure();
                    return;
                }

                _copyButton.Text = "已複製";
                ScreenNotification.ShowNotification(
                    $"已將 {SupportLinks.GameAccountName} 複製到剪貼簿。",
                    ScreenNotification.NotificationType.Info);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to copy the support account name");
                ShowCopyFailure();
            }
            finally
            {
                _copyButton.Enabled = true;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            _copyButton.Text = "複製帳號";
            base.OnShown(e);
        }

        private void ShowCopyFailure()
        {
            _copyButton.Text = "複製帳號";
            ScreenNotification.ShowNotification(
                $"無法複製帳號名稱，請手動輸入 {SupportLinks.GameAccountName}。",
                ScreenNotification.NotificationType.Error);
        }
    }
}
