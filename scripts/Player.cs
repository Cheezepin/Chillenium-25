using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	public const float MaxSpeed = 600.0f;
	public const float JumpVelocity = -1000.0f;
	public const float EnemyBounceVelocity = -600.0f;
	public const float RedBounceVelocity = -1000.0f;
	public const float BlueBounceVelocity = -1900.0f;

	public const double xAcceleration = 5500.0f;
	public const double xFriction = 3500.0f;
	public const float gravity = 2500.0f;

	private Checkpoint checkpoint = null;

	ShaderMaterial s;
	private double flashTimer = 0;
	private double hitHeadTimer = 0;

	[Export] public AudioStreamPlayer footstepSound;
	[Export] public AudioStreamPlayer jumpSound;
	[Export] public AudioStreamPlayer hitSound;
	[Export] public AudioStreamPlayer slideSound;
	[Export] public AudioStreamPlayer attackSound;
	[Export] public AudioStreamPlayer splatSound;
	[Export] public AudioStreamPlayer springSound;

	private CollisionShape2D hitbox;

	public double health = 3;

	private enum Action {
		Move,
		Attack,
		Stunned,
		Faceplant,
		Die,
		Intro,
		End,
		SorryNothing
	};
	
	private Action action;
	private double actionTimer = 0;

	private ColoredPlatforms currPlatform = null;

	public AnimatedSprite2D sprite;
	private bool hasPurpleSpeed = false;

	[Export]public PackedScene flyCardSpawner;
	private bool hasSeenIntro = false;

	public bool killedBaby = false;
	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
		Velocity = Vector2.Zero;
		hitbox = GetNode<Hitbox>("Hitbox").GetNode<CollisionShape2D>("CollisionShape2D");;
		hitbox.Disabled = true;
		action = Action.Move;
		sprite = GetNode<AnimatedSprite2D>("Vuey");
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if(flashTimer > 0) {
			s.SetShaderParameter("flash", true);
			flashTimer -= delta;
			if(flashTimer < 0) {flashTimer = 0; s.SetShaderParameter("flash", false);}
		}
		if(hitHeadTimer > 0) {
			Global.headState = 1;
			hitHeadTimer -= delta;
			if(hitHeadTimer < 0) hitHeadTimer = 0;
		} else {
			switch(health) {
				case 0:
					Global.headState = 3;
					break;
				case 1:
					Global.headState = 2;
					break;
				default:
					Global.headState = 0;
					break;
			}
		}

		Global.heartState = (int)health;

		if(!hasSeenIntro && GlobalPosition.X > -500f && Global.currentLevel.Name == "SkyLevel") {
			ChangeAction(Action.Intro);
			hasSeenIntro = true;
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

		if(health <= 0 && action != Action.Die) {
			ChangeAction(Action.Die);
		}

		switch(action) {
			case Action.Move:
				if (Input.IsActionJustPressed("jump") && IsOnFloor())
				{
					velocity.Y = JumpVelocity;
					jumpSound.Play();
				}

				float xDirection = Input.GetAxis("move_left", "move_right");
				if(IsOnFloor() && currPlatform != null && currPlatform.targetColor == Global.ColorNames.Purple)
				{
					sprite.Play("run");
					velocity.X = Mathf.MoveToward(Velocity.X, GlobalScale.Y*MaxSpeed*2.5f, (float)(xAcceleration*1.0*delta));
					slideSound.Play();
					hasPurpleSpeed = true;
				}
				else if (xDirection != 0)
				{
					if(IsOnFloor()) {sprite.Play("run"); hasPurpleSpeed = false;}

					if (IsOnFloor() && !footstepSound.Playing)
					{
						footstepSound.Play();
					}
					else
					{
						if(!IsOnFloor()) footstepSound.Stop();
					}

					if(!hasPurpleSpeed) {
						velocity.X = Mathf.MoveToward(Velocity.X, xDirection*speed, (float)(xAcceleration*delta));
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
				}
				else
				{
					footstepSound.Stop();
					if(IsOnFloor()) {sprite.Play("idle"); hasPurpleSpeed = false;}
					if(!hasPurpleSpeed) velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xFriction*delta));
				}

				if(!IsOnFloor() && sprite.Animation != "jump") sprite.Play("jump");

				if (!hasPurpleSpeed && Input.IsActionJustPressed("attack"))
				{
					ChangeAction(Action.Attack);
				}

				break;
			case Action.Attack:
				velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xFriction*delta));
				hitbox.Disabled = false;
				if (sprite.Animation != "attack")
				{
					sprite.Play("attack");
					attackSound.Play();
				}
				if(actionTimer > 0.4) {
					if(killedBaby) {
						ChangeAction(Action.End);
						sprite.Play("catch");
					} else {
						ChangeAction(Action.Move);
					}
					hitbox.Disabled = true;
				}
				break;
			case Action.Stunned:
				velocity.X = Mathf.MoveToward(velocity.X, 0, (float)(xFriction*delta));
				if(actionTimer > 0.2) ChangeAction(Action.Move);
				break;
			case Action.Faceplant:
				velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xFriction*delta));
				if (sprite.Animation != "faceplant")
				{
					sprite.Play("faceplant");
					splatSound.Play();
				}
				if(actionTimer > 3.0) {ChangeAction(Action.Move);}
				break;
			case Action.Die:
				velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(xFriction*delta));
				if (sprite.Animation != "die")
				{
					sprite.Play("die");
					splatSound.Play();
				}
				if(actionTimer > 2.0) {ReturnToCheckpoint(); ChangeAction(Action.Move);}
				break;
			case Action.Intro:
				velocity.X = Mathf.MoveToward(velocity.X, 0, (float)(xFriction*delta));
				sprite.Play("idle");
				if(actionTimer > 1.5) ChangeAction(Action.Move);
				break;
			case Action.End:
				velocity.X = Mathf.MoveToward(velocity.X, 0, (float)(xFriction*delta));
				if(actionTimer > 3.0 && !Global.endScreenTriggered) {
					splatSound.Play();
					Global.endScreenTriggered = true;
				}
				if(actionTimer > 8.0)
                {
					Global.transitions.ExitToMainMenu();
					action = Action.SorryNothing;
                }
				break;
		}

		if(currPlatform != null) {
			GlobalPosition += currPlatform.velocity;
		}

		// float dLR = Input.GetAxis("debug_left", "debug_right");
		// float dUD = Input.GetAxis("debug_up", "debug_down");
		// if(dLR != 0 || dUD != 0) {
			// velocity = new Vector2(dLR, dUD)*150000.0f*(float)delta;
		// }

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

	private void ReturnToCheckpoint() {
		health = 3;
		GlobalPosition = checkpoint.GlobalPosition;
	}

	void _OnFootboxAreaEntered(Node2D body) {
		if(body is Jumpbox) {
			Enemy e = (Enemy)((Jumpbox)body).parent;
			if(((Jumpbox)body).parent is Droplet) {
				if((int)((Droplet)e).color != Global.currColorID) return;
			}
			e.OnJumpedOn();
			Velocity = new Vector2(Velocity.X, EnemyBounceVelocity);
			sprite.Play("jump");
		}
	}

	void _OnFootboxBodyEntered(Node2D body) {
		if(body is ColoredPlatforms) {
			ColoredPlatforms plat = (ColoredPlatforms)body;
			currPlatform = plat;
			if (plat.targetColor == Global.ColorNames.Red)
			{
				Velocity = new Vector2(Velocity.X, RedBounceVelocity);
				springSound.Play();
				hasPurpleSpeed = false;
			}
			if (plat.targetColor == Global.ColorNames.Blue)
			{
				Velocity = new Vector2(Velocity.X, BlueBounceVelocity);
				springSound.Play();
				hasPurpleSpeed = false;
			}
		}
	}

	void _OnFootboxBodyExited(Node2D body) {
		if(body == currPlatform) {
			currPlatform = null;
		}
	}

	public void _OnHurtboxAreaEntered(Node2D body) {
		if(body is Checkpoint) {
			checkpoint = (Checkpoint)body;
			GD.Print("Checkpoint reached!");
		}
		if(action >= Action.Stunned) return;
		if(body.GetParent() != this && body is Hitbox) {
			flashTimer = 0.04;
			hitHeadTimer = 0.5;
			float velX = body.GlobalPosition.X - GlobalPosition.X > 0 ? -1000.0f : 1000.0f;
			Velocity = new Vector2(velX, Velocity.Y);
			((Enemy)(body.GetParent())).cooldown = 1.5;
			TakeDamage(1.0f);
			ChangeAction(Action.Stunned);
			hitSound.Play();
		}

		if(body is DisperseTrigger) {
			Global.allColors = false;
			Global.currBW = true;
			Global.colorsUnlocked = 0;
			body.QueueFree();
			ChangeAction(Action.Faceplant);
			Node2D spawner = (Node2D)flyCardSpawner.Instantiate();
			Global.currentLevel.GetNode<CanvasLayer>("Cards").AddChild(spawner);
			spawner.GlobalPosition = GlobalPosition;
		}

		if(body is DeathFloor) {
			splatSound.Play();
			ReturnToCheckpoint();
		}
	}
}
