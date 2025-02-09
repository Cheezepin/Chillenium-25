using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Enemy : AnimatableBody2D
{

	ShaderMaterial s;
	private double flashTimer = 0;

	private Vector2 velocity;

	public const float gravity = 2500.0f;

	[Export]public float health = 1.0f;

	[Export]public bool canBeJumpedOn = true;

	[Export] public AudioStreamPlayer ding;
	[Export] public AudioStreamPlayer hitSound;

	private double timer = 0;

	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
		velocity = Vector2.Zero;
		velocity.X = 0;
		velocity.Y = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void ApproachScale(double delta) {
		float sign = GlobalScale.Y < 0 ? -1 : 1;
		float xScale = Math.Abs(GlobalScale.X);
		float yScale = Math.Abs(GlobalScale.Y);
		Global.AsymptoticApproach(ref xScale, 1.0f, (float)(5.0*delta));
		Global.AsymptoticApproach(ref yScale, 1.0f, (float)(5.0*delta));
		GlobalScale = new Vector2(xScale, yScale);
		// Global.AsymptoticApproach(ref scale, Vector2.One, (float)(5.0*delta));
		// scale.Y *= sign;
		// GD.Print(scale + " " + GlobalScale);
		// GlobalScale = scale;
	}
	public override void _Process(double delta)
	{
		if(flashTimer > 0) {
			s.SetShaderParameter("flash", true);
			flashTimer -= delta;
			if(flashTimer < 0) {flashTimer = 0; s.SetShaderParameter("flash", false);}
		}

		ApproachScale(delta);

		timer += delta;
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity.X = Mathf.MoveToward(velocity.X, 0, 3000.0f * (float)delta);
		velocity.Y += gravity * (float)delta;
		Vector2 xVelVec = new Vector2(velocity.X, 0)*(float)delta; //separation done so horizontal movement is applied without stopping in place
		Vector2 yVelVec = new Vector2(0, velocity.Y)*(float)delta;
		MoveAndCollide(xVelVec);
		MoveAndCollide(yVelVec);
		
		base._PhysicsProcess(delta);
	}

	private void TakeDamage(float dmg) {
		ding.Play();
		hitSound.Play();
		health -= dmg;
		if(health <= 0) {
			QueueFree();
		}
	}

	public void OnBulletHit(Bullet bullet) {
		// QueueFree();
		flashTimer = 0.04;
		velocity = bullet.velocity * 0.25f;
		TakeDamage(1.0f);
	}

	public void OnJumpedOn() {
		flashTimer = 0.04;
		TakeDamage(1.0f);
		GlobalScale = new Vector2(GlobalScale.X, 0.5f);
	}

	public void _OnHurtboxAreaEntered(Node2D body) {
		if(timer < 0.2) return;
		if(body is Hitbox) {
			flashTimer = 0.04;
			velocity.X = body.GlobalPosition.X - GlobalPosition.X > 0 ? -1000.0f : 1000.0f;
			TakeDamage(1.0f);
			GlobalScale = new Vector2(0.5f, 1.0f);
		}
	}
}
