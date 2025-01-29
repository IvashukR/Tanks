using Godot;
using System;

public partial class Level1 : Trenirovka
{
	private TextureButton card_ivisible;
	private TextureButton card_visible;
	private Control card;
	public override void _Ready()
	{
		card_ivisible = GetNode<TextureButton>("%card_invisible");
		card_visible = GetNode<TextureButton>("%card_visible");
		card = GetNode<Control>("%card");
		s =  (PackedScene)ResourceLoader.Load("res://scene/level.tscn");
		card_ivisible.Pressed += () =>
		{
			card_ivisible.Visible = false;
			card_visible.Visible = true;
			foreach(Control i in GetTree().GetNodesInGroup("cards"))
			{
				i.Visible = true;
			}
		};
		card_visible.Pressed += () =>
		{
			card_ivisible.Visible = true;
			card_visible.Visible = false;
			foreach(Control i in GetTree().GetNodesInGroup("cards"))
			{
				i.Visible = false;
			}
		};
		base._Ready();
		Card card_obj = (Card) card;
		card_obj.SetInfo(new Voin());
	}

	
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
