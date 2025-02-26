using Godot;
using System;

public partial class Card : CanvasLayer
{
	private Label money_l;
	private Label speed_l;
	private Label per_l;
	private Label d_cost_l;
	private Label damage_l;
	private Label patron_l;
	private int cost;
	private VBoxContainer info;
	private ColorRect cr_info;
	public TextureButton show_i;
	public TextureButton hide_i;
	public TextureButton main_btn;
	[Export] public string _path;

	public void SetInfo(object obj)
	{
		if (obj is IStats target)
		{
			money_l.Text = $": {target.cost}";
			patron_l.Text = $"патрони: {target.patron_count}";
			speed_l.Text = $"скорость: {target.speed / 100}";
			per_l.Text = $"перезарядка: {target.perezaryad}";
			d_cost_l.Text = $"цена смерти: {target.cost_death}";
			damage_l.Text = $"урон: {target.damage}";
			cost = target.cost;
		}
	}
	
	public void Buy(string _path)
	{
		GlobalManager.Instance.block_drop_unit = true;
		PackedScene s  = ResourceLoader.Load<PackedScene>(this._path);
		var sc = s.Instantiate<Node2D>();
		Node fsm_node = sc.GetNode("%FSM");
		FSM fsm = fsm_node as FSM;
		sc.GlobalPosition = main_btn.GlobalPosition;
		GetParent().AddChild(sc);
		fsm.change_state("Void");
	}
	public override void _Ready()
	{
		cr_info = GetNode<ColorRect>("%cr_info");
		main_btn = GetNode<TextureButton>("%main_btn");
		info = GetNode<VBoxContainer>("%info");
		show_i = GetNode<TextureButton>("%show_i");
		hide_i = GetNode<TextureButton>("%hide_i");
		money_l = GetNode<Label>("%money_l");
		speed_l = GetNode<Label>("%speed_l");
		per_l = GetNode<Label>("%per_l");
		d_cost_l = GetNode<Label>("%death_l");
		patron_l = GetNode<Label>("%patron_l");
		damage_l = GetNode<Label>("%damage_l");
		main_btn.Pressed += () => 
		{
			GlobalManager.Instance.EmitSignal("card_click");
			if(!GlobalManager.Instance.block_drop_unit &&  GlobalManager.Instance.money - Convert.ToInt32(cost) >= 0)Buy(_path);
		};
		show_i.Pressed += () => {
			info.Visible = true;
			cr_info.Visible = true;
			show_i.Visible = false;
			hide_i.Visible = true;
		};
		hide_i.Pressed += () => {
			info.Visible = false;
			cr_info.Visible = false;
			hide_i.Visible = false;
			show_i.Visible = true;
		};

	}

	
}
