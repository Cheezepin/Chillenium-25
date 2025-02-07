using Godot;
using System;

public partial class Ghost : Area2D
{
	// Called when the node enters the scene tree for the first time.
	private Player player;
	private Random r;
	private double offsetTimer;
	private Vector2 offset;
	public override void _Ready()
	{
		player = ((Level)Global.currentLevel).player;
		player.ghosts.Add(this);
		r = new Random();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(offsetTimer <= 0) {
			offsetTimer = 0.2;
			offset = new Vector2(r.NextSingle() - 0.5f, r.NextSingle() - 0.5f)*150.0f;
		}
		offsetTimer -= delta;
		Vector2 pos = GlobalPosition;
		Global.AsymptoticApproach(ref pos, player.GlobalPosition + new Vector2(0, -80.0f) + offset, 2.5f*(float)delta);
		GlobalPosition = pos;
	}
}
