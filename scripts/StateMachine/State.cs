using Godot;
using System;

public partial class State : Node
{
	
	public  virtual void Enter()
	{
	}
	public  virtual void Exit()
	{
	}
	public  virtual void PhysicsProcess(double delta)
	{
	}

	
	public  virtual void Process(double delta)
	{
	}
	public virtual void Inp(InputEvent @event)
	{
	}
	public virtual void _Inp(InputEvent @event)
	{
	}
}
