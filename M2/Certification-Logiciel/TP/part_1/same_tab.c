/*@
requires size >= 0;
requires \valid(&t[0..size-1]);
requires \valid(&s[0..size-1]);
requires \separated(&t[0..size-1], &s[0..size-1]);
assigns \nothing;

ensures \exists integer i; 0<=i<size && t[i] != s[i] ==> \result == 0;
ensures (\forall integer i; 0<=i<size ==> t[i] == s[i]) ==> \result == 1;
ensures \result == 0 || \result == 1;
*/
int same_tab(int t[], int s[], int size){
    /*@
    loop invariant 0 <= i <= size;
    loop invariant \forall integer j; 0<=j<i ==> t[j] == s[j];
    loop assigns i;
    loop variant size-i;
    */
    for(int i = 0; i<size; i++){
        if(t[i] != s[i]) return 0;
    }
    return 1;
}