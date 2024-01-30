/*@
requires \valid(t+(0..n-1)) && n > 0;
assigns \nothing;
ensures (\exists integer i; 0<=i<n && t[i] != 0) ==> \result == 0;
ensures (\forall integer i; 0<=i<n ==> t[i] == 0) ==> \result == 1;
ensures \result == 0 || \result == 1;
*/
int allzeros(int t[], int n){
    /*@
    loop invariant 0 <= i <= n;
    loop invariant (\forall integer j; 0<=j<i ==> t[j] == 0);
    loop assigns i;
    loop variant n-i;
    */
    for(int i = 0; i<n; i++){
        if(t[i] != 0) return 0;
    }
    return 1;
}

// loop variant => valeur positive qui decroit pour dire que la boucle avance
// loop invariant => toujours vrai
// loop assigns => ce qui va etre modifie dans la boucle