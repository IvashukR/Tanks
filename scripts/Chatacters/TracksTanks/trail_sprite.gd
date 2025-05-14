class_name TrailSprite
extends Sprite2D


func _ready() -> void:
	get_parent().tree_exited.connect(_exit_tree_tank)
	
	pass
func _exit_tree_tank() -> void:
	if get_parent() is Node2D:
		return
	if !is_inside_tree():
		return
	await get_tree().create_timer(1.5).timeout
	var tween = get_tree().create_tween()
	tween.set_trans(Tween.TRANS_LINEAR)
	tween.set_trans(Tween.TRANS_SINE)
	tween.tween_property(self, "modulate:a", 0, 1)
	pass
