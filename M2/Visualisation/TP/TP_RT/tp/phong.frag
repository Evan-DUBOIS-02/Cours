layout(location=21) uniform vec3 light_pos;

vec3 compute_lighting(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
{
    vec3 L = normalize(light_pos-P);
    float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
    vec3 R = reflect(-L,N);
    vec3 E = normalize(O-P);
    float se = mix(200,2,rough); // roughness -> specular exposant phong
    float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough*rough);
            // * (1.0-rough*rough)
    return color * lamb + spec * vec3(1);
}

vec3 raytrace(in vec3 Dir, in vec3 Orig)   
{
    traverse_all_bvh(Orig,Dir);
    vec3 N;
    vec3 P;
    intersection_info(N,P);
    vec4 color = intersection_color_info();
    vec4 mat = intersection_mat_info();
    return compute_lighting(Orig,P,N,color.rgb,mat.g);
}