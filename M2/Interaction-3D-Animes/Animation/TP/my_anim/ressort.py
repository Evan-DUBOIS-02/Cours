
import math
from Vector import *
from Particle import *

f = open("ressort.pv","w")

time_step = 0.1
nb_points = 2
nb_frames = 1000

masse_1 = 5
position_1 = Vector(2, 0, 0)
vitesse_1 = Vector(0, 0, 0)
forces_1 = [Vector(0, 0, 0)]
point_1 = Particle(masse_1, position_1, vitesse_1, forces_1)

masse_2 = 5
position_2 = Vector(3, 0, 0)
vitesse_2 = Vector(0, 0, 0)
forces_2 = [Vector(0, 0, 0)]
point_2 = Particle(masse_2, position_2, vitesse_2, forces_2)

# ecriture de l'entete
f.write("#PV==\n" + str(time_step) + "\n" + str(nb_points) + "\n" + str(nb_frames) + "\n")
for lgn in range(10):
    f.write("Point 0:1\n")
f.write("====\n")



# parcours des frames (images)
for fr in range(nb_frames):
    # parcours des points
    v=point_1.get_position();
    f.write(str(v.x) + " " + str(v.y) + " " + str(v.z) + "\n")
    point_1.compute_new_pos_ressort(time_step, point_2)

    v=point_2.get_position();
    f.write(str(v.x) + " " + str(v.y) + " " + str(v.z) + "\n")
    point_2.compute_new_pos_ressort(time_step, point_1)

f.close()
