extends Area2D
class_name Diamand
var id : int
var n_level
var parent : Kristal_Level
const path_json : String = "user://player_data.json"
var collected : bool
func _ready() -> void:
	call_deferred("_init_index")
	call_deferred("load")

func _on_body_entered(body: Node2D) -> void:
	if(body.is_in_group("unit")):
		G.diamand_balance += 1
		collected = true
		queue_free()
	
	pass
func _init_index() -> void:
	parent = get_parent()
	n_level = parent.index_level
func save() -> void:
	var data = {}
	var file
	if not FileAccess.file_exists((path_json)):
		file = FileAccess.open(path_json, FileAccess.ModeFlags.WRITE)
		data = {
			"diamand balance": str(G.diamand_balance),
			"levels":{
				str(n_level):{
					"diamand":{
						id : collected
					}
				}
			}
		}
	else:
		file = FileAccess.open(path_json, FileAccess.ModeFlags.READ)
		if file:
			var content = file.get_as_text()
			var _data = JSON.parse_string(content)
			file.close()
			var levels = _data["levels"]
			if not levels.has(str(n_level)):
				levels[str(n_level)] = {
					"diamand":{
						id : collected
					}
				}
			
			else:
				_data["levels"][str(n_level)]["diamand"][str(id)] = collected
			file = FileAccess.open(path_json, FileAccess.WRITE)
			data = _data
	
	file.store_string(JSON.stringify(data, "\t"))
	pass
func _exit_tree() -> void:
	save()
func get_load_data() -> Dictionary:
	var file = FileAccess.open(path_json, FileAccess.READ)
	var data_text = file.get_as_text()
	var data = JSON.parse_string(data_text)
	file.close()
	return data
func parse_data_loader(data : Dictionary) -> void:
	var levels = data["levels"]
	var diamand = levels[str(n_level)]["diamand"]
	if diamand[str(id)]:
		queue_free()
func load() -> void:
	if not FileAccess.file_exists((path_json)):
		return
	parse_data_loader(get_load_data())
	
	
	
