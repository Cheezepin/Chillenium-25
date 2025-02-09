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

		Global.allColors = true;
		Global.currBW = false;
		Global.colorsUnlocked = 0;
		Global.currColorID = 0;
		Global.switchSound = GetNode<AudioStreamPlayer>("Switch");

		//DEBUG REMOVE LATER
		// Global.allColors = false;
		// Global.colorsUnlocked |= (int)Global.ColorFilters.Red + (int)Global.ColorFilters.Orange + (int)Global.ColorFilters.Yellow + (int)Global.ColorFilters.Green + (int)Global.ColorFilters.Blue;
		// Global.currColorID = 0;
		// Global.colorsUnlocked = 1 + 2 + 4 + 8 + 16 + 32;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
