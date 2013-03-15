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
		public HeadRotation previousHeadState;

		public Sender()
		{
			previousHeadState = HeadRotation.NONE;
		}

		public void AnalyzeAndSend(ref Skeleton skeleon)
		{
			HeadRotation rotation = AngleCalculator.GetHeadRotation(ref skeleon);
			if (previousHeadState != rotation)
			{
				SendCommand(rotation);
			}
			previousHeadState = rotation;
			
		}

		static void SendCommand(HeadRotation rotation)
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
