using Godot;
using System;

public partial class MainMenu : Control
{
    private Timer timer_pig_spawn;
    private PackedScene pig = (PackedScene)ResourceLoader.Load("res://scene/pig_main_menu_move.tscn");
    private int pig_count = 0;
    [Export] private int max_pig;
    private TextureButton level_btn;
    private AnimationPlayer anim_btn;
    private Phone background;
    public override void _Ready()
    {
        background = GetNode<Phone>("%Phone");
        anim_btn = GetNode<AnimationPlayer>("%anim_btn");
        level_btn = GetNode<TextureButton>("%Level");
        timer_pig_spawn = GetNode<Timer>("%t_pig");
        SpawnPig();
        timer_pig_spawn.Start();
        timer_pig_spawn.Timeout += SpawnPig;
        level_btn.Pressed += async () =>
        {
            anim_btn.Play("a");
            await ToSignal(anim_btn, AnimationPlayer.SignalName.AnimationFinished);
            background.anim_phone.Play("close");
            await ToSignal(background.anim_phone, AnimationPlayer.SignalName.AnimationFinished);
            GetTree().ChangeSceneToFile("res://scene/trenirovka.tscn");
        };
    }
    private void SpawnPig()
    {
        if(pig_count > max_pig)
        {
            GlobalManager.Instance.EmitSignal("pig_main_menu_anim");
            pig_count = 0;
            return;
        }
        AddChild(pig.Instantiate());
        pig_count++;
    }
}
