using Godot;
using System;

public partial class BarierUnit : Area2D
{
    public override void _Ready()
	{
		BodyEntered += (body) =>
        {
            if(body is IStats unit)
            {
                if(unit.fsm.current_state.Name == "Void")body.QueueFree();
            }

        };
	}
    public override void _Process(double delta)
    {
        if(GlobalManager.Instance.temp_pick_unit != null)Visible = true;
        else Visible = false;
    }

}
