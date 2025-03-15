using Godot;

namespace GameObjects;
public partial class PigMainMenuMove : RigidBody2D
{
    private Vector2 velocity;
    private AnimationPlayer anim_player;
    private bool collide_flag = true;
    private Timer t_flag;
    [Export] private int speed;
    private AnimatedSprite2D anim_sheets;
    private AudioStreamPlayer audio;
    private AudioStreamPlayer audio2;
    public override void _Ready()
    {
        audio = GetNode<AudioStreamPlayer>("%audio");
        audio2 = GetNode<AudioStreamPlayer>("%audio2");
        t_flag = GetNode<Timer>("%t_flag");
        anim_player = GetNode<AnimationPlayer>("%anim_player");
        anim_sheets = GetNode<AnimatedSprite2D>("%anim_sheets");
        anim_sheets.Play();
        t_flag.Timeout += () => collide_flag = true;
        Position = new Vector2(GD.RandRange(70, 1000), GD.RandRange(70, 500));
        SetLinearVelocity();
        GravityScale = 0;
        GlobalManager.Instance.pig_main_menu_anim += AnimDeath;
        BodyEntered += (body) => PhysicsProcess();
    }
    private async void AnimDeath()
    {
        anim_player.Play("death");
        audio2.Play();
        await ToSignal(anim_player, "animation_finished");
        QueueFree();
    }
    public  void PhysicsProcess()
    {
        if(collide_flag)
        {
            if(!audio.IsPlaying())audio.Play();
            collide_flag = false;
            t_flag.Start();
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
