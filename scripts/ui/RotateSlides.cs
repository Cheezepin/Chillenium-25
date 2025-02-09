using Godot;
using System;

public partial class RotateSlides : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float target = Global.currColorID*-60;
		if(target == RotationDegrees) {
			if(RotationDegrees < 0) RotationDegrees += 360f;
			if(RotationDegrees >= 360.0f) RotationDegrees -= 360f;
		} else {
			
		}
		// RotationDegrees = Mathf.MoveToward(RotationDegrees, target, 300f*(float)delta);
		RotationDegrees = target;
	}
}
