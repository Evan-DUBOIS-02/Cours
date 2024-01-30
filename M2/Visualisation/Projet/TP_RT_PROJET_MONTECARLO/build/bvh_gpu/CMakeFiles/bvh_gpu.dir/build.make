# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.16

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:


#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:


# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list


# Suppress display of executed commands.
$(VERBOSE).SILENT:


# A target that is always out of date.
cmake_force:

.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/bin/cmake

# The command to remove a file.
RM = /usr/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build

# Include any dependencies generated for this target.
include bvh_gpu/CMakeFiles/bvh_gpu.dir/depend.make

# Include the progress variables for this target.
include bvh_gpu/CMakeFiles/bvh_gpu.dir/progress.make

# Include the compile flags for this target's objects.
include bvh_gpu/CMakeFiles/bvh_gpu.dir/flags.make

bvh_gpu/CMakeFiles/bvh_gpu.dir/scene.cpp.o: bvh_gpu/CMakeFiles/bvh_gpu.dir/flags.make
bvh_gpu/CMakeFiles/bvh_gpu.dir/scene.cpp.o: ../bvh_gpu/scene.cpp
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building CXX object bvh_gpu/CMakeFiles/bvh_gpu.dir/scene.cpp.o"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++  $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -o CMakeFiles/bvh_gpu.dir/scene.cpp.o -c /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/scene.cpp

bvh_gpu/CMakeFiles/bvh_gpu.dir/scene.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/bvh_gpu.dir/scene.cpp.i"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/scene.cpp > CMakeFiles/bvh_gpu.dir/scene.cpp.i

bvh_gpu/CMakeFiles/bvh_gpu.dir/scene.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/bvh_gpu.dir/scene.cpp.s"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/scene.cpp -o CMakeFiles/bvh_gpu.dir/scene.cpp.s

bvh_gpu/CMakeFiles/bvh_gpu.dir/bvh.cpp.o: bvh_gpu/CMakeFiles/bvh_gpu.dir/flags.make
bvh_gpu/CMakeFiles/bvh_gpu.dir/bvh.cpp.o: ../bvh_gpu/bvh.cpp
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Building CXX object bvh_gpu/CMakeFiles/bvh_gpu.dir/bvh.cpp.o"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++  $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -o CMakeFiles/bvh_gpu.dir/bvh.cpp.o -c /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/bvh.cpp

bvh_gpu/CMakeFiles/bvh_gpu.dir/bvh.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/bvh_gpu.dir/bvh.cpp.i"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/bvh.cpp > CMakeFiles/bvh_gpu.dir/bvh.cpp.i

bvh_gpu/CMakeFiles/bvh_gpu.dir/bvh.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/bvh_gpu.dir/bvh.cpp.s"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/bvh.cpp -o CMakeFiles/bvh_gpu.dir/bvh.cpp.s

bvh_gpu/CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.o: bvh_gpu/CMakeFiles/bvh_gpu.dir/flags.make
bvh_gpu/CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.o: ../bvh_gpu/gpu_bvh_scene.cpp
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_3) "Building CXX object bvh_gpu/CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.o"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++  $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -o CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.o -c /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/gpu_bvh_scene.cpp

bvh_gpu/CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.i"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/gpu_bvh_scene.cpp > CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.i

bvh_gpu/CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.s"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/gpu_bvh_scene.cpp -o CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.s

bvh_gpu/CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.o: bvh_gpu/CMakeFiles/bvh_gpu.dir/flags.make
bvh_gpu/CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.o: ../bvh_gpu/glsl_rec.cpp
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_4) "Building CXX object bvh_gpu/CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.o"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++  $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -o CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.o -c /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/glsl_rec.cpp

bvh_gpu/CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.i"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/glsl_rec.cpp > CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.i

bvh_gpu/CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.s"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu/glsl_rec.cpp -o CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.s

# Object files for target bvh_gpu
bvh_gpu_OBJECTS = \
"CMakeFiles/bvh_gpu.dir/scene.cpp.o" \
"CMakeFiles/bvh_gpu.dir/bvh.cpp.o" \
"CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.o" \
"CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.o"

# External object files for target bvh_gpu
bvh_gpu_EXTERNAL_OBJECTS =

bvh_gpu/libbvh_gpu.a: bvh_gpu/CMakeFiles/bvh_gpu.dir/scene.cpp.o
bvh_gpu/libbvh_gpu.a: bvh_gpu/CMakeFiles/bvh_gpu.dir/bvh.cpp.o
bvh_gpu/libbvh_gpu.a: bvh_gpu/CMakeFiles/bvh_gpu.dir/gpu_bvh_scene.cpp.o
bvh_gpu/libbvh_gpu.a: bvh_gpu/CMakeFiles/bvh_gpu.dir/glsl_rec.cpp.o
bvh_gpu/libbvh_gpu.a: bvh_gpu/CMakeFiles/bvh_gpu.dir/build.make
bvh_gpu/libbvh_gpu.a: bvh_gpu/CMakeFiles/bvh_gpu.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=/home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/CMakeFiles --progress-num=$(CMAKE_PROGRESS_5) "Linking CXX static library libbvh_gpu.a"
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && $(CMAKE_COMMAND) -P CMakeFiles/bvh_gpu.dir/cmake_clean_target.cmake
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/bvh_gpu.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
bvh_gpu/CMakeFiles/bvh_gpu.dir/build: bvh_gpu/libbvh_gpu.a

.PHONY : bvh_gpu/CMakeFiles/bvh_gpu.dir/build

bvh_gpu/CMakeFiles/bvh_gpu.dir/clean:
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu && $(CMAKE_COMMAND) -P CMakeFiles/bvh_gpu.dir/cmake_clean.cmake
.PHONY : bvh_gpu/CMakeFiles/bvh_gpu.dir/clean

bvh_gpu/CMakeFiles/bvh_gpu.dir/depend:
	cd /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/bvh_gpu /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu /home/evan/Documents/M2/projet-visualisation/TP_RT_PROJET_MONTECARLO/build/bvh_gpu/CMakeFiles/bvh_gpu.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : bvh_gpu/CMakeFiles/bvh_gpu.dir/depend
