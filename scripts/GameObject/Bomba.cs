using Godot;
using System;
using TanksUtilits;

namespace GameObjects;
public partial class Bomba : Sprite2D
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
        if(body.IsInGroup("unit"))
        {
            if(!anim.IsPlaying())anim.Play("1");
            using(var bullet_damage = new Bullet(1000))
            {
                GamaUtilits.TakeDamageUnit(body, bullet_damage);
            }
            
        }
        else if(body.IsInGroup("bullet"))
        if(!anim.IsPlaying())anim.Play("1");
    }
}
