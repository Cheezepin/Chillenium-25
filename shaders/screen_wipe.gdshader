shader_type canvas_item;

uniform float progress;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	if(progress < 1.0) {if(UV.x > progress) COLOR = vec4(0,0,0,0);}
	else 			   {if(UV.x < progress-1.0) COLOR = vec4(0,0,0,0);}
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
