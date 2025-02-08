using Godot;
using System;
using System.Collections;

public partial class Global : Node
{
	public static string startSceneName = "Test";
	public static Node currentLevel;
	public static Transitions transitions;

	public static int currColorID = 0;
	public static Color currColor;
	public static bool currBW;
	public static bool allColors;

	public enum ColorNames {
		Red,
		Orange,
		Yellow,
		Green,
		Blue,
		Purple,
	};

	public static Color[] colors = {
		Color.Color8(255, 0, 0, 255),
		Color.Color8(255, 127, 0, 255),
		Color.Color8(255, 255, 0, 255),
		Color.Color8(0, 255, 0, 255),
		Color.Color8(0, 0, 255, 255),
		Color.Color8(255, 0, 255, 255),
	};

	public static int colorsUnlocked = 0;
	public enum ColorFilters {
		Red = 1 << 0,
		Orange = 1 << 1,
		Yellow = 1 << 2,
		Green = 1 << 3,
		Blue = 1 << 4,
		Purple = 1 << 5,
	};
	public override void _Ready()
	{
		// Input.MouseMode = Input.MouseModeEnum.Hidden;

		currentLevel = GD.Load<PackedScene>("levels/" + startSceneName + ".tscn").Instantiate(); //debug level
		// currentLevel = GD.Load<PackedScene>("ui/MainMenu.tscn").Instantiate();
		GetTree().CurrentScene.AddChild(currentLevel);
		
		transitions = (Transitions)GD.Load<PackedScene>("global/Transitions.tscn").Instantiate();
		GetTree().CurrentScene.AddChild(transitions);

		currColor = colors[currColorID];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("debug")) {
			transitions.ExitToMainMenu();
		}

		// if(Input.IsActionJustPressed("move_left")) {
		// 	currColor = Color.Color8(255, 0, 0, 255);
		// }
		// if(Input.IsActionJustPressed("move_right")) {
		// 	currColor = Color.Color8(255, 255, 255, 255);
		// }

		if(colorsUnlocked != 0) {
			if(Input.IsActionJustPressed("color_left")) {
				do {
					currColorID--;
					if(currColorID < 0) {currColorID = colors.Length-1;}
				} while ((colorsUnlocked & (1 << (int)currColorID)) != 0);
			}

			if(Input.IsActionJustPressed("color_right")) {
				do {
					currColorID++;
					if(currColorID >= colors.Length) {currColorID = 0;}
				} while ((colorsUnlocked & (1 << (int)currColorID)) != 0);
			}
			currBW = false;
		} else {currBW = true;}

		currColor = colors[currColorID];

		allColors = true;
		currBW = false;
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
