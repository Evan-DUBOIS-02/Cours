// QUESTION 1
// par exemple une fonction comparant 2 tableaux necessite le predicat separated pour s'assurer que les tableaux n'occupent pas les même place en memoire

// QUESTION 2
/*@
    assigns \nothing;
    ensures \result == \max(a, \max(b, c));
*/
int max3(int a, int b, int c)
{
    if(a >= b && a >= c) return a;
    else if(b > a && b >= c) return b;
    else return c;
}

// QUESTION 3
/*@
    requires \valid(&t[0..taille-1]);
    requires taille > 1;
    assigns \nothing;
    ensures true: \result == 1 ==> \forall integer i; 0<=i<taille-1 ==> t[i] <= t[i+1];
    ensures false: \result == 0 ==> \exists integer i; 0<=i<taille-1 && t[i] > t[i+1];
*/
int increasing(int t[], int taille)
{
    /*@
        loop invariant 0 <= i <= taille-1;
        loop invariant \forall integer j; 0 <= j < i ==> t[j] <= t[j+1];
        loop assigns i;
        loop variant taille-1-i;
    */
    for(int i = 0; i < taille-1; i++)
    {
        if(t[i] > t[i+1]) return 0;
    }
    return 1;
}

// QUESTION 4
/*@
    requires \valid(&t[0..taille-1]);
    requires taille > 1;
    assigns \nothing;
    ensures true: \result == 1 ==> \forall integer i; 0<=i<taille-1 ==> t[i] >= t[i+1];
    ensures false: \result == 0 ==> \exists integer i; 0<=i<taille-1 && t[i] < t[i+1];
*/
int decreasing(int t[], int taille)
{
    /*@
        loop invariant 0 <= i <= taille-1;
        loop invariant \forall integer j; 0 <= j < i ==> t[j] >= t[j+1];
        loop assigns i;
        loop variant taille-1-i;
    */
    for(int i = 0; i < taille-1; i++)
    {
        if(t[i] < t[i+1]) return 0;
    }
    return 1;
}

/*@
    requires \valid(&t[0..taille-1]);
    requires taille > 1;
    assigns \nothing;
    behavior increasing:
        assumes \exists integer i; 0<=i<taille-1 && t[i] > t[i+1];
        ensures incr_true: \result == 1 ==> \forall integer i; 0<=i<taille-1 ==> t[i] <= t[i+1];
        ensures incr_false: \result == 0 ==> \exists integer i; 0<=i<taille-1 && t[i] > t[i+1];
    behavior decreasing:
        assumes \exists integer i; 0<=i<taille-1 && t[i] < t[i+1];
        ensures decr_true: \result == 1 ==> \forall integer i; 0<=i<taille-1 ==> t[i] >= t[i+1];
        ensures decr_false: \result == 0 ==> \exists integer i; 0<=i<taille-1 && t[i] < t[i+1];
    behavior const:
        assumes \forall integer i; 0<=i<taille-1 ==> t[i] == t[i+1];
        ensures const_true: \result == 1;
    complete behaviors;
*/
int monotonic(int t[], int taille)
{
    /*@
        loop invariant 0 <= i <= taille-1;
        loop invariant \forall integer j; 0 <= j < i ==> t[j] == t[j+1];
        loop assigns i;
        loop variant taille-1-i;
    */
    for(int i = 0; i < taille-1; i++)
    {
        if(t[i] > t[i+1]) return increasing(t, taille);
        else if(t[i] < t[i+1]) return decreasing(t, taille);
    }
    return 1;
}