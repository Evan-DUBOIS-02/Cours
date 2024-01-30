/*@
    requires size > 0;
    requires \valid(&t[0..size-1]);
    assigns \nothing;
    ensures (\result == -1 && (\forall integer i; 0<=i<size ==> t[i] != val)) || t[\result] == val;
*/

int val_idx(int t[], int size, int val){
    /*@
        loop invariant 0 <= i <= size;
        loop invariant \forall integer j; 0<=j<i ==> t[j] != val;
        loop assigns i;
        loop variant size-i;
    */
    for(int i = 0; i<size; i++){
        if(t[i] == val) return i;
    }
    return -1;
}