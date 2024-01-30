import math
from Vector import *


class Particle:
    def __init__(self,masse=0,position=Vector(),vitesse=Vector(),forces=[]):
        self.masse=masse
        self.position=position
        self.vitesse=vitesse
        self.forces=forces

    def set(self,masse,position,vitesse,forces):
        self.masse=masse
        self.position=position
        self.vitesse=vitesse
        self.forces=forces

    def add_force(self, F):
        self.forces.append[F]

    def somme_forces(self):
        res = Vector()
        for F in self.forces:
            res += F
        return res

    def compute_new_pos(self, T):
        somme_forces = Vector()
        for F in self.forces:
            somme_forces += F
        self.position = self.position + self.vitesse*T + (somme_forces*T*T)*(1/self.masse)
        self.vitesse = self.vitesse + (somme_forces*T*T)*(1/self.masse)

    def compute_new_pos_rebond(self, T):
        # somme des vecteurs 4
        somme_forces = Vector()
        for F in self.forces:
            somme_forces += F

        # application des formules
        self.position = self.position + self.vitesse*T + (somme_forces*T*T)*(1/self.masse)
        k = self.vitesse.y
        self.vitesse = self.vitesse + (somme_forces*T*T)*(1/self.masse)

        # calcul du vecteur opose a la gravite
        k /= self.vitesse.y
        if self.position.y <= 0:
            self.forces[0] = Vector(0, 0, 0)
            self.forces[1] = Vector(0, 1, 0)*(-k*self.position.y)
        else:
            self.forces[0] = Vector(0, self.masse*-0.981, 0)
            self.forces[1] = Vector(0, 0, 0)


    def compute_new_pos_ressort(self, T, autre):
        k = 10
        lo = 0.5
        print(lo-self.position.distanceAvec(autre.position))

        u = self.position - autre.position
        if(u.x < 0): u = Vector(-1, 0, 0)
        else: u = Vector(1, 0, 0)
        self.forces[0] = u*k*(lo-self.position.distanceAvec(autre.position))

        somme_forces = Vector()
        for F in self.forces:
            somme_forces += F
        self.position = self.position + self.vitesse*T + (somme_forces*T*T)*(1/self.masse)
        self.vitesse = self.vitesse + (somme_forces*T*T)*(1/self.masse)


    def get_position(self):
        return self.position
