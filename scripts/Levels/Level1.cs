using Godot;
using System;

public partial class Level1 : Trenirovka
{
	private TextureButton card_ivisible;
	private TextureButton card_visible;
	private CanvasLayer card;
	private CanvasLayer control;
	private Town1 town;
	private Timer lvl_t;
	private Label lvl_t_l;
	public override void _Ready()
	{
		town = GetNode<Town1>("%town");
		control = GetNode<CanvasLayer>("%Control");
		card_ivisible = GetNode<TextureButton>("%card_invisible");
		card_visible = GetNode<TextureButton>("%card_visible");
		card = GetNode<CanvasLayer>("%card");
		lvl_t = GetNode<Timer>("%lvl_t");
		lvl_t_l = GetNode<Label>("%lvl_t_l");
		lvl_t.Timeout += TimeoutLvl;
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
		lvl_t_l.Show();
		lvl_t.Start();
		town.on_ai.MouseEntered += () => outline_set(true);
        town.on_ai.MouseExited += () => outline_set(false);
	}
	private void TimeoutLvl()
	{
		int lvl_l_int = Convert.ToInt32(lvl_t_l.Text);
		if(lvl_l_int != 0)
		{
			lvl_t_l.Text = $"{lvl_l_int - 1}";
			if(Convert.ToInt32(lvl_t_l.Text) == 3)
			{
				lvl_t_l.AddThemeColorOverride("font_color", new Color(1, 0, 0));
			}
		}
		else
		{
			lvl_t.Stop();
			losse();
			lvl_t_l.Hide();
		}
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
