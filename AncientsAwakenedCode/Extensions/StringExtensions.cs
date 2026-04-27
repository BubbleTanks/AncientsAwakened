namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "card_portraits", "big", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "powers", "big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "relics", "big", path);
    }
}