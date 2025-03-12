using Godot;
using System;

public partial class UnitVoidState : State
{
    private bool life = true;
	[Export] private Node2D v;
	[Export] public UnitLogic unit;
	[Export] private Area2D area_void;

	private FSM fsm;
	
	public override void _Ready()
	{
		fsm = GetParent<FSM>();
	}
	public override void Process(double delta)
	{
		if (v.GlobalPosition.X < 0 || v.GlobalPosition.X > GetViewport().GetVisibleRect().Size.X || v.GlobalPosition.Y < 0 || v.GlobalPosition.Y > GetViewport().GetVisibleRect().Size.Y)
		{
			GlobalManager.Instance.block_drop_unit = false;
			v.QueueFree();
		}
		v.GlobalPosition = GetViewport().GetMousePosition();
		if(v is  CharacterBody2D body)body.MoveAndSlide();
	}
	public override void Exit()
	{
		GlobalManager.Instance.block_drop_unit = false;
		area_void.QueueFree();
		area_void = null;
	}
	public override void Enter()
	{
		GlobalManager.Instance.temp_pick_unit = v;
	}
	private void CheckCollideUnit()
	{
		foreach(Node2D node in area_void.GetOverlappingBodies())
		{
			if(node.IsInGroup("unit"))
			{
				return;
			}
		}
	}
	public override void _Inp(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
			CheckCollideUnit();
			GlobalManager.Instance.EmitSignal("change_money", unit.stats.cost);
			GlobalManager.Instance.block_drop_unit = false;
			GlobalManager.Instance.temp_pick_unit = null;
			fsm.change_state("Playing");
        }
        
    }
}