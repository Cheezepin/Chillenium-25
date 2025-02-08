using Godot;
using System;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public static GameUI ui;

	public Camera camera;
	public Player player;
	public override void _Ready()
	{
		ui = (GameUI)GD.Load<PackedScene>("ui/GameUI.tscn").Instantiate();
		AddChild(ui);
		camera = GetNode<Camera>("Camera"); //should we just spawn these instead of looking for them?
		player = GetNode<Player>("Player");
		Global.switchSound = GetNode<AudioStreamPlayer>("Switch");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
