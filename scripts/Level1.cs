using Godot;
using System;

public partial class Level1 : Trenirovka
{
	
	private Control card;
	public override void _Ready()
	{
		card = GetNode<Control>("%card");
		s =  (PackedScene)ResourceLoader.Load("res://scene/level.tscn");
		base._Ready();
		Card card_obj = (Card) card;
		card_obj.SetInfo(new Voin());
	}

	
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
