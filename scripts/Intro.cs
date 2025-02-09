using Godot;
using System;

public partial class Intro : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private AnimatedSprite2D hand;
	private AnimatedSprite2D girl;
	private Vector2 home;
	private int state = 0;
	private double timer = 0;
	private Player player;
	public override void _Ready()
	{
		hand = GetNode<AnimatedSprite2D>("Hand");
		girl = GetNode<AnimatedSprite2D>("Girl");
		home = hand.Position;
		hand.Position += new Vector2(500.0f, 0.0f);
		player = ((Level)Global.currentLevel).player;
		girl.Show();
		hand.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch(state) {
			case 0:
				if(player.GlobalPosition.X > -600) {
					state++;
					timer = 0;
				}
				break;
			case 1:
				hand.Position += new Vector2(-500.0f*(float)delta, 0.0f);
				if(timer >= 0.8) {
					girl.Play("snatch");
					state++;
				}
				break;
			case 2:
				if(timer >= 1.0) {
					hand.Hide();
					girl.Position += new Vector2(500.0f*(float)delta, 0.0f);
				}
				break;
		}
	}
}
