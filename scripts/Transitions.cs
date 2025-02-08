using Godot;
using System;
using static Global;

public partial class Transitions : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	private string levelTo = null;
	private bool toMainMenu = false;
	private double timer = 0;

	private bool transitionInto = false;
	private bool transitionFrom = false;

	private ColorRect dotFadeRect;
	private ShaderMaterial dotFade;

	private ColorRect screenWipeRect;
	private ShaderMaterial screenWipe;
	public override void _Ready()
	{
		dotFadeRect = GetNode<ColorRect>("DotFade");
		dotFade = (ShaderMaterial)dotFadeRect.Material;
		dotFadeRect.Hide();

		screenWipeRect = GetNode<ColorRect>("ScreenWipe");
		screenWipe = (ShaderMaterial)screenWipeRect.Material;
		screenWipeRect.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void LoadNewLevel() {
		if(Global.currentLevel != null)
			Global.currentLevel.QueueFree();
		if(toMainMenu) Global.currentLevel = GD.Load<PackedScene>("ui/MainMenu.tscn").Instantiate();
		else Global.currentLevel = GD.Load<PackedScene>("levels/" + levelTo + ".tscn").Instantiate();
		GetTree().CurrentScene.AddChild(Global.currentLevel);
		GetTree().CurrentScene.MoveChild(Global.currentLevel, 0);
		levelTo = null;
		toMainMenu = false;
	}

	private void DotFade(double delta) {
		dotFadeRect.Show();
		if(transitionInto) {
			dotFade.SetShaderParameter("radius", timer / 6.0);

			if(timer >= 0.75) {
				transitionInto = false;
				transitionFrom = true;
				timer = 0;
				LoadNewLevel();
			}
			timer += delta;
		} else if (transitionFrom) {
			dotFade.SetShaderParameter("radius", (timer / 6.0) + .22);
			if(timer >= 1.0) {transitionFrom = false; timer = 0;}
			timer += delta;
		} else {
			dotFadeRect.Hide();
		}
	}

	public void ScreenWipe(double delta) {
		screenWipeRect.Show();
		if(transitionInto) {
			screenWipe.SetShaderParameter("progress", timer*2);

			if(timer >= 0.5) {
				transitionInto = false;
				transitionFrom = true;
				timer = 0;
				LoadNewLevel();
			}
			timer += delta;
		} else if (transitionFrom) {
			screenWipe.SetShaderParameter("progress", (timer*2) + 1);
			if(timer >= 0.5) {transitionFrom = false; timer = 0;}
			timer += delta;
		} else {
			screenWipeRect.Hide();
		}
	}
	
	public override void _Process(double delta)
	{
		DotFade(delta);
		// ScreenWipe(delta);
	}

	public void ChangeLevel(string nextLevel) {
		levelTo = nextLevel;
		timer = 0.0;
		transitionInto = true;
	}

	public void ExitToMainMenu() {
		toMainMenu = true;
		timer = 0.02;
		transitionInto = true;
	}
}
