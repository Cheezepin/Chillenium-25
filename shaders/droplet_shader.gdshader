shader_type canvas_item;

uniform bool flash;
uniform bool trans;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	if(flash && COLOR.a > 0.0) COLOR = vec4(1,1,1,1);
	if(trans/* && (COLOR.r > 0.05 || COLOR.g > 0.05 || COLOR.b > 0.05)*/) {
		COLOR *= vec4(1,1,1,0.25);
	}
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
