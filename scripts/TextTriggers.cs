using Godot;
using System;

public partial class TextTriggers : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(tutorialLabel != null)
        {
			tutorialLabel.Position = new Vector2(tutorialLabel.Position.X, Mathf.Lerp(tutorialLabel.Position.Y, tutorialPos, (float)delta * 8));
        }
	}

	RichTextLabel tutorialLabel;
	int tutorialPos = -150;

	bool movement = false;
	bool jump = false;
	bool switchT = false;
	bool attack = false;

	public void OnBodyEntered(Node2D body, string tutorial)
    {
		if(body is Player)
        {
			if(tutorialLabel == null) tutorialLabel = GetParent().GetNode<GameUI>("GameUI").GetNode<RichTextLabel>("Tutorial");
			switch (tutorial)
            {
				case "MovementTutorial":
					if(!movement)
                    {
						tutorialLabel.Text = "[center]Use the arrow keys or WASD to move![/center]";
						tutorialPos = 182;
						//movement = true;
                    }
					break;
				case "JumpTutorial":
					if (!jump)
					{
						tutorialLabel.Text = "[center]Use Z or SPACE to jump![/center]";
						tutorialPos = 182;
						//jump = true;
					}
					break;
				case "SwitchTutorial":
					if (!switchT)
					{
						tutorialLabel.Text = "[center]Use Q and E to switch between colors![/center]";
						tutorialPos = 182;
						//switchT = true;
					}
					break;
				case "AttackTutorial":
					if(!attack)
                    {
						tutorialLabel.Text = "[center]Use X to attack![/center]";
						tutorialPos = 182;
						//attack = true;
					}
					break;
			}
        }
    }

	public void OnBodyExit(Node2D body)
    {
		if (body is Player)
		{
			tutorialPos = -150;
		}
    }
}
