using Godot;
using System;

public partial class DotFade : ColorRect
{
	// Called when the node enters the scene tree for the first time.
	ShaderMaterial s;
	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	double timer = 0;
	public override void _Process(double delta)
	{
		s.SetShaderParameter("radius", (Math.Sin(timer*2)*0.020) + 0.020);
		timer += delta;
	}
}
