using Godot;
using System;
using TanksUtilits;
using GameUnit;
using GameView;
using GameObjects;


namespace GameLevels;
public partial class Level1 : Trenirovka
{
	private TextureButton card_ivisible;
	private TextureButton card_visible;
	private CanvasLayer card;
	private CanvasLayer control;
	private Town1 town;
	private Timer lvl_t;
	private Label lvl_t_l;
	private AnimationPlayer anim_cant_pick_unit;
	public override void _Ready()
	{
		town = GetNode<Town1>("%town");
		control = GetNode<CanvasLayer>("%Control");
		card_ivisible = GetNode<TextureButton>("%card_invisible");
		card_visible = GetNode<TextureButton>("%card_visible");
		card = GetNode<CanvasLayer>("%card");
		lvl_t = GetNode<Timer>("%lvl_t");
		anim_cant_pick_unit = GetNode<AnimationPlayer>("%anim_cant_pick_unit");
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
		GlobalManager.Instance.cant_pick_unit += CantPickUnit;
	}
	protected override void losse()
	{
		base.losse();
		lvl_t_l.Hide();
	}
	protected override void win()
	{
		base.win();
		lvl_t_l.Hide();
	}
	private void CantPickUnit()
	{
		if(!anim_cant_pick_unit.IsPlaying())anim_cant_pick_unit.Play("start");
	}
	protected virtual void _Start()
	{
		card_ivisible.Show();
		lvl_t_l.Show();
		lvl_t.Start();
		town.on_ai.MouseEntered += () =>
		{
			if(GlobalManager.Instance.temp_pick_unit != null)return;
			outline_set(true);
		};
        town.on_ai.MouseExited += () =>
		{
			if(GlobalManager.Instance.temp_pick_unit != null)return;
			outline_set(false);
		};
	}
	private void TimeoutLvl()
	{
		int lvl_l_int = Convert.ToInt32(lvl_t_l.Text);
		if(lvl_l_int != 0)
		{
			lvl_t_l.Text = $"{lvl_l_int - 1}";
			int new_value = Convert.ToInt32(lvl_t_l.Text);
			switch (new_value)
			{
				case 3:
				lvl_t_l.AddThemeColorOverride("font_color", new Color(1, 0, 0));
				break;
				case 0:
				ShowLosse();
				break;
			}
		}
	}
	private void ShowLosse()
	{
		lvl_t.Stop();
		losse();
		lvl_t_l.Hide();
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
		GlobalManager.Instance.cant_pick_unit -= CantPickUnit;
		GlobalManager.Instance.money = 0;
		Start -= _Start;
	}
}
