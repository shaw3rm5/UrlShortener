namespace UrlShortener.Application;

public class Base62Manager
{
    public string ToBase62(long number)
    {
        var alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var n = number;
        int basis = 62;
        var ret = "";
        while (n > 0)
        {
            long temp = n % basis;
            ret = alphabet[(int)temp] + ret;
            n = (n / basis);

        }
        return ret;  
    }
}