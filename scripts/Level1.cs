using Godot;
using System;

public partial class Level1 : Trenirovka
{
	
	
	public override void _Ready()
	{
		s =  (PackedScene)ResourceLoader.Load("res://scene/level.tscn");
		inp = "Покупай юнитов за деньги , за каждого убитого вражеского юнита будут прибавлятся деньги а за своего уменьшатся зависимости от стоимости юнита.Уничтожь вражескую башню ,удачи!.";
		base._Ready();
	}

	
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
