using Godot;
using System;

namespace GameUnit;
[GlobalClass]
public partial class UnitStats : Resource
{
    [Export] public int patron_count;
	[Export] public float perezaryad;
	[Export] public int damage;
	[Export] public int proch;
	[Export] public float speed;
	[Export] public int cost;
	[Export] public int cost_death;
	[Export] public int iq_ai;
}
