import numpy as np
import matplotlib.pyplot as plt
from skimage import color, io, segmentation, filters, exposure, measure
import sys, math, os, argparse

# python3 waterpixel.py images/waterpixel.png --size_tile 10 --size_outline 3 --k 10

# Définir les arguments de ligne de commande
parser = argparse.ArgumentParser(description="Water Pixel Processing")
parser.add_argument("image_path", help="Chemin de l'image à traiter")
parser.add_argument("--size_tile", "-st", type=int, default=30, help="Taille des carreaux de la grille")
parser.add_argument("--size_outline", "-so", type=int, default=5, help="Taille du contour de la grille")
parser.add_argument("--k", "-k", type=int, default=4, help="Paramètre de régularisation spatiale du gradient.")

# Analyser les arguments de la ligne de commande
args = parser.parse_args()

size_tile = args.size_tile
size_outline = args.size_outline
k_regulized_gradient = args.k

# Colorie la composante connexe du pixel indice [i][j]
def connectedComponent(tab, indice_I, indice_J, tabToReturn, val, tab_mark):
    if (tab_mark[indice_I][indice_J] == 1):
        return
    
    tab_mark[indice_I][indice_J] = 1

    if (tab[indice_I][indice_J] != val):
        return

    tabToReturn[indice_I][indice_J] = 255

    if (indice_I - 1 > int(size_outline/2) and tab_mark[indice_I-1][indice_J] != 1):
        connectedComponent(tab, indice_I - 1, indice_J, tabToReturn, val, tab_mark)
    if (indice_J - 1 > int(size_outline/2) and tab_mark[indice_I][indice_J-1] != 1):
        connectedComponent(tab, indice_I, indice_J - 1, tabToReturn, val, tab_mark)
    if (indice_I + 1 < tab.shape[0]-1-int(size_outline/2) and tab_mark[indice_I+1][indice_J] != 1):
        connectedComponent(tab, indice_I + 1, indice_J, tabToReturn, val, tab_mark)
    if (indice_J + 1 < tab.shape[1]-1-int(size_outline/2) and tab_mark[indice_I][indice_J+1] != 1):
        connectedComponent(tab, indice_I, indice_J + 1, tabToReturn, val, tab_mark)
    return tabToReturn

# Calcul le minimum local d'un tableau
def min_local_tab(tab):
    sizeComponent = 0
    min = 255
    tabToReturn = np.full_like(tab, 0)
    for i in range (int(size_outline/2), tab.shape[0]-int(size_outline/2)):
        for j in range (int(size_outline/2),tab.shape[1]-int(size_outline/2)):
            if (tab[i][j] < min):
                tab_mark = np.full_like(tab, 0)
                tabCountSize = np.full_like(tab, 0)
                tabCountSize = connectedComponent(np.copy(tab), i, j, tabCountSize, tab[i][j], tab_mark)
                taille = np.count_nonzero(tabCountSize == 255)
                tabToReturn = tabCountSize
                sizeComponent = taille
                min = tab[i][j]
            elif (tab[i][j] == min):
                tab_mark = np.full_like(tab, 0)
                tabCountSize = np.full_like(tab, 0)
                tabCountSize = connectedComponent(np.copy(tab), i, j, tabCountSize, tab[i][j], tab_mark)
                taille = np.count_nonzero(tabCountSize == 255)
                if (sizeComponent <= taille):
                    tabToReturn = tabCountSize
                    sizeComponent = taille

    return tabToReturn

# Génère les marqueurs d'une image
def generate_marker(img):
    img_to_return = np.copy(img)

    for i in range (0, int(img.shape[0]/size_tile)+1):
        for j in range (0, int(img.shape[1]/size_tile)+1):
            X = i*size_tile
            X_1 = (i+1)*size_tile
            Y = j*size_tile
            Y_1 = (j+1)*size_tile
            img_to_return[X:X_1,Y:Y_1] = min_local_tab(img_to_return[X:X_1,Y:Y_1])
    
    return img_to_return

# Génère une grille 2D
def generate_grid(img, color, modify, grid_epaisseur = size_outline):
    if (not(modify)):
        grid = np.full_like(img, 0)
    else:
        grid = img

    for i in range (0, img.shape[0], size_tile):
        for k in range (-grid_epaisseur, grid_epaisseur):
            for j in range (img.shape[1]):
                if (k+i < img.shape[0]):
                    grid[k+i][j] = color

    for i in range (0, img.shape[1], size_tile):
        for k in range (-grid_epaisseur, grid_epaisseur):
            for j in range (img.shape[0]):
                if (k+i < img.shape[1]):
                    grid[j][i+k] = color

    return grid

