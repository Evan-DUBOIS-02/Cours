/*@
    requires size > 0;
    requires \valid(&t[0..size-1]);
    assigns \nothing;
    ensures (\forall integer i; 0<=i<size ==> t[i] == t[size-1-i]) ==> \result == 1; 
    ensures (\exists integer i; 0<=i<size && t[i] != t[size-1-i]) ==> \result == 0;
*/

int is_palindrome(int t[], int size){

    /*@
        loop invariant 0 <= i <= size;
        loop invariant \forall integer j; 0<=j<i ==> t[j] == t[size-1-j];
        loop assigns i;
        loop variant size-i;
    */
    for(int i = 0; i < size; i++){
        if(t[i] != t[size-1-i]) return 0;
    }
    return 1;
}