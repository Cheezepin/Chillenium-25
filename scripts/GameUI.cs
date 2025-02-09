using Godot;
using System;

public partial class GameUI : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	private TextureRect slide;
	private RichTextLabel end;
	public override void _Ready()
	{
		slide = GetNode<TextureRect>("Slide");
		end = GetNode<RichTextLabel>("End");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		slide.Position = new Vector2(-1920.0f, 0.0f);
		float x = Global.transitionLeft ? -1920.0f : 1920.0f;
		if(Global.colorSlideProgress < 1.0) {
			slide.Position += new Vector2((float)Global.colorSlideProgress*x, 0);
		} else {
			slide.Position += new Vector2((float)Global.colorSlideProgress*x - (x*2.0f), 0);
		}

		if(Global.endScreenTriggered) {
			end.Show();
		}
	}
}
