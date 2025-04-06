extends Node2D
@export var actor : CharacterBody2D
@onready var t : Timer = $Timer
@onready var trail1 = $trail
var list_trails : Array = []
var flag_move : bool = false
func _ready() -> void:
	for i in get_children():
		if i is Timer:
			break
		list_trails.append(i)
	pass
	
func _physics_process(_delta: float) -> void:
	if actor.velocity.length() > 0 and not flag_move:
		flag_move = true
		t.start()
func s():
	if get_child_count() == 0:
		for i : Sprite2D in list_trails:
			i.reparent(self, false)
			i.global_position = global_position
			i.global_rotation = global_rotation
			await get_tree().create_timer(0.5).timeout
			
		return
	get_child(0).reparent(get_tree().root)


func _on_timer_timeout() -> void:
	if actor.velocity.length() > 0:
		s()
	else:
		flag_move = false
		t.stop()
	pass 
