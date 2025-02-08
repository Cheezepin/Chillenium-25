using Godot;
using System;

public partial class PostProcessing : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	private ShaderMaterial s;
	private ColorRect rect;
	public override void _Ready()
	{
		s = (ShaderMaterial)GetNode<ColorRect>("ColorRect").Material;
		rect = GetNode<ColorRect>("ColorRect");
		rect.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		rect.Show();
		if(Global.currBW) {s.SetShaderParameter("screen_color_bw", true);}
		else if(Global.allColors) {rect.Hide();}
		else {
			s.SetShaderParameter("screen_color_bw", false);
			s.SetShaderParameter("screen_color", Global.currColor);
			s.SetShaderParameter("progress", (float)Global.colorSlideProgress);
		}
	}
}
