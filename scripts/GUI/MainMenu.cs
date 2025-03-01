using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
    private Timer timer_pig_spawn;
    private PackedScene pig = (PackedScene)ResourceLoader.Load("res://scene/pig_main_menu_move.tscn");
    private int pig_count = 0;
    [Export] private int max_pig;
    private TextureButton level_btn;
    private AnimationPlayer anim_btn;
    private AnimationPlayer anim_btn2;
    private TextureButton setting_btn;
    private PackedScene setting_menu = (PackedScene)ResourceLoader.Load("res://scene/setting_window.tscn");

    private Phone background;
    public override void _Ready()
    {
        background = GetNode<Phone>("%Phone");
        anim_btn = GetNode<AnimationPlayer>("%anim_btn");
        level_btn = GetNode<TextureButton>("%Level");
        anim_btn2 = GetNode<AnimationPlayer>("%anim_btn2");
        setting_btn = GetNode<TextureButton>("%setting_btn");
        timer_pig_spawn = GetNode<Timer>("%t_pig");
        SpawnPig();
        timer_pig_spawn.Start();
        timer_pig_spawn.Timeout += SpawnPig;
        setting_btn.Pressed += async () =>
        {
            await PlayingAnimAsync(anim_btn2, "a");
            var inst = setting_menu.Instantiate<Control>();
            AddChild(inst);
            
        };
        level_btn.Pressed += async () =>
        {
            await PlayingAnimAsync(anim_btn, "a");
            await PlayingAnimAsync(background.anim_phone, "close");
            GetTree().ChangeSceneToFile("res://scene/trenirovka.tscn");
        };
    }
    private async Task PlayingAnimAsync(AnimationPlayer anim, string anim_name)
    {
        if(!anim.IsPlaying())anim.Play(anim_name);
        await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
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
