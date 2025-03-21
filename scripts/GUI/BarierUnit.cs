using Godot;
using System;
using GameUnit;

namespace GameView;
public partial class BarierUnit : Area2D
{
    public override void _Ready()
	{
		BodyEntered += (body) =>
        {
            if(!body.IsInGroup("unit"))return;
            if(body.GetNodeOrNull("logic") is UnitLogic unit)
            {
                if(unit.fsm.current_state.Name == "Void")body.QueueFree();
                GlobalManager.Instance.EmitSignal("cant_pick_unit");
            }

        };
	}
    public override void _Process(double delta)
    {
        if(GlobalManager.Instance.temp_pick_unit != null)Visible = true;
        else Visible = false;
    }

}
