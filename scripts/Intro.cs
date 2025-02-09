using Godot;
using System;

public partial class Intro : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private AnimatedSprite2D hand;
	private AnimatedSprite2D girl;
	private Vector2 home;
	private int introState = 0;
	private double timer = 0;
	private Player player;
	public override void _Ready()
	{
		hand = GetNode<AnimatedSprite2D>("Hand");
		girl = GetNode<AnimatedSprite2D>("Girl");
		home = hand.Position;
		hand.Position += new Vector2(5000.0f, 0.0f);
		player = ((Level)Global.currentLevel).player;
		girl.Show();
		hand.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		player = ((Level)Global.currentLevel).player;
		// GD.Print(player.GlobalPosition.X);
		Vector2 handPos = hand.Position;
		Vector2 girlPos = girl.Position;
		switch(introState) {
			case 0:
				if(player.GlobalPosition.X > -500.0) {
					introState++;
					timer = 0;
				}
				break;
			case 1:
				Global.AsymptoticApproach(ref handPos, new Vector2(home.X, handPos.Y), 5.5f*(float)delta);
				if(timer >= 0.8) {
					girl.Play("snatch");
					introState++;
				}
				break;
			case 2:
				Global.AsymptoticApproach(ref handPos, new Vector2(home.X, handPos.Y), 5.5f*(float)delta);
				if(timer >= 1.0) {
					hand.Hide();
					Global.AsymptoticApproach(ref girlPos, new Vector2(home.X + 10000.0f, girlPos.Y), 0.75f*(float)delta);
				}
				if(timer >= 10.0) {Hide(); QueueFree();}
				break;
		}
		hand.Position = handPos;
		girl.Position = girlPos;
		timer += delta;
	}
}
