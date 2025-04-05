using Godot;
using System;


namespace Enemy.Transport;
public partial class TankEnemy : CharacterBody2D
{
    private NavigationAgent2D agent;
    private Timer t_patrul;
    private Vector2 direction;
    public Sprite2D pushka { get; set; }
    private Sprite2D  osnova;
    [Export] private float speed;
    [Export] private float rotation_time;
    public override void _Ready()
    {
        agent = GetNode<NavigationAgent2D>("%agent");
        pushka = GetNode<Sprite2D>("%bashnya");
        osnova = GetNode<Sprite2D>("%osnova");
        t_patrul = GetNode<Timer>("%t_patrul");
        t_patrul.Start();
        t_patrul.Timeout += SetRandomDirection;
    }
    public override void _PhysicsProcess(double delta)
    {
        Velocity = direction * speed * (float)delta;
        MoveAndSlide();
    }
    private async void SetRandomDirection()
    {
        direction = Vector2.Zero;
        var new_direction = new Vector2(GD.RandRange(15, 230), GD.RandRange(15, 230));
        var distance = new_direction - GlobalPosition;
        if(distance.Length() < 1)return;
        direction = distance.Normalized();
        var target_angle = distance.Angle();
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(this, "rotation", target_angle , rotation_time);
        await ToSignal(tween, "finished");
        direction = distance.Normalized();
    }
    
}
