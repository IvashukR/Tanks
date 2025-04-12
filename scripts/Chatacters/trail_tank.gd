extends Node2D
@export var actor : CharacterBody2D
@onready var t : Timer = $Timer
var list_trails : Array = []
var flag_move : bool = false
var flag_hide : bool = false
func _ready() -> void:
	for i in get_children():
		if i is Timer:
			break
		list_trails.append(i)
	pass

func _process(_delta: float) -> void:
	if !flag_hide and get_child_count() < 4:
		flag_hide = true
		hide_trails()
		pass
func _physics_process(_delta: float) -> void:
	if actor.velocity.length() > 0 and not flag_move:
		flag_move = true
		t.start()
func show_trail() -> void:
	if get_child_count() == 0:
		return
	get_child(0).reparent(get_tree().root)

func hide_trails() -> void:
	for i : Sprite2D in list_trails:
		i.reparent(self, false)
		i.global_position = global_position
		i.global_rotation = global_rotation
		await get_tree().create_timer(0.5).timeout
	flag_hide = false

func _on_timer_timeout() -> void:
	if actor.velocity.length() > 0:
		show_trail()
	else:
		flag_move = false
		t.stop()
	pass 

func _exit_tree() -> void:
	for node: Node in get_tree().root.get_children():
		if node.is_in_group("trail"):
			node.queue_free()
