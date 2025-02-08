using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	public const float MaxSpeed = 600.0f;
	public const float JumpVelocity = -1000.0f;
	public const float EnemyBounceVelocity = -600.0f;

	public const double xAcceleration = 5500.0f;
	public const double xFriction = 3500.0f;
	public const float gravity = 2500.0f;

	ShaderMaterial s;
	private double flashTimer = 0;

	[Export]public PackedScene bulletScene;

	private CollisionShape2D hitbox;

	public double health = 3;

	private enum Action {
		Move,
		Attack,
		Stunned,
	};
	
	private Action action;
	private double actionTimer = 0;

	private ColoredPlatforms currPlatform = null;
	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
		Velocity = Vector2.Zero;
		hitbox = GetNode<Hitbox>("Hitbox").GetNode<CollisionShape2D>("CollisionShape2D");;
		hitbox.Disabled = true;
		action = Action.Move;
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if(flashTimer > 0) {
			s.SetShaderParameter("flash", true);
			flashTimer -= delta;
			if(flashTimer < 0) {flashTimer = 0; s.SetShaderParameter("flash", false);}
		}
		base._Process(delta);
	}

	private void ChangeAction(Action to) {
		action = to;
		actionTimer = 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		float speed = MaxSpeed;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += new Vector2(0, gravity) * (float)delta;
		}

		switch(action) {
			case Action.Move:
				if (Input.IsActionJustPressed("jump") && IsOnFloor())
				{
					velocity.Y = JumpVelocity;
				}

				float xDirection = Input.GetAxis("move_left", "move_right");
				if (xDirection != 0)
				{
					velocity.X = Mathf.MoveToward(Velocity.X, xDirection*speed, (float)(xAcceleration*delta));
					GD.Print(GlobalScale);
					if(xDirection > 0) {
						if(GlobalScale != new Vector2(1.0f, 1.0f)) {
							GlobalScale = new Vector2(1.0f, -1.0f);
							RotationDegrees = 0;
						}
					} else {
						if(GlobalScale != new Vector2(1.0f, -1.0f)) {
							GlobalScale = new Vector2(1.0f, -1.0f);
							RotationDegrees = 180;
						}
					}
				}
				else
				{
					velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xFriction*delta));
				}

				if (Input.IsActionJustPressed("attack"))
				{
					ChangeAction(Action.Attack);
				}
				break;
			case Action.Attack:
				velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xFriction*delta));
				hitbox.Disabled = false;
				if(actionTimer > 0.2) {
					ChangeAction(Action.Move);
					hitbox.Disabled = true;
				}
				break;
			case Action.Stunned:
				velocity.X = Mathf.MoveToward(velocity.X, 0, (float)(xFriction*delta));
				if(actionTimer > 0.2) ChangeAction(Action.Move);
				break;
		}

		if(currPlatform != null) {
			if(currPlatform.targetColor == Global.ColorNames.Purple) {
				velocity.X =currPlatform.velocity.Length() * GlobalScale.Y * MaxSpeed * 2;
			} else {
				GlobalPosition += currPlatform.velocity;
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		actionTimer += delta;
	}

	private void TakeDamage(float dmg) {
		health -= dmg;
		if(health <= 0) {
			// QueueFree();
		}
	}

	void _OnFootboxAreaEntered(Node2D body) {
		if(body is Jumpbox) {
			GD.Print("Yeah!");
			Enemy e = ((Jumpbox)body).parent;
			e.OnJumpedOn();
			Velocity = new Vector2(Velocity.X, EnemyBounceVelocity);
		}
	}

	void _OnFootboxBodyEntered(Node2D body) {
		if(body is ColoredPlatforms) {
			ColoredPlatforms plat = (ColoredPlatforms)body;
			currPlatform = plat;
			if(plat.targetColor == Global.ColorNames.Red) Velocity = new Vector2(Velocity.X, EnemyBounceVelocity*3);
			if(plat.targetColor == Global.ColorNames.Blue) Velocity = new Vector2(Velocity.X, EnemyBounceVelocity*8);
		}
	}

	void _OnFootboxBodyExited(Node2D body) {
		if(body == currPlatform) {
			currPlatform = null;
		}
	}

	public void _OnHurtboxAreaEntered(Node2D body) {
		if(action == Action.Stunned) return;
		if(body.GetParent() != this && body is Hitbox) {
			flashTimer = 0.04;
			float velX = body.GlobalPosition.X - GlobalPosition.X > 0 ? -1000.0f : 1000.0f;
			Velocity = new Vector2(velX, Velocity.Y);
			TakeDamage(1.0f);
			ChangeAction(Action.Stunned);
		}
	}
}
