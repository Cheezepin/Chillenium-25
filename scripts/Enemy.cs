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
	[Export]public PackedScene ghostScene;

	public override void _Ready()
	{
		s = (ShaderMaterial)Material;
		velocity = Vector2.Zero;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(flashTimer > 0) {
			s.SetShaderParameter("flash", true);
			flashTimer -= delta;
			if(flashTimer < 0) {flashTimer = 0; s.SetShaderParameter("flash", false);}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity.X = Mathf.MoveToward(velocity.X, 0, 2500.0f * (float)delta);
		velocity.Y += gravity * (float)delta;
		Vector2 xVelVec = new Vector2(velocity.X, 0)*(float)delta; //separation done so horizontal movement is applied without stopping in place
		Vector2 yVelVec = new Vector2(0, velocity.Y)*(float)delta;
		MoveAndCollide(xVelVec);
		MoveAndCollide(yVelVec);
		
		base._PhysicsProcess(delta);
	}

	private void TakeDamage(float dmg) {
		health -= dmg;
		if(health <= 0) {
			var ghost = ghostScene.Instantiate<Ghost>();
			Global.currentLevel.CallDeferred(Node.MethodName.AddChild, ghost);//what?
			ghost.GlobalPosition = GlobalPosition;

			QueueFree();
		}
	}

	public void OnBulletHit(Bullet bullet) {
		// QueueFree();
		flashTimer = 0.04;
		velocity = bullet.velocity * 0.25f;
		TakeDamage(1.0f);
	}
}
