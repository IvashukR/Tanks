using Godot;
using System;

public partial class Void : State
{
	private bool life = true;
	private Timer timer;
	private Voin v;
	private FSM fsm;
	
	public override void _Ready()
	{
		GD.Print("READY_VOID");
		var parent = GetParent();
		v = (Voin) parent.GetParent();
		timer = GetNode<Timer>("%t");
		timer.Timeout += () => parent.QueueFree();
		fsm = GetParent<FSM>();
	}
	public override void Enter()
	{
		timer.Start();
	}
	public override void Exit()
	{
		timer.Stop();
	}
	
	public override void Process(double delta)
	{

		v.GlobalPosition = GetViewport().GetMousePosition();
	}
	public override void Inp(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
			GlobalManager.Instance.EmitSignal("change_money");
            fsm.change_state("Playing");
			GD.Print(fsm.current_state);

        }
        
    }
}
