extends Timer

@onready var anim : AnimationPlayer = %anim
@onready var voin : CharacterBody2D = $".."
var idle : bool = false
func _on_voin_null_ammo() -> void:
	start()
	pass # Replace with function body.


func _on_timeout() -> void:
	anim.play("idle")
	idle = true
	pass # Replace with function body.

func _process(_delta: float) -> void:
	if(idle and voin.velocity.length() <= 0 and not anim.is_playing()):
		anim.play("idle")
		
		
	
