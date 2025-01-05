using Godot;
using System;

public partial class Voin : CharacterBody2D, IStats
{
	public int patron_count { get; set; } = 10;
	public float perezaryad { get; set; } = 0.5f;
	public int damage { get; set; } = 5;
	public int proch { get; set; } = 20;
	public float speed { get; set; } = 100;
	public int cost { get; set; } = 100;
	public int cost_death { get; set; } = 120;

}
