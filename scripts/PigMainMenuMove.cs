using Godot;

public partial class PigMainMenuMove : RigidBody2D
{
    private Vector2 velocity;
    private AnimationPlayer anim_player;
    private bool collide_flag;
    private Timer t_flag;
    [Export] private int speed;
    private AnimatedSprite2D anim_sheets;
    public override void _Ready()
    {
        t_flag = GetNode<Timer>("%t_flag");
        anim_player = GetNode<AnimationPlayer>("%anim_player");
        anim_sheets = GetNode<AnimatedSprite2D>("%anim_sheets");
        anim_sheets.Play();
        t_flag.Timeout += () => collide_flag = true;
        Position = new Vector2(GD.RandRange(70, 1000), GD.RandRange(70, 500));
        SetLinearVelocity();
        GravityScale = 0;
        GlobalManager.Instance.pig_main_menu_anim += AnimDeath;
    }
    private async void AnimDeath()
    {
        anim_player.Play("death");
        await ToSignal(anim_player, "animation_finished");
        QueueFree();
    }
    public override void _PhysicsProcess(double delta)
    {
        if(GetCollidingBodies().Count > 0 && collide_flag)
        {
            collide_flag = false;
            t_flag.Start();
            GlobalManager.Instance.EmitSignal("pig_main_menu_audio");
            SetLinearVelocity();
        }
    }
    private void SetLinearVelocity()
    {
        velocity = (new Vector2(GD.RandRange(-200, 200), GD.RandRange(-200, 200))).Normalized() * speed;
        LinearVelocity = velocity;
    }
    public override void _ExitTree()
    {
        GlobalManager.Instance.pig_main_menu_anim -= AnimDeath;
    }
}
