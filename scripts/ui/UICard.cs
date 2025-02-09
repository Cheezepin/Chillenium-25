using Godot;
using System;

public partial class UICard : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]public int id;
	private RotateSlides parent;
	private TextureRect child;
	public override void _Ready()
	{
		parent = GetParent<RotateSlides>();
		child = GetChild<TextureRect>(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int val;
		int checkID;

		int totalColors = (Global.colorsUnlocked & 1)
						+ ((Global.colorsUnlocked >> 1) & 1)
						+ ((Global.colorsUnlocked >> 2) & 1)
						+ ((Global.colorsUnlocked >> 3) & 1)
						+ ((Global.colorsUnlocked >> 4) & 1)
						+ ((Global.colorsUnlocked >> 5) & 1);
		if(Global.allColors) {totalColors = 6;}

		switch(id) {
			case 0:
				child.Modulate = Global.colors[parent.currColor];
				break;
			case 1:
			case -1:
				if(totalColors < 2) {child.Hide();}
				child.Show();
				val =  id > 0 ? 1 : -1;
				checkID = (parent.currColor + val + 12) % 6;
				if(!Global.allColors) {while ((Global.colorsUnlocked & (1 << checkID)) == 0) {
					checkID += val;
					checkID += 12;
					checkID %= 6;
				}}
				child.Modulate = Global.colors[checkID];
				break;
			case 2:
			case -2:
				if(totalColors < 2) {child.Hide();}
				child.Show();
				val =  id > 0 ? 1 : -1;
				checkID = (parent.currColor + val + 12) % 6;
				if(!Global.allColors) {while ((Global.colorsUnlocked & (1 << checkID)) == 0) {
					checkID += val;
					checkID += 12;
					checkID %= 6;
				}
				checkID = (checkID + val + 12) % 6;
				while ((Global.colorsUnlocked & (1 << checkID)) == 0) {
					checkID += val;
					checkID += 12;
					checkID %= 6;
				}}
				child.Modulate = Global.colors[checkID];
				break;
		}
		// if((Global.colorsUnlocked & (1 << col)) != 0) Show(); else Hide();
		// if(Global.allColors) Show();
	}
}
