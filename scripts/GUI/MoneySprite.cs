using Godot;
using System;

public partial class MoneySprite : TextureRect
{
    private Label label_money;
    private Level1 level;
    private AudioStreamPlayer audio;
    [Export] private int count_money;
    private int money;
    private AnimationPlayer anim_money_havent;
    public override void _Ready()
    {
        anim_money_havent = GetNode<AnimationPlayer>("%anim_money_havent");
        money = count_money;
        GlobalManager.Instance.money = money;
        label_money = GetNode<Label>("%money_l");
        audio = GetNode<AudioStreamPlayer>("%audio");
        level = (Level1)GetTree().CurrentScene;
        level.Start += ShowMoney;
        GlobalManager.Instance.change_money += ChangeMoney;
        GlobalManager.Instance.havent_money += HaventMoney;
        label_money.Text = $"{money}";
    }
    private void ShowMoney() => Show();
    private void HaventMoney()
    {
        if(!anim_money_havent.IsPlaying())anim_money_havent.Play();
    }
    private async void ChangeMoney(int money)
    {
        this.money -= money;
        GlobalManager.Instance.money = this.money;
        if(this.money < 0)return;
        label_money.Text = $"{this.money}";
        audio.Play();
        await ToSignal(audio, AudioStreamPlayer.SignalName.Finished);
    }
    public override void _ExitTree()
    {
        level.Start -= ShowMoney;
        GlobalManager.Instance.havent_money -= HaventMoney;
    }
}

