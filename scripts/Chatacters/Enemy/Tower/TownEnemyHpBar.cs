using Godot;
using System;

<<<<<<< HEAD
namespace Enemy.Transport;
=======
namespace Enemy.Tower;
>>>>>>> a5c0e3187446ca1006d4a127187244c240887b08
public partial class TownEnemyHpBar : ProgressBar
{
    private TownEnemy enemy_town;
    public override void _Ready()
    {
        enemy_town = GetNode<TownEnemy>("%town_enemy");
        GlobalManager.Instance.destroyed_town += change_value;
        enemy_town.Ready += () => 
        {
            MaxValue = enemy_town.max_proch;
            Value = enemy_town.proch;
        };
    }
    private void change_value(Node2D node)
    {
        if(node == GetParent())Value = enemy_town.proch;
    }
    public override void _ExitTree()
    {
        GlobalManager.Instance.destroyed_town -= change_value;
    }
}