# Supperpose image1 sur image2 si le pixel est blanc
def superpose_watershed_line(img1, img2):
    for i in range(img1.shape[0]):
        for j in range(img1.shape[1]):
            if (img1[i][j] == 0):
                img2[i][j] = [255, 0, 0]             

    return img2

# Superpose les markers sur la grille
def superpose_marker_grid(img1, img2):
    for i in range(img1.shape[0]):
        for j in range(img1.shape[1]):
            if (img1[i][j] == 255):
                img2[i][j] = 1             

    return img2

# Calcul la distance d'un point (x, y) par rapport au centre de la cellule
def get_cell_center_distance(x, y):
    mult_X = x//size_tile
    mult_Y = y//size_tile

    dist = math.dist([x, y], [mult_X * size_tile + int(size_tile/2), mult_Y * size_tile + int(size_tile/2)])
    return dist/size_tile

# Calcul de la régularisation du gradient
def regulized_gradient(image, k = k_regulized_gradient, sigma = 50):
    img_to_return = np.full_like(image, 0)
    for i in range (image.shape[0]):
        for j in range (image.shape[1]):
            img_to_return[i][j] = image[i][j] + (k * (2 * get_cell_center_distance(i, j))/sigma)

    img_to_return = (img_to_return - np.min(img_to_return)) / (np.max(img_to_return) - np.min(img_to_return))

    return img_to_return

# Calculer la couleur moyenne d'un label
def get_mean_color(image, watershed_image):
    sum = [[0,0,0]]*(watershed_image.max()+1)
    number = [0]*(watershed_image.max()+1)
    img_moyenne = np.copy(image)
    for i in range(watershed_image.shape[0]):
        for j in range(watershed_image.shape[1]):
            sum[watershed_image[i][j]] += image[i][j]
            number[watershed_image[i][j]] += 1
    for i in range(watershed_image.shape[0]):
        for j in range(watershed_image.shape[1]):
            img_moyenne[i][j][0] = sum[watershed_image[i][j]][0]/number[watershed_image[i][j]]
            img_moyenne[i][j][1] = sum[watershed_image[i][j]][1]/number[watershed_image[i][j]]
            img_moyenne[i][j][2] = sum[watershed_image[i][j]][2]/number[watershed_image[i][j]]

    return img_moyenne


def generate_waterpixels(image_path):    
    # Charger l'image
    image = io.imread(image_path)

    # Egalisation de l'histogramme grâce à la méthode CLAHE
    image_clahe = exposure.equalize_adapthist(image)

    grid = color.rgb2gray(generate_grid(image, 255, False, 1)).astype(int)

    # Conversion en niveaux de gris
    gray_image = color.rgb2gray(image)
    
    # Calcul du gradient
    gradient = filters.sobel(gray_image)

    # grid = generate_grid(image, 255, False)
    regulized_gradient_img = regulized_gradient(gradient)

    # Conversion de float en int pour utiliser watershed
    gradient = np.round(gradient * 255.0).astype(int)
    regulized_gradient_img = np.round(regulized_gradient_img * 255.0).astype(int)

    # Calcul des différents marqueurs
    marker = generate_marker(gradient)
    # Conversion de float en int pour utiliser watershed
    marker_int = marker.astype(int)

    # Conversion en labels de nos marqueurs
    marker_label = measure.label(marker_int)
    # Calcul du watershed avec la ligne de séparation des eaux
    watershed_image = segmentation.watershed(regulized_gradient_img, markers=marker_label, watershed_line=True)

    # Affichage de l'image originale et des superpixels
    plt.figure(figsize=(15, 10))
    plt.subplot(121), plt.imshow(image),  plt.axis('off'), plt.title('Image originale')

    # Afficher les contours du watershed sur l'image originale
    waterpixel_image = superpose_watershed_line(watershed_image, np.copy(image))
    plt.subplot(122), plt.imshow(waterpixel_image), plt.axis('off'), plt.title('Résultat final')

    # Sauvegarder les images pour le rapport
    if (os.name == "nt"):
        image_name = image_path.split("\\")
    else:
        image_name = image_path.split("/")
        
    plt.savefig("imagesTest/" + image_name[len(image_name) - 1], bbox_inches='tight')

def main():
    if (size_outline >= size_tile):
        print("ERREUR : La taille du contour de la grille doit être inférieur à la taille des carreaux", file=sys.stderr) 
        exit(1)

    image_path = sys.argv[1]
    generate_waterpixels(image_path)

if __name__ == "__main__":
    main()