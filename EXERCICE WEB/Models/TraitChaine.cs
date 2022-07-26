using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class TraitChaine
{

    public static string Replace(string chaine)
    { return (chaine ?? "").Replace("'","''"); }

    #region type
    public static int retournInt(String value)
    {
        int ouput = 0;
        if (int.TryParse(value, out ouput))
        {
            ouput = int.Parse(value);
        }
        return ouput;
    }
    public static decimal retourndecimal(String value)
    {
        decimal ouput = 0;
        if (decimal.TryParse(value, out ouput))
        {
            ouput = decimal.Parse(value);
        }
        return ouput;
    }
    public static bool retournbool(String value)
    {
        bool ouput = false;
        if (bool.TryParse(value, out ouput))
        {
            ouput = bool.Parse(value);
        }
        return ouput;
    }
    public static DateTime retourndate(String value)
    {
        DateTime ouput;
        if (DateTime.TryParse(value, out ouput))
        {
            ouput = DateTime.Parse(value);
        }
        return ouput;
    }
    public static long returnlong(String value)
    {
        long ouput = 0;
        if (long.TryParse(value, out ouput))
        {
            ouput = long.Parse(value);
        }
        return ouput;
    }
    #endregion type


}
