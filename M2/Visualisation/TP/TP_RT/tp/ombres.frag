layout(location=21) uniform vec3 light_pos;

vec3 compute_shadow(in vec3 O, in vec3 P, in vec3 N, in vec3 color, float rough)
{
    // ambiant
    vec3 final = 0.1*color;
    // dir of light
    vec3 L = normalize(light_pos-P);

    // are we in the shadow ?
    just_hit_bvh(P+BIAS*N,L);
    if (hit())
        return final;

    // if not continue normal lighting computation
    float lamb = 0.9*max(0.0,dot(L,N));
    vec3 R = reflect(-L,N);
    vec3 E = normalize(O-P);
    float se = mix(200,2,rough);
    float spec = pow(max(0.0,dot(E,R)),se) * (1.0-rough);
    final += color * lamb + spec * vec3(1);
    return final;
}


vec3 raytrace(in vec3 Dir, in vec3 Orig)   
{
    traverse_all_bvh(Orig,Dir);
    vec3 N;
    vec3 P;
    intersection_info(N,P);
    vec4 color = intersection_color_info();
    vec4 mat = intersection_mat_info();
    return compute_shadow(Orig,P,N,color.rgb,mat.g);
}