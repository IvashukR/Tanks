using Godot;
using System;

public partial class ViewMap : Camera2D
{
    [Export] private Vector2 limit;
    [Export] private float shappiness;
    public override void _Process(double _delta)
    {
        if(Input.IsMouseButtonPressed(MouseButton.Left))
        {
            var mouse_velocity = Input.GetLastMouseVelocity();
            if (mouse_velocity.Length() > 0)
            {
                Position += new Vector2(-mouse_velocity.X * shappiness, 0);
                Position = new Vector2(Mathf.Clamp(Position.X, limit.X, limit.Y), Position.Y);
            }
        }
    }
}

