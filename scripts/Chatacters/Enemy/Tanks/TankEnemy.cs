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
    private PathFollow2D path_patrul;
    [Export] private float speed, speed_patrul;
    [Export] private PhysicsBody2D target;
    private bool attack = false;
    public override void _Ready()
    {
        agent = GetNode<NavigationAgent2D>("%agent");
        pushka = GetNode<Sprite2D>("%bashnya");
        osnova = GetNode<Sprite2D>("%osnova");
        t_patrul = GetNode<Timer>("%t_patrul");
        t_attack = GetNode<Timer>("%t_attack");
        path_patrul = GetParent<PathFollow2D>();
        //t_attack.Start();
        //t_patrul.Start();
        SetRandomDirection();
        t_attack.Timeout += Attack;
        t_patrul.Timeout += SetRandomDirection;
    }
    public override void _PhysicsProcess(double delta)
    {
        if(attack)
        {
            var next_pos = agent.GetNextPathPosition();
            direction = GlobalPosition.DirectionTo(next_pos).Normalized();
        }
        Velocity = direction * speed * (float)delta;
        var angle_target = direction.Angle();
        Rotation = Mathf.LerpAngle(Rotation, angle_target, (float)delta * 0.4f);
        MoveAndSlide();
    }
    public override void _Process(double delta)
    {
        path_patrul.Progress += speed_patrul;
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
        var new_direction = new Vector2((float)GD.RandRange(GlobalPosition.X - 30, GlobalPosition.X + 130), 
        (float)GD.RandRange(GlobalPosition.Y - 100, GlobalPosition.Y + 130));
        var distance = new_direction - GlobalPosition;
        if(distance.Length() < 1)return;
        SetTarget(new_direction);
    }
    
}
