using Godot;
using System;
using System.Threading.Tasks;

public partial class ResumeMenu : ColorRect
{
    private TextureButton turn_menu;
    private TextureButton restart_btn;
    public Label fail_l;
    public Label pause_l;
    private TextureButton next_btn;
    public int index_next_level;
    public override void _Ready()
    {
        fail_l = GetNode<Label>("%fail_l");
        next_btn = GetNodeOrNull<TextureButton>("%next_btn");
        pause_l = GetNode<Label>("%pause_l");
        turn_menu = GetNode<TextureButton>("%turn_menu");
        restart_btn = GetNode<TextureButton>("%restart_btn");
        if(next_btn != null)
        {
            next_btn.Pressed += () => NextLevel(index_next_level);
        }
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
    private void NextLevel(int index) => GetTree().ChangeSceneToFile(GlobalManager.Instance.PathLevels[index]);
    private async Task phone_c()
    {
        GetNode<Phone>("%Phone").Show();
        GetNode<Phone>("%Phone").anim_phone.Play("close");
        await ToSignal(GetNode<Phone>("%Phone").anim_phone, "animation_finished");
        GetTree().Paused = false;
    }
}
