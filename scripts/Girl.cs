using Godot;
using System;

public partial class Girl : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	float velY = 0;
	public Player player;
	public override void _Ready()
	{
		velY = -100.0f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		velY += 400.0f*(float)delta;
		Vector2 pos = GlobalPosition;
		pos.Y += velY*(float)delta;
		if(pos.Y > 0.0f) {
			pos.Y = 0.0f;
			player.sprite.Play("catch2");
			QueueFree();
		}
		pos.X = player.GlobalPosition.X;
		GlobalPosition = pos;
	}
}
