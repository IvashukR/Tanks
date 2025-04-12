using Godot;
using System;


namespace GameObjects;
public partial class Bomba : Sprite2D
{
    private PathFollow2D path_move;
    private Area2D area_collide;
    [Export] private float _speed;
    public override void _Ready()
    {
        path_move = GetNode<PathFollow2D>("%path_move");
        area_collide = GetNode<Area2D>("%area_collide");
        area_collide.BodyEntered += AreaCollideEntered;
        path_move.Progress = GD.RandRange(20, 360);
        
    }
    public override void _Process(double delta)
    {
        if(!GetTree().Paused)
        path_move.Progress += _speed;
    }
    private void AreaCollideEntered(Node2D body)
    {
        if(body is IDamageble actor)
        {
            actor.TakeDamage(1000);
        }
    }
}
