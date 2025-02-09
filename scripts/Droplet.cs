using Godot;
using System;

public partial class Droplet : Enemy
{
	// Called when the node enters the scene tree for the first time.
	[Export]public Global.ColorNames color;
	private Random r;
	private double phase;
	private float terminalY = 400.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Ready()
	{
		r = new Random();
		phase = r.NextDouble()*2.0;
		explosion.Modulate = Global.colors[(int)color];
		base._Ready();
	}
	public override void _Process(double delta)
	{
		s.SetShaderParameter("trans", Global.currBW || Global.currColorID != (int)color);
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		if(velocity.Y > terminalY) {velocity.Y = terminalY;}
		player = ((Level)Global.currentLevel).player;
		float targetVel = (timer + phase) % 2.0 < 1.0 ?  -100.0f : 100.0f;
		double dist = Math.Pow(GlobalPosition.X - player.GlobalPosition.X, 2) + Math.Pow(GlobalPosition.Y - player.GlobalPosition.Y, 2);
		if(dist < 200000.0) {
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").SpeedScale = 2.0f;
			targetVel = player.GlobalPosition.X > GlobalPosition.X ? 250.0f : -250.0f;
		} else {
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").SpeedScale = 1.0f;
		}
		velocity.X = Mathf.MoveToward(velocity.X, targetVel, 1000.0f * (float)delta);
		velocity.Y += gravity * (float)delta;

		Move(delta);
	}

	public override void _OnHurtboxAreaEntered(Node2D body) {
		if(timer < 0.2) return;
		if(!Global.currBW && Global.currColorID == (int)color && body is Hitbox) {
			flashTimer = 0.04;
			velocity.X = body.GlobalPosition.X - GlobalPosition.X > 0 ? -1000.0f : 1000.0f;
			TakeDamage(1.0f);
			GlobalScale = new Vector2(0.5f, 1.0f);
		}
	}
}
