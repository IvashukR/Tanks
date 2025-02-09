using Godot;
using System;

public partial class Void : State
{
	private bool life = true;
	private Voin v;
	private FSM fsm;
	
	public override void _Ready()
	{
		var parent = GetParent();
		v = (Voin) parent.GetParent();
		fsm = GetParent<FSM>();
	}
	public override void Process(double delta)
	{
		if (v.GlobalPosition.X < 0 || v.GlobalPosition.X > GetViewport().GetVisibleRect().Size.X || v.GlobalPosition.Y < 0 || v.GlobalPosition.Y > GetViewport().GetVisibleRect().Size.Y)
		{
			v.QueueFree();
		}
		v.GlobalPosition = GetViewport().GetMousePosition();
		v.MoveAndSlide();
	}
	public override void Inp(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
			GlobalManager.Instance.EmitSignal("change_money");
            fsm.change_state("Playing");
			GlobalManager.Instance.block_drop_unit = false;

        }
        
    }
}
