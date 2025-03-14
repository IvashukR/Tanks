using Godot;
using System;

namespace GameUnit.Voin.States;
public partial class Playing : State
{
	private Vector2 dir;
	private Node fsm;
	[Export] public UnitLogic unit;
	private bool pressed_on_ai = false;
	private AnimationPlayer anim;
	private BoxContainer info;
	[Export] private Voin v;
	public override void _Ready()
	{
		info = GetNode<BoxContainer>("%info");
		anim = GetNode<AnimationPlayer>("%anim");
		fsm = GetNode<Node>("%FSM");
	}
	public override void Enter()
	{
		info.Show();
	}
	public override void Exit()
	{
		if(info != null)info.Hide();
	}
	public override void Process(double delta)
	{
		
		Vector2 mouse_pos = GetViewport().GetMousePosition();
		v.LookAt(mouse_pos);
		if (v.Velocity.Length() > 0)
		{
			anim.Play("go");
		}
		else anim.Stop();
	}
	public override void PhysicsProcess(double delta)
	{
		dir = Input.GetVector("l", "r", "d", "u").Normalized();
		v.Velocity = dir  * unit.stats.speed * (float)delta;
		v.MoveAndSlide();
	}
	public override void Inp(InputEvent @event)
    {
		if(GlobalManager.Instance.block_input)return;
		if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
            {
                v.Shoot(true);
            }
        }
        
    }
	
}
