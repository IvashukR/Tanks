using Godot;
using System;

namespace GameView;
public partial class ChooseLevel : Control
{

    private Phone ph;
    private AnimationPlayer anim;
    public override void _Ready()
    {
        ph =  GetNode<Phone>("%Phone");
        anim = GetNode<AnimationPlayer>("%anim");
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
                int local_index = index;
                btn.Pressed += async () =>
                {
                    GD.Print(local_index);
                    if(GlobalManager.Instance.last_level >= local_index)
                    {
                        ph.Show();
                        ph.anim_phone.Play("close");
                        await ToSignal(ph.anim_phone, AnimationPlayer.SignalName.AnimationFinished);
                        GetTree().ChangeSceneToFile(GlobalManager.Instance.PathLevels[local_index]);
                    }
                    else
                    {
                        if(!anim.IsPlaying())anim.Play("start");
                    }
                };
                index++;
            }
        }
    }
    
    
}
