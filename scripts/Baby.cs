using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Baby : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.

	private bool hit = false;
	private float velY = 0;
	private double flashTimer = 0;
	private ShaderMaterial s;
	private Player player;
	[Export]public PackedScene girl;

	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Player player = ((Level)Global.currentLevel).player;
		if(!hit) {
			if(player.GlobalPosition.X > 850f) {
				Play("eat");
			}
		} else {
			velY += 800.0f*(float)delta;
			Position += new Vector2(0, velY)*(float)delta;
		}

		if(flashTimer > 0) {
			s.SetShaderParameter("flash", true);
			flashTimer -= delta;
			if(flashTimer < 0) {flashTimer = 0; s.SetShaderParameter("flash", false);}
		}
	}

	public void _on_hurtbox_area_entered(Node2D body) {
		if(!hit && body is Hitbox) {
			hit = true;
			velY = -320.0f;
			flashTimer = 0.04;
			body.GetParent<Player>().killedBaby = true;
			Girl girlNode = girl.Instantiate<Girl>();
			girlNode.GlobalPosition += new Vector2(-150.0f, -150.0f);
		}
	}
}
