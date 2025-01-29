using Godot;
public partial class Phone : ColorRect
{
	public AnimationPlayer anim_phone;
	public override void _Ready()
	{
		anim_phone = GetNode<AnimationPlayer>("%anim_phone");
	}
}
