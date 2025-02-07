using Godot;
using System;

public partial class Global : Node
{
	public static string startSceneName = "Test";
	public static Node currentLevel;
	public static Transitions transitions;
	
	public override void _Ready()
	{
		// Input.MouseMode = Input.MouseModeEnum.Hidden;

		// currentLevel = GD.Load<PackedScene>("levels/" + startSceneName + ".tscn").Instantiate(); //debug level
		currentLevel = GD.Load<PackedScene>("ui/MainMenu.tscn").Instantiate();
		GetTree().CurrentScene.AddChild(currentLevel);
		
		transitions = (Transitions)GD.Load<PackedScene>("global/Transitions.tscn").Instantiate();
		GetTree().CurrentScene.AddChild(transitions);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("debug")) {
			transitions.ExitToMainMenu();
		}
	}

	public static void AsymptoticApproach(ref float curr, float target, float mult) {
		curr += (target - curr) * mult;
	}

	public static void AsymptoticApproach(ref Vector2 curr, Vector2 target, float mult) {
		curr += (target - curr) * mult;
	}

	public static void SymmetricApproach(ref float curr, float target, float inc) {
		float dist = target - curr;

		if (inc < 0) {inc = -1 * inc;}
		if (dist > 0) {
			dist -= inc;
			if (dist >= 0) {curr = target - dist;}
			else {curr = target;}
		} else {
			dist += inc;
			if (dist <= 0) {curr = target - dist;}
			else {curr = target;}
		}
	}
}
