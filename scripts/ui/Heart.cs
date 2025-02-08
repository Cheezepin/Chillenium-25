using Godot;
using System;

public partial class Heart : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	[Export]public Texture2D[] texs;
	[Export]public int id;
	public override void _Ready()
	{
		Texture = texs[0];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Texture = texs[Global.heartState >= id ? 0 : 1];
	}
}
