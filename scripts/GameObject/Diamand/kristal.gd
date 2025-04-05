extends Node
class_name Kristal_Level
@export var index_level : float
func _ready() -> void:
	var index : int = 0
	for i : Diamand in get_children():
		i.id = index
		index += 1
	pass
