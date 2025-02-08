using Godot;
using System;

public partial class Camera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	private Player player;

	private float shakeMagnitude = 0.0f;
	private double shakeTimer = 0.0;

	private Random r;
	public override void _Ready()
	{
		player = Global.currentLevel.GetNode<Player>("Player");
		GlobalPosition = player.GlobalPosition;
		r = new Random();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 position = GlobalPosition;

		Vector2 intendedPos = player.GlobalPosition + new Vector2(0, -50.0f);

		if(shakeTimer > 0) {
			Vector2 shakeVec = new Vector2(r.NextSingle() - 0.5f, r.NextSingle() - 0.5f)*shakeMagnitude;
			intendedPos += shakeVec;
			shakeTimer = Math.Clamp(shakeTimer-delta, 0, Mathf.Inf);
		}
		Global.AsymptoticApproach(ref position, intendedPos, 12.5f*(float)delta);
		GlobalPosition = position; //wtf
		// GlobalPosition = intendedPos;
	}

	public void ShakeCamera(float magnitude, double timer) {
		shakeMagnitude = magnitude;
		shakeTimer = timer;
	}
}
