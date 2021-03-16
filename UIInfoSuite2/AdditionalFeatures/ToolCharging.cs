
using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace UIInfoSuite.AdditionalFeatures
{
    public class ToolCharging
    {
        private static int extraRemove = 30;
        //public ToolChargingConfig config;

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public static void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            int hold = Game1.player.toolHold;
            if (!Game1.player.canReleaseTool || hold <= 0)
                return; //either maxed or not being held

            if (hold - extraRemove <= 0)
                Game1.player.toolHold = 1;
            else
                Game1.player.toolHold -= extraRemove;
        }
    }
}
