using Godot;
using System;

public partial class Hurtbox : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public Node2D parent;
	public override void _Ready()
	{
		parent = (Node2D)GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
