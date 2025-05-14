class_name TrailsTank
extends Node2D

@export var actor : CharacterBody2D
@onready var t : Timer = $Timer

var list_trails : Array = []
var flag_move : bool = false
var flag_hide : bool = false

const MIN_DELAY_T : float = 0.6
const MAX_DELAY_T : float = 5


func _ready() -> void:
	for i in get_children():
		if i is Timer:
			break
		list_trails.append(i)
	pass
	
func update_timer_delay(speed) -> void:
	var delay = clamp(MAX_DELAY_T - (speed * 0.15), MIN_DELAY_T, MAX_DELAY_T)
	t.wait_time = delay
	
func _process(_delta: float) -> void:
	if !flag_hide and get_child_count() < 4:
		flag_hide = true
		hide_trails()
	
		
func _physics_process(_delta: float) -> void:
	var speed = actor.velocity.length()
	if speed > 0 and not flag_move:
		flag_move = true
		t.start()
	update_timer_delay(speed)
func show_trail() -> void:
	if get_child_count() == 0:
		return
	get_child(0).reparent(get_tree().current_scene)

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
