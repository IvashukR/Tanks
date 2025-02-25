using Godot;
using System;

public partial class AudioServer : Node
{
	private AudioStreamPlayer pig_main_menu;
	
	public override void _Ready()
	{
		pig_main_menu = GetNode<AudioStreamPlayer>("%pig_main_menu");
		GlobalManager.Instance.pig_main_menu_audio += () => pig_main_menu.Play();
	}


}
