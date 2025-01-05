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
		var parent = GetTree().CurrentScene;
		v = (Voin) parent;
		timer = GetNode<Timer>("%t");
		timer.Timeout += () => parent.QueueFree();
		fsm = GetParent<FSM>();
	}
	public override void Enter()
	{
		timer.Start();
	}
	
	public override void _Process(double delta)
	{

		v.GlobalPosition = GetViewport().GetMousePosition();
	}
	public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
			GlobalManager.Instance.EmitSignal("change_money");
            fsm.change_state("Playing");
        }
        
    }
}
