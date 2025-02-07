using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	public const float MaxSpeed = 600.0f;
	public const float JumpVelocity = -800.0f;

	public const double xAcceleration = 5500.0f;
	public const float gravity = 2500.0f;

	[Export]public PackedScene bulletScene;

	public List<Ghost> ghosts = new List<Ghost>();

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		float speed = MaxSpeed - (ghosts.Count*200.0f);

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += new Vector2(0, gravity) * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float xDirection = Input.GetAxis("move_left", "move_right");
		if (xDirection != 0)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, xDirection*speed, (float)(xAcceleration*delta));
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xAcceleration*delta));
		}


		if (Input.IsActionJustPressed("shoot"))
		{
			var bullet = bulletScene.Instantiate<Bullet>();
			Global.currentLevel.AddChild(bullet);
			bullet.GlobalPosition = GlobalPosition + new Vector2(0, -80.0f);
			bullet.LookAt(bullet.GetGlobalMousePosition());
			bullet.velocity = new Vector2((float)Math.Cos(bullet.Rotation), (float)Math.Sin(bullet.Rotation)) * bullet.speed;
			velocity -= new Vector2((float)Math.Cos(bullet.Rotation), (float)Math.Sin(bullet.Rotation)) * 500.0f;

			Level currLevel = (Level)Global.currentLevel;
			Camera cam = currLevel.camera;
			cam.ShakeCamera(100.0f, 0.08);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
