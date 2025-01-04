using Godot;
using System;

public partial class Voin : CharacterBody2D
{
	public int patron_count = 10;
	public float perezaryad = 0.5f;
	public int damage = 5;
	public int proch = 20;
	public float speed = 100;
	public int cost = 100;
	public int cost_death = 200;
	public Texture2D texture_card = (Texture2D)ResourceLoader.Load("res://textures/card_voinpng.png");

}
