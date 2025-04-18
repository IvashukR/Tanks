using GameObjects;
using Godot;
using System;
using TanksUtilits;


namespace Enemy.Transport;
public partial class TankEnemy : CharacterBody2D, ITower
{
    private NavigationAgent2D agent;
    private Timer t_patrul, t_attack;
    private Vector2 direction;
    private PathFollow2D path_patrul;
    [Export] private float speed, speed_patrul;
    [Export] private PhysicsBody2D target;
    [Export] public TowerLogicShoot logic {set;get;}
    private bool attack = true;
    private State current_state = State.Patrul;
    private Area2D triger_area;

    private enum State
    {
        Patrul,
        Follow,
        Targeting
    }
    [Export] public RayCast2D ray_attack {set;get;}
    public override void _Ready()
    {
        agent = GetNode<NavigationAgent2D>("%agent");
        triger_area = GetNode<Area2D>("triger_area");
        t_patrul = GetNode<Timer>("%t_patrul");
        t_attack = GetNode<Timer>("%t_attack");
        path_patrul = GetParent<PathFollow2D>();
        t_attack.Start();
        t_patrul.Start();
        SetRandomDirection();
        //t_attack.Timeout += Attack;
        //t_patrul.Timeout += SetRandomDirection;
        triger_area.BodyEntered += EnteredTrigerArea;
    }
    public override void _PhysicsProcess(double delta)
    {
        if(current_state != State.Patrul)
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
        if(current_state == State.Patrul)
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
    public virtual void TakeDamage(int damage)
	{
		logic.TakeDamage(damage);
	}
    
    public virtual void Shoot()
    {
        logic.Shoot();
    }
    private void EnteredTrigerArea(Node2D body)
    {
        GamaUtilits.EnteredBulletInTownZone(body, this);
    }

}
