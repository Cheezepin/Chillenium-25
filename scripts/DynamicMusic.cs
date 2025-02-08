using Godot;
using System;

public partial class DynamicMusic : Node
{
	[Export]
	AudioStreamPlayer[] musicTracks;
	float[] maxVolumes;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		maxVolumes = new float[musicTracks.Length];
		for(int i = 0; i < musicTracks.Length; i++)
        {
			maxVolumes[i] = musicTracks[i].VolumeDb;
			if(i != 0) musicTracks[i].VolumeDb = -80;
			// musicTracks[i].Play();
        }
	}

	public bool startedPlaying = false;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int totalColors = (Global.colorsUnlocked & 1)
						+ ((Global.colorsUnlocked >> 1) & 1)
						+ ((Global.colorsUnlocked >> 2) & 1)
						+ ((Global.colorsUnlocked >> 3) & 1)
						+ ((Global.colorsUnlocked >> 4) & 1)
						+ ((Global.colorsUnlocked >> 5) & 1);

		if(!startedPlaying && Global.currBW) {
			startedPlaying = true;
			for(int i = 0; i < musicTracks.Length; i++)
			{musicTracks[i].Play();}
		} 

		for(int i = 0; i < musicTracks.Length; i++)
        {
			float newVol = -80;
			if (i <= totalColors) newVol = maxVolumes[i];
			musicTracks[i].VolumeDb = Mathf.Lerp(musicTracks[i].VolumeDb, newVol, (float)delta * 4);
        }
	}
}
