using Godot;
using System;
using TanksUtilits;
using GameObjects;

namespace GameUnit;
public partial class UnitLogic : Node2D
{
    [Export] public string name_unit = "Voin";
	[Export] public UnitStats stats;
	[Export] private Node2D unit;
    public TextureButton on_ai;
	protected bool this_is_pick_unit;
    public FSM fsm;
    public  Node2D unit_sprite;
    public  Label hp_l;
	public CpuParticles2D bloom;
    private bool is_ai;
    public override void _Ready()
    {
        hp_l = GetNode<Label>("%hp_l");
		bloom = GetNode<CpuParticles2D>("%bloom");
        on_ai = GetNode<TextureButton>("%on_ai");
		unit_sprite = GetNode<Node2D>("%unit_sprite");
		fsm = GetNode<FSM>("%FSM");
		hp_l.Text = $"{name_unit} Health: {stats.proch}";
		on_ai.Pressed += () =>
		{
			if(ReturnCantPick())return;
			if(fsm.current_state.Name == "Void")return;
			is_ai = !is_ai;
			if(is_ai)fsm.change_state("AI");
			else fsm.change_state("Playing");
			this_is_pick_unit = true;
			GlobalManager.Instance.EmitSignal("pick_unit");
		};
        on_ai.MouseEntered += () =>
		{
			if(ReturnCantPick())return;
			GlobalManager.Instance.block_input = true;
			GamaUtilits.set_shader(unit_sprite, true, "render");
		}; 
        on_ai.MouseExited += () => 
		{
			if(ReturnCantPick())return;
			GlobalManager.Instance.block_input = false;
			GamaUtilits.set_shader(unit_sprite, false, "render");
		};
        GlobalManager.Instance.pick_unit += PickUnit;
		GlobalManager.Instance.card_click += CardClick;
    }
    
    protected virtual void PickUnit()
	{
		if(!is_ai && !this_is_pick_unit)is_ai = true;
		if(is_ai)fsm.change_state("AI");
		this_is_pick_unit = false;
	}
    protected virtual void CardClick()
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
		GlobalManager.Instance.pick_unit -= PickUnit;
		GlobalManager.Instance.card_click -= CardClick;
	}
	private bool ReturnCantPick()
	{
		if(GlobalManager.Instance.temp_pick_unit != null)
		if(!CanPick())return true;
		return false;
	}
	private bool CanPick()
	{
		return GlobalManager.Instance.temp_pick_unit ==  unit;
	}
}
