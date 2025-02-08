using Godot;
using System;

public partial class ColoredPlatforms : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	ShaderMaterial s;

	[Export]public Global.ColorNames targetColor;

	Vector2 home;
	double timer = 0;

	public Vector2 velocity;
	
	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
		s.SetShaderParameter("target_color", Global.colors[(int)targetColor]);
		home = GlobalPosition;
		velocity = Vector2.Zero;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Global.allColors) {
			s.SetShaderParameter("current_color", Global.colors[(int)targetColor]);
			CollisionEnabled = true;
		} else if(Global.currBW) {
			s.SetShaderParameter("current_color", Color.Color8(0,0,0,0));
			CollisionEnabled = false;
		} else {
			s.SetShaderParameter("current_color", Global.currColor);
			CollisionEnabled = Global.currColor == Global.colors[(int)targetColor];
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		switch(targetColor) {
			case Global.ColorNames.Red:
				GlobalPosition = new Vector2(home.X, 10f*(float)Math.Sin(timer*20.0));
				break;
			case Global.ColorNames.Orange:
				float yVel = (timer % 2.0) < 1.0 ? 200.0f : -200.0f;
				velocity = new Vector2(0, yVel)*(float)delta;
				GlobalPosition += velocity;
				break;
			case Global.ColorNames.Yellow:
				float xVel = (timer % 2.0) < 1.0 ? 200.0f : -200.0f;
				velocity = new Vector2(xVel, 0)*(float)delta;
				GlobalPosition += velocity;
				break;
			case Global.ColorNames.Green:
				GlobalRotation += (float)delta;
				break;
			case Global.ColorNames.Blue:
				GlobalPosition = new Vector2(home.X, 10f*(float)Math.Sin(timer*20.0));
				break;
			case Global.ColorNames.Purple:
				GlobalPosition = new Vector2(home.X + 10f*(float)Math.Sin(timer*20.0), home.Y);
				break;
			default:
				break;
		}
		timer += delta;
		base._PhysicsProcess(delta);
	}
}
