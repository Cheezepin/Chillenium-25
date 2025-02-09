using Godot;
using System;

public partial class UICard : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	[Export]public int col;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if((Global.colorsUnlocked & (1 << col)) != 0) Show(); else Hide();
		if(Global.allColors) Show();
	}
}
