shader_type canvas_item;
uniform sampler2D dotted_tex;
uniform vec4 target_color  : source_color = vec4(1.0);
uniform vec4 current_color : source_color = vec4(1.0);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	if(target_color != current_color) {COLOR = texture(dotted_tex, UV, 0);}
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
