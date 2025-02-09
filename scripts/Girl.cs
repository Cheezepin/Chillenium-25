using Godot;
using System;

public partial class Girl : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 pos = GlobalPosition;
		pos.Y += 80.0f*(float)delta;
		if(pos.Y > 70.0f) {
			pos.Y = 70.0f;
		}
		GlobalPosition = pos;
	}
}
