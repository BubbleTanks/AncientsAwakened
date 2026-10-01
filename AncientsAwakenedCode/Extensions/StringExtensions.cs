using Godot;

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
        path = Path.Join(AncientsAwakenedMain.ResPath, "images", "card_portraits", path);
        if (ResourceLoader.Exists(path)) return path;

        AncientsAwakenedMain.Logger.Info("Could not find card image path: " + path);
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "card_portraits", "card.png");
    }

    public static string BigCardImagePath(this string path)
    {
        path = Path.Join(AncientsAwakenedMain.ResPath, "images", "card_portraits", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        AncientsAwakenedMain.Logger.Info("Could not find big card image path: " + path);
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "card_portraits", "big", "card.png");
    }

    public static string PowerImagePath(this string path)
    {
        path = Path.Join(AncientsAwakenedMain.ResPath, "images", "powers", path);
        if (ResourceLoader.Exists(path)) return path;

        AncientsAwakenedMain.Logger.Info("Could not find power image path: " + path);
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "powers", "power.png");
    }

    public static string BigPowerImagePath(this string path)
    {
        path = Path.Join(AncientsAwakenedMain.ResPath, "images", "powers", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        AncientsAwakenedMain.Logger.Info("Could not find big power image path: " + path);
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "powers", "big", "power.png");
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
    
    public static string AncientScenePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "scenes", "ancients", path);
    }
    
    public static string AncientMapIconImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "ancients", "map_icons", path);
    }
    
    public static string AncientRunHistoryIconImagePath(this string path)
    {
        return Path.Join(AncientsAwakenedMain.ResPath, "images", "ancients", "run_history_icons", path);
    }
}