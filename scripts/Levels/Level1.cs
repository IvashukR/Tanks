using Godot;
using System;

public partial class Level1 : Trenirovka
{
	private TextureButton card_ivisible;
	private TextureButton card_visible;
	private CanvasLayer card;
	private CanvasLayer control;
	private Town1 town;
	public override void _Ready()
	{
		town = GetNode<Town1>("%town");
		control = GetNode<CanvasLayer>("%Control");
		card_ivisible = GetNode<TextureButton>("%card_invisible");
		card_visible = GetNode<TextureButton>("%card_visible");
		card = GetNode<CanvasLayer>("%card");
		s =  (PackedScene)ResourceLoader.Load("res://scene/level.tscn");
		Start += _Start;
		card_ivisible.Pressed += () =>
		{
			card_ivisible.Visible = false;
			card_visible.Visible = true;
			foreach(CanvasLayer i in GetTree().GetNodesInGroup("cards"))
			{
				i.Visible = true;
			}
		};
		card_visible.Pressed += () =>
		{
			card_ivisible.Visible = true;
			card_visible.Visible = false;
			foreach(CanvasLayer i in GetTree().GetNodesInGroup("cards"))
			{
				i.Visible = false;
			}
		};
		base._Ready();
		Card card_obj = (Card) card;
		card_obj.SetInfo((UnitStats)ResourceLoader.Load("res://CustomResources/DefaultVoinStats.tres"));
	}
	protected virtual void _Start()
	{
		card_ivisible.Show();
		town.on_ai.MouseEntered += () => outline_set(true);
        town.on_ai.MouseExited += () => outline_set(false);
	}
	private void outline_set(bool value) => GamaUtilits.set_shader(town, value, "render");
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		GlobalManager.Instance.temp_pick_unit = null;
		GlobalManager.Instance.money = 0;
		Start -= _Start;
	}
}
