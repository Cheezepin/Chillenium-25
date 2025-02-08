using Godot;
using System;

public partial class PostProcessing : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	private ShaderMaterial s;
	public override void _Ready()
	{
		s = (ShaderMaterial)GetNode<ColorRect>("ColorRect").Material;
		GetNode<ColorRect>("ColorRect").Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		s.SetShaderParameter("screen_color", Global.currColor);
	}
}
