shader_type canvas_item;

uniform bool flash;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 uv = UV;
	
	// Called for every pixel the material is visible on.
	
	uv.x += sin(5.0*TIME+floor(uv.y*50.0)*.02*10.0)*0.05;
	
	COLOR = texture(TEXTURE, uv);
	COLOR *= vec4(1,1,2.0,1);
	
	if(flash && COLOR.a > 0.0) COLOR = vec4(1,1,1,1);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
