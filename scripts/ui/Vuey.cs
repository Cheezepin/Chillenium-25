using Godot;
using System;

public partial class Vuey : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	private Vector2 home;
	private double timer = 0;
	public override void _Ready()
	{
		home = GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = new Vector2(0, -50.0f + (float)(25.0*Math.Sin((timer*1.5))));
		timer += delta;
	}
}
