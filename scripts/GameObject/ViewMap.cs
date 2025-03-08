using Godot;
using System;

public partial class ViewMap : Camera2D
{
    [Export] private Vector2 limit;
    [Export] private float shappiness;

    public override void _Ready()
    {

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouse_event)
        {
            if (mouse_event.ButtonIndex == MouseButton.Left)
            {
                
            }
        }
    }
    public override void _Process(double _delta)
    {
        if(Input.IsMouseButtonPressed(MouseButton.Left))
        {
            var mouse_velocity = Input.GetLastMouseVelocity();
            if (mouse_velocity.Length() > 0)
            {
                GD.Print("MOVE");
                //float new_pos_x;
                //if(mouse_velocity.X < 0)new_pos_x  = Position.X - shappiness;
                //else new_pos_x  = Position.X + shappiness;
                //Position = new Vector2(Mathf.Clamp(new_pos_x, limit.X, limit.Y), Position.Y);
                Position += new Vector2(-mouse_velocity.X * shappiness, 0);
                Position = new Vector2(Mathf.Clamp(Position.X, limit.X, limit.Y), Position.Y);
            }
                
        }
    }
}

