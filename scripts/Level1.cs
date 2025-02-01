using Godot;
using System;
using System.Collections.Generic;

public partial class Level1 : Trenirovka
{
	private TextureButton card_ivisible;
	private TextureButton card_visible;
	private CanvasLayer card;
	private Control control;
	public List<TextureButton> all_btn_ui = new List<TextureButton>();
	public override void _Ready()
	{
		control = GetNode<Control>("%Control");
		card_ivisible = GetNode<TextureButton>("%card_invisible");
		card_visible = GetNode<TextureButton>("%card_visible");
		card = GetNode<CanvasLayer>("%card");
		s =  (PackedScene)ResourceLoader.Load("res://scene/level.tscn");
		Start += () => card_ivisible.Show();
		card_ivisible.Pressed += () =>
		{
			card_ivisible.Visible = false;
			card_visible.Visible = true;
			foreach(CanvasLayer i in GetTree().GetNodesInGroup("cards"))
			{
				i.Visible = true;
			}
		};
		foreach(var node in control.GetChildren())
		{
			if(node is TextureButton)all_btn_ui.Add((TextureButton)node);
		}
		card_visible.Pressed += () =>
		{
			card_ivisible.Visible = true;
			card_visible.Visible = false;
			foreach(CanvasLayer i in GetTree().GetNodesInGroup("cards"))
			{
				i.Visible = false;
			}
		};
		foreach(Card i in GetTree().GetNodesInGroup("cards"))
		{
			all_btn_ui.Add(i.show_i);
			all_btn_ui.Add(i.hide_i);
			all_btn_ui.Add(i.main_btn);
		}
		base._Ready();
		Card card_obj = (Card) card;
		card_obj.SetInfo(new Voin());
	}

	
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
