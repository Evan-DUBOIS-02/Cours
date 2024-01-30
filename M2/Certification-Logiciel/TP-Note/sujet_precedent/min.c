/*@
    assigns \nothing;
    ensures \result == \min(a, b);
*/
int min2(int a, int b)
{
    if(a <= b) return a;
    else if (b < a) return b;
}

/*@
    assigns \nothing;
    ensures \result == \min(a, \min(b, c));
*/
int min3(int a, int b, int c)
{
    if(a <= b && a <= c) return a;
    else if(b < a && b <= c) return b;
    else if(c < a && c < b) return c;
}

/*@
    assigns \nothing;
    ensures \result == \min(\min(a, b), \min(c, d));
*/
int min4(int a, int b, int c, int d)
{
    return min2(min2(a, b), min2(c, d));
}

/*@
    assigns \nothing;
    ensures \result == a && a >= b && a >= c || \result == b && b > a && b >= c || \result == c && c > a && c > b;
*/
void max3(int a, int b, int c)
{
    if(a >= b && a >= c) return a;
    else if(b > a && b >= c) return b;
    else if(c > a && c > b) return c;
}