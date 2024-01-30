cmake_minimum_required(VERSION 3.0)
project(RayTracing_TP)

set (CMAKE_CXX_STANDARD 11)
add_definitions("-DEIGEN_NO_DEBUG=1 -EIGEN_MAX_STATIC_ALIGN_BYTES=16 -DEIGEN_STRONG_INLINE=1")

if(MSVC)
add_compile_options(/arch:AVX2)
else()
    add_compile_options(-msse4)
endif()

if(APPLE)
	link_directories("/usr/local/lib")
	message(WARNING MACOSX)
endif()

add_subdirectory(easycppogl_src)
add_subdirectory(RaytracingGPU)
