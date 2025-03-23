using Godot;
using System;

namespace GameUnit;
public partial class UnitVoidState : State
{
	[Export] private PhysicsBody2D v;
	[Export] public UnitLogic unit;
	[Export] private Area2D area_void;
	private uint collision_layer;
	private uint collision_mask;
	private int zindex_sprite;
	private FSM fsm;

	public override void _Ready()
	{
		fsm = GetParent<FSM>();
	}
	public override void Process(double delta)
	{
		if (v.GlobalPosition.X < 0 || v.GlobalPosition.X > GetViewport().GetVisibleRect().Size.X || v.GlobalPosition.Y < 0 || v.GlobalPosition.Y > GetViewport().GetVisibleRect().Size.Y)
		{
			Unfocused();
			v.QueueFree();
		}
		v.GlobalPosition = GetViewport().GetMousePosition();
		if(v is  CharacterBody2D body)body.MoveAndSlide();
	}
	public override void Exit()
	{
		// return unit collision layer and mask
		v.CollisionLayer = collision_layer; 
		v.CollisionMask  = collision_mask;
		unit.unit_sprite.ZIndex = zindex_sprite;
		Unfocused();
		area_void.QueueFree();
		area_void = null;
	}
	private void Unfocused()
	{
		GlobalManager.Instance.block_drop_unit = false;
		GlobalManager.Instance.temp_pick_unit = null;
	}
	public override void Enter()
	{
		collision_layer = v.CollisionLayer;
		collision_mask = v.CollisionMask;
		//set unit collision mask and layer on barier unit
		v.CollisionLayer = (1 << 6);
		v.CollisionMask = (1 << 7);
		CallDeferred("SetZIndex");
		GlobalManager.Instance.temp_pick_unit = v;
	}
	private void SetZIndex()
	{
		zindex_sprite = unit.unit_sprite.ZIndex;
		unit.unit_sprite.ZIndex = 1000;
	}
	private bool CheckCollideUnit()
	{
		foreach(Node2D node in area_void.GetOverlappingBodies())
		{
			if(node == v)continue;
			if(node is PhysicsBody2D)
			{
				GlobalManager.Instance.EmitSignal("cant_pick_unit");
				return true;
			}
		}
		return false;
	}
	public override void _Inp(InputEvent @event)
    {
		if (@event is InputEventMouseButton)
        {
			if(CheckCollideUnit())return;
			GlobalManager.Instance.EmitSignal("change_money", unit.stats.cost);
			GlobalManager.Instance.block_drop_unit = false;
			GlobalManager.Instance.temp_pick_unit = null;
			fsm.change_state("Playing");
        }
        
    }
}