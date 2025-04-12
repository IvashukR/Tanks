using Godot;
using GameObjects;

public interface ITower : IDamageble
{
    public TowerLogicShoot logic {set;get;}
    public void Shoot(){}
    public RayCast2D ray_attack {set;get;}
}