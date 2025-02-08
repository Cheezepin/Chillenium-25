using Godot;
using System;

public partial class ColoredPlatforms : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	ShaderMaterial s;
	[Export]public Color targetColor;
	
	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
		s.SetShaderParameter("target_color", targetColor);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		s.SetShaderParameter("current_color", Global.currColor);
		CollisionEnabled = Global.currColor == targetColor;
	}
}
