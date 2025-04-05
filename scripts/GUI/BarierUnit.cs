using Godot;


namespace GameView;
public partial class BarierUnit : Area2D
{
    public override void _Process(double delta)
    {
        Visible = GlobalManager.Instance.temp_pick_unit != null;
    }

}
