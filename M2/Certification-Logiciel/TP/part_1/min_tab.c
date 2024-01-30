/*@
    requires size > 0;
    requires \valid(&t[0..size-1]);
    assigns \nothing;
    ensures (\forall integer i; 0<=i<size ==> t[i] >= t[\result]);
*/

int min_idx(int t[], int size){
    int min_idx = 0;
    /*@
        loop invariant 0 <= i <= size;
        loop invariant \forall integer j; 0<=j<i ==> t[j] >= t[min_idx];
        loop invariant 0 <= min_idx <= i;
        loop assigns i, min_idx;
        loop variant size-i;
    */
    for(int i = 0; i<size; i++){
        if(t[i] < t[min_idx]) min_idx = i;
    }
    return min_idx;
}