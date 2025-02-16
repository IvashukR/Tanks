using Godot;
using System;

public partial class TownEnemyHpBar : ProgressBar
{
    private TownEnemy enemy_town;
    public override void _Ready()
    {
        enemy_town = GetNode<TownEnemy>("%town_enemy");
        GlobalManager.Instance.destroyed_enemy_town += change_value;
        enemy_town.Ready += () => 
        {
            MaxValue = enemy_town.max_proch;
            Value = enemy_town.proch;
            GD.Print(Value, MaxValue, enemy_town.proch);
        };
    }
    private void change_value()
    {
        Value = enemy_town.proch;
        GD.Print(Value, MaxValue, enemy_town.proch);
    }
    public override void _ExitTree()
    {
        GlobalManager.Instance.destroyed_enemy_town -= change_value;
    }
}
