using Godot;
using System;

public partial class ChooseLevel : Control
{

    private Phone ph;
    public override void _Ready()
    {
        ph =  GetNode<Phone>("Phone");
        ph.anim_phone.Play("open");
        ph.anim_phone.AnimationFinished += (animationName) => ph.Hide();
        SetHandlersBtnLevel();
    }
    private void SetHandlersBtnLevel()
    {
        int index = 0;
        foreach(var child in GetChildren())
        {
            if(child is TextureButton btn)
            {
                if(GlobalManager.Instance.last_level < index)
                btn.TextureNormal = (Texture2D)ResourceLoader.Load("res://textures/levels_btn_locked.png");
                btn.Pressed += () =>
                {
                    if(GlobalManager.Instance.last_level >= index)
                    {
                        ph.Show();
                        ph.anim_phone.Play("close");
                        GetTree().ChangeSceneToFile(GlobalManager.Instance.PathLevels[index]);
                    }
                };
                index++;
            }
        }
    }
    
    public override void _ExitTree()
    {
        
    }
}
