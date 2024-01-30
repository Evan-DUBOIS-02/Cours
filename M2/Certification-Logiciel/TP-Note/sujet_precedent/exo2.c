/*@
    requires \valid(&t[0..size-1]);
    requires 1 < u < size;
    requires size > 0;
    assigns \nothing;
    behaviour not_increase:
        assumes \forall integer i; 0 <= i < u ==> t[i] < t[i+1];
        ensures \result == 0;
    ensures \forall integer i; u<=i<taille-1 ==> t[i] > t[i+1];
*/
int up_and_down(int t[], int taille, int u)
{
    /*@
        loop invariant 0 <= i <= size;
        loop invariant \forall integer j; 0 <= j < i < u ==> t[j] < t[j+1];
        loop invariant \forall integer j; u <= j < i < taille ==> t[j] > t[j+1];
        loop assigns i;
        loop variant size-i;
    */
    for(int i = 0; i<taille-1; i++)
    {
        if(i < u && t[i] > t[i+1]) return 0;
        else if(i>u && t[i] < t[i+1]) return 0;
    }
    return 1;
}