using Godot;
using System;

public partial class Voin : CharacterBody2D, IStats
{
	public int patron_count { get; set; } = 10;
	public float perezaryad { get; set; } = 0.5f;
	public int damage { get; set; } = 5;
	public int proch { get; set; } = 20;
	public float speed { get; set; } = 100;
	public int cost { get; set; } = 100;
	public int cost_death { get; set; } = 120;
	private FSM fsm;
	private Node2D voin_sprite;
	public TextureButton on_ai;
	private bool this_is_pick_unit;
	private bool is_ai;
	public override void _Ready()
	{
		on_ai = GetNode<TextureButton>("%on_ai");
		voin_sprite = GetNode<Node2D>("%pig");
		fsm = GetNode<FSM>("%FSM");
		on_ai.Pressed += () =>
		{
			is_ai = !is_ai;
			if(is_ai)fsm.change_state("AI");
			else fsm.change_state("Playing");
			this_is_pick_unit = true;
			GlobalManager.Instance.EmitSignal("pick_unit");
		};
		on_ai.MouseEntered += () => Town1.set_outline(voin_sprite, true);
    	on_ai.MouseExited += () => Town1.set_outline(voin_sprite, false);
		GlobalManager.Instance.pick_unit += () =>
		{
			is_ai = !this_is_pick_unit;
			if(is_ai)fsm.change_state("AI");
			this_is_pick_unit = false;
		};
		if(GetParent() is Level1 level)level.all_btn_ui.Add(on_ai);
	}

}
