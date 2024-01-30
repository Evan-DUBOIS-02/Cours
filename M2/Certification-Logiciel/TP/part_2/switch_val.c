/*@
    requires \valid(a) && \valid(b) && \separated(a, b);
    assigns *a, *b;
    ensures *a == \old(*b) && *b == \old(*a);
*/
void swap(int* a, int* b){
    int tmp = *a;
    *a = *b;
    *b = tmp;
}

/*@
    requires \valid(a) && \valid(b) && \separated(a, b);
    assigns *a, *b;
    ensures *a <= *b;
    behavior a_inf:
        assumes *a <= *b;
        ensures *a == \old(*a) && *b == \old(*b);
    behavior b_inf:
        assumes *b < *a;
        ensures *a == \old(*b) && *b == \old(*a);
    complete behaviors;
    disjoint behaviors;
*/
void order2(int* a, int* b){
    if(*a > *b) swap(a, b);
}

/*@
    requires \valid(a) && \valid(b) && \valid(c);
    requires \separated(a, b, c);
    assigns *a, *b, *c;
    ensures *a <= *b <= *c;
    ensures \union(*a, *b, *c) == \union(\old(*a), \old(*b), \old(*c));
    // ajouter avec suivant b ce qu'on avait avant => \old(*a <= *b <= *c ...)
*/
void order3(int* a, int* b, int* c){
    order2(a, b);
    order2(a, c);
    order2(b, c);
}

//@ assigns \nothing;
void test() {
    int a = 1, b = 3, c = 2;
    order3(&a, &b, &c);
    //@ assert a == 1 && b == 2 && c == 3;
    a = 2, b = 1, c = 0;
    order3(&a, &b, &c);
    //@ assert a == 0 && b == 1 && c == 2;
    a = 2, b = 2, c = 1;
    order3(&a, &b, &c);
    //@ assert a == 1 && b == 2 && c == 2;
    a = 1, b = 2, c = 1;
    order3(&a, &b, &c);
    //@ assert a == 1 && b == 1 && c == 2;
}