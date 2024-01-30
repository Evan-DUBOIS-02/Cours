vec3 sample_hemisphere(float roughness)
{
	// Z <- rand entre ? et ?
	float Z = random_float();
	// beta <- Z : angle/plan_xy
	float beta = acos(pow(Z, 1.0-roughness));
	// alpha <- rand entre ? et ?
	float alpha = 2.0 * PI * random_float();
	// x,y,z <- alpha,beta : coord polaire -> cartesienne
	float x = sin(beta) * cos(alpha);
    float y = sin(beta) * sin(alpha);
    float z = cos(beta);

	return normalize(vec3(x, y, z));

	// fake !!!!!!
	// return normalize(random_vec3());
}

vec3 random_ray(in vec3 D)
{
	vec3 W;
	if(abs(D.x) >= 0.5)
		W = normalize(cross(D, vec3(0.0, 1.0, 0.0)));
    else
        W = normalize(cross(D, vec3(1.0, 0.0, 0.0)));
	vec3 U = normalize(cross(W, D));
	vec3 V = normalize(cross(D, U));
	mat3 M = mat3(U, V, D);
	vec3 hemisphereSample = sample_hemisphere(0.9);
	return M * hemisphereSample;
}


void main()
{
	// param de srand le nombre de random_float appelé dans le shader
	srand(3u);
	vec3 P = random_ray(normalize(normal));
	gl_Position = pvMatrix * vec4(P,1);
}
