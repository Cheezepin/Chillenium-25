using Godot;
using System;

public partial class Credits : Control
{

	/* Credits Ready Function
	Parameters: None
	Description: Creates a back button and its event for when it is pressed */

	public override void _Ready(){
		var backButton = (TextureButton)GetNode("Return");
		var backButtonCaller = new Callable(this, nameof(OnBackButtonPressed));
		backButton.Connect("pressed", backButtonCaller);
	}

	private void OnBackButtonPressed(){
		Hide();
		GetParent().GetNode<Control>("MainMenuItems").Show();
	}
}
