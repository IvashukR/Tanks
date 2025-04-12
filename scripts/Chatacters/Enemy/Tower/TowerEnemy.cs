using Godot;
using GameObjects;


namespace Enemy.Transport;

public partial class TowerEnemy : StaticBody2D, IDamageble
{
    [Export] public  BaseTowerLogic _logic  {set;get;}
	private Node2D parent_logic;
	[Signal]
	public  delegate void change_hpEventHandler();


	public virtual void TakeDamage(int damage)
	{
		_logic.TakeDamage(damage);
		EmitSignal("change_hp");
	}
}
