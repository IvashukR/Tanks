using Godot;
using System;

namespace GameObjects;
public partial class ViewMap : Camera2D
{
    [Export] private Vector2 limit_x;
    [Export] private float speed;
    [Export] private float zoom_speed;
    private Vector2 last_mouse_pos;
    private bool draging;
    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                draging = mouseEvent.Pressed;
                last_mouse_pos = mouseEvent.Position;
            }
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                Zoom *= 1 - zoom_speed;
            }
            if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                Zoom *= 1 + zoom_speed;
            }
        }
        
    }
    
    public override void _Process(double _delta)
    {
        if(draging)
        {
            var mouse_pos = GetViewport().GetMousePosition();
            var delta_move = last_mouse_pos - mouse_pos;
            last_mouse_pos = mouse_pos;
            Position += delta_move * speed;
        }
    }
}

