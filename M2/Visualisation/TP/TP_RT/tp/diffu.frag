// ajouter dans le .cpp 
//     set_uniform_value(21, GLVec3(10,10,100))
layout(location=21) uniform vec3 light_pos;

vec3 compute_diffuse(in vec3 P, in vec3 N, in vec3 color)
{
    vec3 L = normalize(light_pos-P);
    float lamb = 0.1 + 0.9*max(0.0,dot(L,N)); //0.1 = ambiant
    return color * lamb;
}

vec3 raytrace(in vec3 Dir, in vec3 Orig)   
{
    traverse_all_bvh(Orig,Dir);
    vec3 N;
    vec3 P;
    intersection_info(N,P);
    vec4 color = intersection_color_info();
    vec4 mat = intersection_mat_info();
    return compute_diffuse(P,N,color.rgb);
}