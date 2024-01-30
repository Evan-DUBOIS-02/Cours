/*@
ensures (\result == a && a<b) || (\result == b && a>=b);
assigns \nothing;
*/
int minimum(int a, int b) {
    return (a < b) ? a : b;
}