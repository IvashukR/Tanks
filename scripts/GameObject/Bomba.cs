using Godot;
using System;
using GameUnit;

namespace GameObjects;
public partial class Bomba : StaticBody2D
{
    private PathFollow2D path_move;
    private Area2D area_collide;
    private AnimationPlayer anim;
    [Export] private float speed;
    public override void _Ready()
    {
        path_move = GetNode<PathFollow2D>("%path_move");
        anim = GetNode<AnimationPlayer>("anim");
        area_collide = GetNode<Area2D>("%area_collide");
        area_collide.BodyEntered += AreaCollideEntered;
    }
    public override void _Process(double delta)
    {
        path_move.Progress += speed;
    }
    private void AreaCollideEntered(Node2D body)
    {
        if(!anim.IsPlaying())anim.Play();
        if(body.GetNodeOrNull("%logic") is UnitLogic unit)
        {
            using(var bullet_damage = new Bullet(1000))
            {
                unit.TakeDamage(bullet_damage);
            }
            
        }
    }
}
