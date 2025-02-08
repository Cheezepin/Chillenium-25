shader_type spatial;
render_mode world_vertex_coords;
uniform float snap = 10.0;

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX = vec3(round(VERTEX.x*snap)/snap, round(VERTEX.y*snap)/snap, round(VERTEX.z*snap)/snap);
}

void fragment() {
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
