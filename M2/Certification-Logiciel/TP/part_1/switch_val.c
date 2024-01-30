/*@
    requires \valid(a) && \valid(b) && \separated(a, b);
    assigns *a, *b;
    ensures *a == \old(*b) && *b == \old(*a);
*/

void switch_val(int* a, int* b){
    *a = *a + *b;
    *b = *a - *b;
    *a = *a - *b;
}