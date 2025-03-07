using Godot;
using System;

public partial class Voin : CharacterBody2D, IStats, IUnit
{
	public int patron_count { get; set; } = 10;
	public float perezaryad { get; set; } = 0.5f;
	public int damage { get; set; } = 5;
	public int proch { get; set; } = 20;
	public float speed { get; set; } = 9000;
	public int cost { get; set; } = 100;
	public int cost_death { get; set; } = 120;
	public FSM fsm { get; set; }
	public Node2D voin_sprite { get; set; }
	public TextureButton on_ai;
	private bool this_is_pick_unit;
	private bool is_ai = true;
	public  Label hp_l { get; set; }
	public bool on_ai_active;
	[Export] public string name_unit { get; set; } = "Voin";
	public override void _Ready()
	{
		hp_l = GetNode<Label>("%hp_l");
		on_ai = GetNode<TextureButton>("%on_ai");
		voin_sprite = GetNode<Node2D>("%pig");
		fsm = GetNode<FSM>("%FSM");
		hp_l.Text = $"{name_unit} Health: {proch}";
		fsm.change_state("Void");
		on_ai.Pressed += () =>
		{
			is_ai = !is_ai;
			if(is_ai)fsm.change_state("AI");
			else fsm.change_state("Playing");
			this_is_pick_unit = true;
			GlobalManager.Instance.EmitSignal("pick_unit");
		};
		on_ai.MouseEntered += () =>
		{
			GlobalManager.Instance.block_input = true;
			GamaUtilits.set_shader(voin_sprite, true, "render");
		}; 
    	on_ai.MouseExited += () => 
		{
			GlobalManager.Instance.block_input = false;
			GamaUtilits.set_shader(voin_sprite, false, "render");
		};
		GlobalManager.Instance.pick_unit += PickUnit;
		GlobalManager.Instance.take_damage += TakeDamage;
		GlobalManager.Instance.card_click += CardClick;
	}
	private void PickUnit()
	{
		if(!is_ai && !this_is_pick_unit)is_ai = true;
		if(is_ai)fsm.change_state("AI");
		this_is_pick_unit = false;
	}
	private void TakeDamage(Node2D body, Bullet bullet)
	{
		if(body != this)return;
		GamaUtilits.TakeDamageUnit(body, bullet);
	}
	private void CardClick()
	{
		if(GlobalManager.Instance.temp_pick_unit != this)
		{
			is_ai = true;
			fsm.change_state("AI");
		}
	}
	public override void _ExitTree()
	{
		if(fsm.current_state.Name == "Void")
		{
			GlobalManager.Instance.block_drop_unit = false;
			GlobalManager.Instance.temp_pick_unit = null;
		}
		GlobalManager.Instance.take_damage -= TakeDamage;
		GlobalManager.Instance.pick_unit -= PickUnit;
		GlobalManager.Instance.card_click -= CardClick;
	}
}
