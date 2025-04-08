using Godot;
using System;


namespace Enemy.Transport;
public partial class TankEnemy : CharacterBody2D
{
    private NavigationAgent2D agent;
    private Timer t_patrul, t_attack;
    private Vector2 direction;
    public Sprite2D pushka { get; set; }
    private Sprite2D  osnova;
    [Export] private float speed;
    [Export] private PhysicsBody2D target;
    private bool attack = false;
    public override void _Ready()
    {
        agent = GetNode<NavigationAgent2D>("%agent");
        pushka = GetNode<Sprite2D>("%bashnya");
        osnova = GetNode<Sprite2D>("%osnova");
        t_patrul = GetNode<Timer>("%t_patrul");
        t_attack = GetNode<Timer>("%t_attack");
        t_attack.Start();
        t_patrul.Start();
        SetRandomDirection();
        t_attack.Timeout += Attack;
        t_patrul.Timeout += SetRandomDirection;
    }
    public override void _PhysicsProcess(double delta)
    {
        var next_pos = agent.GetNextPathPosition();
        direction = GlobalPosition.DirectionTo(next_pos);
        Velocity = direction * speed * (float)delta;
        var angle_target = direction.Angle();
        Rotation = Mathf.LerpAngle(Rotation, angle_target, (float)delta * 0.4f);
        MoveAndSlide();
    }
    private void Attack()
    {
        SetTarget(target.GlobalPosition);
        
    }
    private void SetTarget(Vector2 target_pos)
    {
        agent.TargetPosition = target_pos;
    }
    private void SetRandomDirection()
    {
        direction = Vector2.Zero;
        var new_direction = new Vector2(GD.RandRange(15, 230), GD.RandRange(15, 230));
        var distance = new_direction - GlobalPosition;
        if(distance.Length() < 1)return;
        SetTarget(new_direction);
    }
    
}
