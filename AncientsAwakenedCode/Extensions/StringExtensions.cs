namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "card_portraits", "big", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "powers", "big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "relics", "big", path);
    }
    
    public static string RestSiteImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "rest_site", path);
    }
    
    public static string EnchantmentImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "enchantments", path);
    }
    
    public static string PotionImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "potions", path);
    }
    
    public static string AfflictionScenePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "scenes", "afflictions", path);
    }
}