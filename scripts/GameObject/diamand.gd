extends Area2D
class_name Diamand
var id : int
var n_level
const path_json : String = "user://player_data.json"
func _ready() -> void:
	var parent : Kristal_Level = get_parent()
	n_level = parent.index_level


func _on_body_entered(body: Node2D) -> void:
	if(body.is_in_group("unit")):
		G.diamand_balance += 1
		queue_free()
	
	pass # Replace with function body.
