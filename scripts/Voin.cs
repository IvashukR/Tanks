using Godot;
using System;

public partial class Voin : CharacterBody2D, IStats
{
	public int patron_count { get; set; } = 10;
	public float perezaryad { get; set; } = 0.5f;
	public int damage { get; set; } = 5;
	public int proch { get; set; } = 20;
	public float speed { get; set; } = 9000;
	public int cost { get; set; } = 100;
	public int cost_death { get; set; } = 120;
	private FSM fsm;
	private Node2D voin_sprite;
	public TextureButton on_ai;
	private bool this_is_pick_unit;
	private bool is_ai = true;
	private Label hp_l;
	[Export] public string name_unit = "Voin";
	public override void _Ready()
	{
		hp_l = GetNode<Label>("%hp_l");
		on_ai = GetNode<TextureButton>("%on_ai");
		voin_sprite = GetNode<Node2D>("%pig");
		fsm = GetNode<FSM>("%FSM");
		on_ai.Disabled = true;
		hp_l.Text = $"{name_unit} Health: {proch}";
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
			Town1.set_shader(voin_sprite, true, "render");
		}; 
    	on_ai.MouseExited += () => 
		{
			GlobalManager.Instance.block_input = false;
			Town1.set_shader(voin_sprite, false, "render");
		};
		GlobalManager.Instance.pick_unit += () =>
		{
			if(!is_ai && !this_is_pick_unit)is_ai = true;
			if(is_ai)fsm.change_state("AI");
			this_is_pick_unit = false;
		};
		GlobalManager.Instance.take_damage += (node, bullet) => TakeDamage(node, bullet);
	}
	private async void TakeDamage(Node2D body, Bullet bullet)
	{
		if(body != this)return;
		proch -= bullet.damage;
		hp_l.Text = $"{name_unit} Health: {proch}";
		if(proch > 0)
		{
			Town1.set_shader(voin_sprite, true, "damage");
            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
            Town1.set_shader(voin_sprite, false, "damage");
		}
		else
		{

			QueueFree();
		}
		
	}
}
