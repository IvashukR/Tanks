using Godot;
using System;

public partial class AudioServer : Node
{
	private AudioStreamPlayer drive_tank;
	
	
	public override void _Ready()
	{
		drive_tank = GetNode<AudioStreamPlayer>("DriveTank");
		drive_tank.Play();
		GlobalManager.Instance.drive_tank += () => drive_tank.Stop();
		
	}


	public override void _Process(double delta)
	{
		
	}
}
