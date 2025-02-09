using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class FlyCard : Sprite2D
{
	// Called when the node enters the scene tree for the first time.

	[Export]public int id;
	private double timer = 0;
	private float speed = 1500.0f;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.One;
		switch(id) {
			case 0: velocity = new Vector2(1.0f, 0.0f); break;
			case 1: velocity = new Vector2(-1.0f, 0.0f); break;
			case 2: velocity = new Vector2(0.5f, 0.866025404f); break;
			case 3: velocity = new Vector2(-0.5f, 0.866025404f); break;
			case 4: velocity = new Vector2(0.5f, -0.866025404f); break;
			case 5: velocity = new Vector2(-0.5f, -0.866025404f); break;
		}
		Position += velocity*speed*(float)delta;
		speed -= (float)(delta*200.0);
		timer += delta;
		if(timer > 3.0) {QueueFree();}
	}
}
