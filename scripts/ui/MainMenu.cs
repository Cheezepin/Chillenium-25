using Godot;
using System;

public partial class MainMenu : Control
{
	
	Credits creditsNode;
	AudioStreamPlayer hoverSound;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hoverSound = GetNode<AudioStreamPlayer>("Hover");
		void Hover()
        {
			hoverSound.Play();
        }
		Callable hoverCallable = new Callable(this, nameof(Hover));

		var playButton = (TextureButton)GetNode("MainMenuItems/Play");
		var playButtonCaller = new Callable(this, nameof(OnPlayButtonPressed));
		playButton.Connect("pressed", playButtonCaller);

		var creditsButton = (TextureButton)GetNode("MainMenuItems/Credits");
		var creditsButtonCaller = new Callable(this, nameof(OnCreditsButtonPressed));
		creditsButton.Connect("pressed", creditsButtonCaller);

		var quitButton = (TextureButton)GetNode("MainMenuItems/Quit");
		var quitButtonCaller = new Callable(this, nameof(OnQuitButtonPressed));
		quitButton.Connect("pressed", quitButtonCaller);

		creditsNode = GetNode<Credits>("Credits");

		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	private void OnPlayButtonPressed(){
		// Global.FadeToScene("res://levels/level1.tscn");
		// Global.FadeToScene("res://ui/cutscenes/cutscene_1.tscn");
		hoverSound.Play();
		Global.transitions.ChangeLevel("SkyLevel");
	}

	private void OnCreditsButtonPressed(){
		creditsNode.Show();
		GetNode<Control>("MainMenuItems").Hide();
		hoverSound.Play();
	}

	private void OnQuitButtonPressed(){
		hoverSound.Play();
		GetTree().Quit();
	}
}
