import skimage as ski
import numpy as np
from matplotlib import pyplot as plt

F1 = np.array([
[0, 0, 0, 0, 0],
[0, 0, 1, 1, 0],
[1, 1, 1, 1, 0],
[0, 1, 1, 0, 0],
[0, 1, 0, 0, 0]
])

F2 = np.array([
[0, 0, 0, 0, 0],
[0, 0, 1, 1, 0],
[1, 1, 0, 1, 0],
[0, 1, 1, 0, 0],
[0, 1, 0, 0, 0]
])

# B = np.array([
# [0, 0, 0, 1, 1],
# [0, 0, 1, 1, 1],
# [0, 1, 1, 1, 0],
# [1, 1, 1, 0, 0],
# [1, 1, 0, 0, 0]
# ])

B = np.array([
    [0, 0, 0, 0, 0, 0, 0, 0, 0, 1],
    [0, 0, 0, 0, 0, 0, 0, 0, 1, 0],
    [0, 0, 0, 0, 0, 0, 0, 1, 0, 0],
    [0, 0, 0, 0, 0, 0, 1, 0, 0, 0],
    [0, 0, 0, 0, 0, 1, 0, 0, 0, 0],
    [0, 0, 0, 0, 1, 0, 0, 0, 0, 0],
    [0, 0, 0, 1, 0, 0, 0, 0, 0, 0],
    [0, 0, 1, 0, 0, 0, 0, 0, 0, 0],
    [0, 1, 0, 0, 0, 0, 0, 0, 0, 0],
    [1, 0, 0, 0, 0, 0, 0, 0, 0, 0],

])

def mon_erosion_niv_gris(F, B):
    G = np.zeros_like(F)
    origin_x = B.shape[0]//2
    origin_y = B.shape[1]//2
    for x in range(F.shape[0]):
        for y in range(F.shape[1]):
            min = np.max(F)
            for x_elem in range(B.shape[0]):
                for y_elem in range(B.shape[1]):
                    if(B[x_elem][y_elem] == 1):
                        decalage_x = x_elem-origin_x
                        decalage_y = y_elem-origin_y
                        if(x+decalage_x>=0 and x+decalage_x<F.shape[0] and y+decalage_y>=0 and y+decalage_y<F.shape[1]):
                            if(F[x+decalage_x][y+decalage_y]<min):
                                min=F[x+decalage_x][y+decalage_y]
            G[x][y]=min
    return G

def ma_dilatation_niv_gris(F, B):
    G = np.zeros_like(F)
    origin_x = B.shape[0]//2
    origin_y = B.shape[1]//2
    for x in range(F.shape[0]):
        for y in range(F.shape[1]):
            max = np.min(F)
            for x_elem in range(B.shape[0]):
                for y_elem in range(B.shape[1]):
                    if(B[x_elem][y_elem] == 1):
                        decalage_x = x_elem-origin_x
                        decalage_y = y_elem-origin_y
                        if(x+decalage_x>=0 and x+decalage_x<F.shape[0] and y+decalage_y>=0 and y+decalage_y<F.shape[1]):
                            if(F[x+decalage_x][y+decalage_y]>max):
                                max=F[x+decalage_x][y+decalage_y]
            G[x][y]=max
    return G

def closing(F, B):
    return ski.morphology.erosion(ski.morphology.dilation(F,np.flip(B)), B)

def opening(F, B):
    return ski.morphology.dilation(ski.morphology.erosion(F,B), np.flip(B))

# print("IN:\n"+str(F1))
# print("OUT:\n"+str(mon_erosion_niv_gris(F1, B)))
# print("CORRECTION:\n"+str(ski.morphology.erosion(F1, B)))

# # marche pas
# print("IN:\n"+str(F2))
# print("OUT:\n"+str(ma_dilatation_niv_gris(F2, B)))
# print("CORRECTION:\n"+str(ski.morphology.dilation(F1, B)))

# print("IN:\n"+str(F1))
# print("OUT:\n"+str(opening(F1, B)))

# # marche pas
# print("IN:\n"+str(F2))
# print("OUT:\n"+str(opening(F2, B)))

img = ski.io.imread("lines.png", as_gray=True)

img_ferm = ski.morphology.closing(img,B)

ski.io.imshow(img_ferm)
plt.show()