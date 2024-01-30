import skimage as ski
import numpy as np
from matplotlib import pyplot as plt

def ImgSum(I):
    res = 0
    for line in I:
        for val in line:
            res += val
    return res

def granularite_bin():
    img = ski.io.imread("grains.jpg", as_gray=True)

    # seuillage de l'image pour la passer en binaire
    img_seuil = np.where(img>120/256, 1, img)
    img_seuil = np.where(img_seuil<120/256, 0, img_seuil)

    sum = []
    for i in range(10):
        img_open = ski.morphology.isotropic_opening(img_seuil, i)
        sum.append(ImgSum(img_open))

    sum_percent = []
    max = sum[0]
    for i in range(10):
        sum_percent.append(sum[i]*100/max)

    sum_new = []
    x_new = []
    for i in range(9): 
        x_new.append((i + i + 1) / 2)
        sum_new.append(sum_percent[i+1]-sum_percent[i])

    plt.plot(sum_percent)
    plt.plot(x_new, sum_new)
    plt.show()

def granularite_pas_bin():
    img = ski.io.imread("grains.jpg", as_gray=True)

    sum = []
    for i in range(10):
        img_open = ski.morphology.opening(img)
        sum.append(ImgSum(img_open))

    sum_percent = []
    max = sum[0]
    for i in range(10):
        sum_percent.append(sum[i]*100/max)

    sum_new = []
    x_new = []
    for i in range(9): 
        x_new.append((i + i + 1) / 2)
        sum_new.append(sum_percent[i+1]-sum_percent[i])

    plt.plot(sum_percent)
    plt.plot(x_new, sum_new)
    plt.show()

granularite_bin()