cmake_minimum_required(VERSION 3.0)
project(RaytracingGPU LANGUAGES CXX)

set (SYS_DATA_PATH $ENV{HOME} CACHE PATH "Path where to read image mesh ...")
set(SHADER_PATH ${CMAKE_SOURCE_DIR}/shaders/ CACHE PATH "Path to shader directory")
set(TP_SHADER_PATH ${CMAKE_SOURCE_DIR}/tp/ CACHE PATH "Path to shader directory")

set(CMAKE_CXX_STANDARD 14)

add_library(raytracer STATIC scene.h scene.cpp bvh.h bvh.cpp gpu_bvh_scene.h gpu_bvh_scene.cpp glsl_rec.h glsl_rec.cpp)
target_link_libraries(raytracer easycppogl)

add_executable(${PROJECT_NAME} rtgpu_bvh.cpp )
target_link_libraries(${PROJECT_NAME} raytracer)
target_compile_definitions(${PROJECT_NAME}
		PRIVATE
		-DSHADER_PATH=std::string\(\"${SHADER_PATH}/\"\)
		-DTP_SHADER_PATH=std::string\(\"${TP_SHADER_PATH}/\"\)
		-DDATA_PATH=std::string\(\"${SYS_DATA_PATH}/\"\)
		)
