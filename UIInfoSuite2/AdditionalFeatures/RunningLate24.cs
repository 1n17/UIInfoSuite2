using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace UIInfoSuite.AdditionalFeatures
{
	public class RunningLate24
	{
		private static int InternalTime { get; set; }

		private static int LastGameTime { get; set; }

		public static void Display_RenderedHud(object sender, RenderedHudEventArgs e)
		{
			bool flag = Game1.displayHUD && !Game1.eventUp && Game1.currentBillboard == 0 && Game1.gameMode == 3 && !Game1.freezeControls && !Game1.panMode && !Game1.HostPaused;
			if (flag)
			{
				int time = Game1.timeOfDay;
				bool flag2 = Context.IsMultiplayer && !Context.IsMainPlayer;
				if (flag2)
				{
					bool flag3 = time != LastGameTime;
					if (flag3)
					{
						LastGameTime = time;
						InternalTime = 0;
					}
					else
					{
						InternalTime += Game1.currentGameTime.ElapsedGameTime.Milliseconds;
					}
				}
				else
				{
					InternalTime = Game1.gameTimeInterval;
				}
				int actualTime = time + InternalTime / (700 + ((Game1.MasterPlayer.currentLocation != null) ? (Game1.MasterPlayer.currentLocation.getExtraMillisecondsPerInGameMinuteForThisLocation() / 10) : 0));
				Rectangle sourceRect = new Rectangle(333, 431, 71, 43);
				SpriteBatch b = e.SpriteBatch;
				Vector2 offset = new Vector2(108f, 112f);
				Rectangle bounds = new Rectangle(360, 459, 40, 9);
				Vector2 moneyBoxPos = Game1.dayTimeMoneyBox.position;
				b.Draw(Game1.mouseCursors, moneyBoxPos + offset, new Rectangle?(bounds), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
				string hours;
				hours = string.Concat(time / 100 % 24);
				hours = ((time / 100 % 24 <= 9) ? ("0" + hours) : hours);
				string onesPad = (actualTime % 100 == 0) ? "0" : "";
				string tensPad = (actualTime % 100 < 10 && actualTime % 100 > 0) ? "0" : "";
				int minutes = actualTime % 100;
				string timeText = string.Concat(new object[]
				{
					hours,
					":",
					tensPad,
					(minutes > 59) ? 59 : minutes,
					onesPad
				});
				bool t24plus = time >= 2400;

				Vector2 txtSize = Game1.dialogueFont.MeasureString(timeText);
				int timeShake = Game1.dayTimeMoneyBox.timeShakeTimer;
				Vector2 timePosition = new Vector2((float)sourceRect.X * 0.55f - txtSize.X / 2f + (float)((timeShake > 0) ? Game1.random.Next(-2, 3) : 0), (float)sourceRect.Y * 0.31f - txtSize.Y / 2f + (float)((timeShake > 0) ? Game1.random.Next(-2, 3) : 0));
				float fade = (Game1.shouldTimePass() || Game1.fadeToBlack || Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 2000.0 > 1000.0) ? 1f : 0.5f;
				Utility.drawTextWithShadow(b, timeText, Game1.dialogueFont, moneyBoxPos + timePosition, t24plus ? Color.Red : (Game1.textColor * fade), 1f, -1f, -1, -1, 1f, 3);
			}
		}
	}
}

