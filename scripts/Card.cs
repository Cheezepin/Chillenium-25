using Godot;
using System;

public partial class Card : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]public Global.ColorFilters color;
	private Player player;
	public override void _Ready()
	{
		player = ((Level)Global.currentLevel).player;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		player = ((Level)Global.currentLevel).player;
		double dist = Math.Pow(GlobalPosition.X - player.GlobalPosition.X, 2) + Math.Pow(GlobalPosition.Y - player.GlobalPosition.Y, 2);
		if(dist < 10000.0f) {
			Global.colorsUnlocked |= (int)color;
			switch(color) {
				case Global.ColorFilters.Red: Global.targetColorID = (int)Global.ColorNames.Red; break;
				case Global.ColorFilters.Orange: Global.targetColorID = (int)Global.ColorNames.Orange; break;
				case Global.ColorFilters.Yellow: Global.targetColorID = (int)Global.ColorNames.Yellow; break;
				case Global.ColorFilters.Green: Global.targetColorID = (int)Global.ColorNames.Green; break;
				case Global.ColorFilters.Blue: Global.targetColorID = (int)Global.ColorNames.Blue; break;
				case Global.ColorFilters.Purple: Global.targetColorID = (int)Global.ColorNames.Purple; break;
			}
			Global.transitioningColor = true;
			Global.transitionLeft = false;
			GD.Print((int)color);
			QueueFree();
		}
	}
}
