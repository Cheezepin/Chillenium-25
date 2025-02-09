using Godot;
using System;

public partial class RotateSlides : Control
{
	// Called when the node enters the scene tree for the first time.
	public int currColor;
	public int state = 0;
	private double timer;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch(state) {
			case 0:
				if(currColor != Global.currColorID) {
					int neg = (((currColor - 1) + 12) % 6);
					int pos = (currColor + 1) % 6;
					while(neg != Global.currColorID && pos != Global.currColorID) {
						neg--;
						pos++;
						neg += 12;
						neg %= 6;
						pos %= 6;
					}
					if(neg == Global.currColorID) {
						state = 1;
					} else {
						state = 2;
					}
					timer = 0;
				}
				break;
			case 1:
			case 2:
				RotationDegrees += 300f*((float)delta)*(state == 1 ? 1f : -1f);
				if(timer >= 0.2) {
					do {
						currColor += (state == 1 ? -1 : 1) + 12;
						currColor %= 6;
					} while ((Global.colorsUnlocked & (1 << currColor)) == 0);
					timer = 0;
					state = 0;
					RotationDegrees = 0;
				}
				timer += delta;
				break;
		}
	}
}
