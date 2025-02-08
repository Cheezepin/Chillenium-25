using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export]public float speed = 2000.0f;
	public Vector2 velocity;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	double timer = 0;
	public override void _Process(double delta)
	{
		Position += velocity * (float)delta;
		if(timer > 5.0) {QueueFree();}
	}

	public void _OnBodyEntered(Node2D body)
	{
		if(body is TileMapLayer) {
			QueueFree();
		}
		if(body is Enemy) {
			((Enemy)body).OnBulletHit(this);
			QueueFree();
		}
	}
}
