using Godot;
using System;
using System.Threading.Tasks;

public partial class ResumeMenu : ColorRect
{
    private TextureButton turn_menu;
    private TextureButton restart_btn;
    public override void _Ready()
    {
        turn_menu = GetNode<TextureButton>("%turn_menu");
        restart_btn = GetNode<TextureButton>("%restart_btn");
        turn_menu.Pressed += async () =>
        {
            await phone_c();
            GetTree().ChangeSceneToFile("res://scene/main_menu.tscn");
        };
        restart_btn.Pressed += async () =>
        {
            await phone_c();
            GetTree().ReloadCurrentScene();
        };

        
    }
    private async Task phone_c()
    {
        GetNode<Phone>("%Phone").Show();
        GetNode<Phone>("%Phone").anim_phone.Play("close");
        await ToSignal(GetNode<Phone>("%Phone").anim_phone, "animation_finished");
        GetTree().Paused = false;
    }
}
