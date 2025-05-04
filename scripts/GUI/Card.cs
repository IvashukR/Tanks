using Godot;
using System;
using GameUnit;

namespace GameView;
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
	private Label hp_l;
	private Label ai_l;
	public TextureButton show_i;
	public TextureButton hide_i;
	public TextureButton main_btn;
	[Export] public string _path;

	public void SetInfo(UnitStats stats)
	{
		money_l.Text = $": {stats.cost}";
		patron_l.Text = $"ammo: {stats.patron_count}";
		speed_l.Text = $"speed: {stats.speed / 100}";
		per_l.Text = $"reload: {stats.perezaryad}";
		d_cost_l.Text = $"cost death: {stats.cost_death}";
		damage_l.Text = $"damage: {stats.damage}";
		hp_l.Text = $"health: {stats.proch}";
		ai_l.Text = $"ai iq: {stats.iq_ai}";
		cost = stats.cost;
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
		ai_l = GetNode<Label>("%ai_l");
		info = GetNode<VBoxContainer>("%info");
		show_i = GetNode<TextureButton>("%show_i");
		hide_i = GetNode<TextureButton>("%hide_i");
		money_l = GetNode<Label>("%money_l");
		hp_l = GetNode<Label>("%hp_l");
		speed_l = GetNode<Label>("%speed_l");
		per_l = GetNode<Label>("%per_l");
		d_cost_l = GetNode<Label>("%death_l");
		patron_l = GetNode<Label>("%patron_l");
		damage_l = GetNode<Label>("%damage_l");
		main_btn.Pressed += () => 
		{
			if(GlobalManager.Instance.temp_pick_unit != null)return;
			GlobalManager.Instance.EmitSignal("card_click");
			if(!GlobalManager.Instance.block_drop_unit &&  GlobalManager.Instance.money - Convert.ToInt32(cost) >= 0 )Buy(_path);
			else GlobalManager.Instance.EmitSignal("havent_money");
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
