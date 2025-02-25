using Godot;
using System;
using System.Collections.Generic;
public partial class FSM : Node
{
	[Export] private State initial_state;
	public State current_state;
	private Dictionary<string, State> States = new Dictionary<string, State>();

	public override void _Ready()
	{
		foreach (var state  in GetChildren())
		{
			if (state is State)
			{
				State st = (State) state;
				States.Add(state.Name , st);
			}
		}
		if (initial_state != null)
		{
			current_state = initial_state;
			current_state.Enter();
		}
	}

	public void change_state(string new_state)
	{
		if (current_state == States[new_state])
		{
			return;
		}
		State  _new = States[new_state];
		if (_new != null)
		{
			current_state.Exit();
			current_state = _new;
			_new.Enter();
		}

	}

	public override void _Process(double delta)
	{
		current_state.Process(delta);
	}
    public override void _PhysicsProcess(double delta)
    {
        current_state.PhysicsProcess(delta);
    }
	public override void _Input(InputEvent @event)
	{
		current_state.Inp(@event);
	}
}
