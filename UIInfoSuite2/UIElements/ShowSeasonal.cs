using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using UIInfoSuite.Infrastructure;
using UIInfoSuite.Infrastructure.Extensions;

namespace UIInfoSuite.UIElements
{
    class ShowSeasonal : IDisposable
    {
        private readonly PerScreen<bool> _drawSeasonalIcon = new PerScreen<bool>();
        private Point iconPosition;
        //private bool _drawSeasonalIcon = false;
        private string _seasonalText;
        private readonly PerScreen<ClickableTextureComponent> _seasonalIcon = new PerScreen<ClickableTextureComponent>();
        private readonly IModHelper _helper;

        public void ToggleOption(bool showSeasonalIcon)
        {
            _helper.Events.Display.RenderingHud -= OnRenderingHud;
            _helper.Events.Display.RenderedHud -= OnRenderedHud;
            _helper.Events.GameLoop.DayStarted -= OnDayStarted;
            _helper.Events.GameLoop.SaveLoaded -= OnSaveLoaded;

            if (showSeasonalIcon)
            {
                CheckForSeasonal();
                _helper.Events.GameLoop.DayStarted += OnDayStarted;
                _helper.Events.Display.RenderingHud += OnRenderingHud;
                _helper.Events.Display.RenderedHud += OnRenderedHud;
                _helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            }
        }

        public ShowSeasonal(IModHelper helper)
        {
            _helper = helper;
        }

        /// <summary>Raised before drawing the HUD (item toolbar, clock, etc) to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {
            // draw icon
            if (!Game1.eventUp)
            {
                if (_seasonalText != null)
                {
                    iconPosition = IconHandler.Handler.GetNewIconPosition();
                    _seasonalIcon.Value.draw(Game1.spriteBatch);
                }
            }
        }

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            // draw hover text
            if (_seasonalText != null &&
                _seasonalIcon.Value.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
            {
                IClickableMenu.drawHoverText(
                    Game1.spriteBatch,
                    _seasonalText,
                    Game1.dialogueFont);
            }
        }

        public void Dispose()
        {
            ToggleOption(false);
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            CheckForSeasonal();
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            CheckForSeasonal();
        }

        private void CheckForSeasonal()
        {
            if (Game1.currentSeason.Equals("spring") && Game1.dayOfMonth >= 15 && Game1.dayOfMonth <= 18)
            {
                _seasonalIcon.Value = new ClickableTextureComponent(
                        new Rectangle(iconPosition.X, iconPosition.Y, 40, 40),
                        Game1.objectSpriteSheet,
                        new Rectangle(128, 192, 16, 16),
                        2.7f);
                _seasonalText = "Salmonberry season";
            }

            else if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth >= 12 && Game1.dayOfMonth <= 14)
            {
                _seasonalIcon.Value = new ClickableTextureComponent(
                        new Rectangle(iconPosition.X, iconPosition.Y, 40, 40),
                        Game1.objectSpriteSheet,
                        new Rectangle(160, 256, 16, 16),
                        2.7f);
                _seasonalText = "Beach forageables";
            }
                
            else if (Game1.currentSeason.Equals("fall") && Game1.dayOfMonth >= 8 && Game1.dayOfMonth <= 11)
            {
                _seasonalIcon.Value = new ClickableTextureComponent(
                        new Rectangle(iconPosition.X, iconPosition.Y, 40, 40),
                        Game1.objectSpriteSheet,
                        new Rectangle(32, 272, 16, 16),
                        2.7f);
                _seasonalText = "Blackberry season";
            }
                
            else if (Game1.currentSeason.Equals("winter") && Game1.dayOfMonth >= 15 && Game1.dayOfMonth <= 17)
            {
                _seasonalIcon.Value = new ClickableTextureComponent(
                           new Rectangle(iconPosition.X, iconPosition.Y, 40, 40),
                           Game1.objectSpriteSheet,
                           new Rectangle(128, 528, 16, 16), // fixme
                           2.7f);
                _seasonalText = "Night Market";
            }
                
            else _seasonalText = null;
        }
    }
}
