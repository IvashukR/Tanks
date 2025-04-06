extends Node2D
@export var actor : CharacterBody2D
@onready var t : Timer = $Timer
var flag_move : bool = false
func _ready() -> void:
	pass
	
func _physics_process(_delta: float) -> void:
	if actor.velocity.length() > 0 and not flag_move:
		flag_move = true
		t.start()
func s():
	if get_child_count() == 0:
		t.stop()
		return
	get_child(0).reparent(get_tree().root)


func _on_timer_timeout() -> void:
	if actor.velocity.length() > 0:
		s()
	pass 
