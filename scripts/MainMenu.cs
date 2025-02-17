using Godot;
using System;

public partial class MainMenu : Control
{
    private Timer timer_pig_spawn;
    private PackedScene pig = (PackedScene)ResourceLoader.Load("res://scene/pig_main_menu_move.tscn");
    private int pig_count = 0;
    [Export] private int max_pig;
    public override void _Ready()
    {
        timer_pig_spawn = GetNode<Timer>("%t_pig");
        SpawnPig();
        timer_pig_spawn.Start();
        timer_pig_spawn.Timeout += SpawnPig;
    }
    private void SpawnPig()
    {
        if(pig_count > max_pig)
        {
            GlobalManager.Instance.EmitSignal("pig_main_menu_anim");
            GlobalManager.Instance.EmitSignal("pig_main_menu_audio");
            pig_count = 0;
            return;
        }
        AddChild(pig.Instantiate());
        pig_count++;
    }
}
