using Godot;
using System;


namespace Enemy.Transport;


public partial class TownEnemyHpBar : ProgressBar
{
    [Export] private TowerEnemy tower;
    public override void _Ready()
    {
        tower.change_hp += change_value;
        tower.Ready += () => 
        {
            MaxValue = tower._logic.max_proch;
            Value = tower._logic.proch;
        };
    }
    private void change_value()
    {
        Value = tower._logic.proch;
    }
    
    public override void _ExitTree()
    {
        tower.change_hp -= change_value;
    }
}
