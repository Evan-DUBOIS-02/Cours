
import math
from Vector import *
from Particle import *

f = open("gravity.pv","w")

time_step = 0.1
nb_points = 1
nb_frames = 1000

masse = 10
position = Vector(0, 3, 0)
vitesse = Vector(0, 0, 0)
forces = [Vector(0, -0.981, 0)*masse, Vector(0, 0, 0)]
point = Particle(masse, position, vitesse, forces)

# ecriture de l'entete
f.write("#PV==\n" + str(time_step) + "\n" + str(nb_points) + "\n" + str(nb_frames) + "\n")
for lgn in range(10):
    f.write("Point 0:0\n")
f.write("====\n")



# parcours des frames (images)
for fr in range(nb_frames):
    # parcours des points
    for n in range(nb_points):
        v=point.get_position();
        f.write(str(v.x) + " " + str(v.y) + " " + str(v.z) + "\n")
        point.compute_new_pos_rebond(time_step)

f.close()
