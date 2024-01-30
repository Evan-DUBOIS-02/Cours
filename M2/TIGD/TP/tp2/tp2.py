import skimage as ski
from scipy import ndimage
import numpy as np
from matplotlib import pyplot as plt

# =====[ I. HMT ]=====
def open_hmt(img, B):
    img_ret = np.full_like(img, 0)
    # pour chaque pixel de l'image
    for x in range(img.shape[0]):
        for y in range(img.shape[1]):
            garde = True # on gade la forme
            # pour chaque pixel de l'elem structurant
            for i in range(B.shape[0]):
                for j in range(B.shape[1]):
                    # si on ne depasse pas les dimension de l'image et que l'image ne respacte pas l'element structurant
                    if x+i < img.shape[0] and y+j < img.shape[1] and img[x+i][y+j] != B[i][j]:
                        # on ne garde aucun pixel
                        garde = False
            # si on doit garder des pixels
            if garde:
                # pour chaque pixel de l'elem structurant
                for i in range(B.shape[0]):
                    for j in range(B.shape[1]):
                        # si on ne depasse pas les dimensions de l'image et que le pixel doit etre allume
                        if x+i < img.shape[0] and y+j < img.shape[1] and B[i][j] == 255:
                            # on l'alum
                            img_ret[x+i][y+j] = 255
    return img_ret

# -----[ TEST ]-----
# B = np.array([
#     [0, 0, 0, 0, 0, 0, 0],
#     [0, 0, 255, 255, 255, 0, 0],
#     [0, 0, 255, 255, 255, 0, 0],
#     [0, 255, 255, 255, 255, 255, 0],
#     [0, 0, 255, 255, 255, 0, 0],
#     [0, 0, 255, 255, 255, 0, 0],
#     [0, 0, 0, 0, 0, 0, 0],
# ])
# img = ski.io.imread("hmt.png", as_gray=True)
# img_hmt = ndimage.binary_hit_or_miss(img, B)
# img_dil = ski.morphology.dilation(img_hmt,B)
# img_my_hmt = open_hmt(img, B)

# =====[ II. ouverture annulaire ]=====
def ouverture_annulaire(img, B):
    img_dil = ski.morphology.dilation(img,B)
    img_ret = np.full_like(img, 0)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            img_ret[i][j] = np.minimum(img[i][j], img_dil[i][j])
    return img_ret

# -----[ TEST ]-----
# B = np.zeros((120, 120), dtype=np.uint8)
# cc,rr = ski.draw.circle_perimeter(60, 60, 50)
# B[cc, rr] = 255

# img = ski.io.imread("amas.png", as_gray=True)
# img_ouv_ann = ouverture_annulaire(img, B)

# =====[ III white/black top hat ]=====
def white_top_hat(img):
    img_ret = np.full_like(img, 0)
    img_ouv = ski.morphology.opening(img)
    img_ret = abs(img-img_ouv)
    return img_ret

# -----[ TEST ]-----
img = ski.io.imread("numbers.png")
img_ski = ski.morphology.white_tophat(img)
my_img = white_top_hat(img)

# =====[ AFFICHAGE ]=====
ski.io.imshow(my_img)
plt.show()