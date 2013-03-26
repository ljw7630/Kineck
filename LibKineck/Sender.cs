using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

using LibKineck;

namespace LibKineck
{
	public class Sender
	{
		private HeadRotation _previousHeadState;
		private static readonly long _delay = 500;

		public HeadRotation PreviousHeadState
		{
			get { return _previousHeadState; }
		}

		private long _elapsedTicks;

		public Sender()
		{
			_previousHeadState = HeadRotation.NONE;
			_elapsedTicks = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}

		private bool IsDelayExpired()
		{
			long current = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;
			if (current - _elapsedTicks > _delay)
			{
				_elapsedTicks = current;
				return true;
			}
			return false;
		}

		public void AnalyzeAndSend(string command)
		{
			switch (command)
			{
				case "UP":
					VolumeControl.IncreaseVolume();
					break;
				case "DOWN":
					VolumeControl.DecreaseVolume();
					break;
				case "MUTE":
					VolumeControl.MuteVolume();
					break;
				case "LEFT":
					KeyControl.SendKey(KeyControl.LEFT_ARROW);
					break;
				case "RIGHT":
					KeyControl.SendKey(KeyControl.RIGHT_ARROW);
					break;
			}
		}

		public void AnalyzeAndSend(ref Skeleton skeleon)
		{
			HeadRotation rotation = AngleCalculator.GetHeadRotation(ref skeleon);

			if (IsDelayExpired())
			{
				if (_previousHeadState != rotation)
				{
					SendCommand(rotation);
					_previousHeadState = rotation;
				}
			}
		}

		public void SendCommand(HeadRotation rotation)
		{
			if (rotation == HeadRotation.LEFT)
			{
				KeyControl.SendKey(KeyControl.LEFT_ARROW);
			}
			else if (rotation == HeadRotation.RIGHT)
			{
				KeyControl.SendKey(KeyControl.RIGHT_ARROW);
			}
		}
	}
}
