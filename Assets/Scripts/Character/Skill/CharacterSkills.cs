public static class CharacterSkills
{
    #region Resurrection

    public static void ReviveWith20PercentHealthAnd130PercentDamage(Player player)
    {
        player.onDie = (whichTime) =>
        {
            if(whichTime == 0)
            {
                player.SetStat("health", player.character.startHealth / 5);
                player.SetStat("damage", (int)(player.character.damage * 1.3f));
            }
            else
            {
                player.DefaultOnDie(whichTime);
            }
        };
    }

    public static void ReviveWith40PercentHealthAnd150PercentDamage(Player player)
    {
        player.onDie = (whichTime) =>
        {
            if (whichTime == 0)
            {
                player.SetStat("health", (int)(player.character.startHealth * 0.4f));
                player.SetStat("damage", (int)(player.character.damage * 1.5f));
            }
            else
            {
                player.DefaultOnDie(whichTime);
            }
        };
    }

    #endregion

    #region Start Power Boost

    public static void StartWithTwoPowers(Player player)
    {
        player.onStart += () =>
        {
            player.SetStat("power", 2f);
        };
    }

    public static void StartWithOnePowerEach(Player player)
    {
        player.onStart += () =>
        {
            player.SetStat("power", 1f);
        };
    }

    #endregion

    #region When Health Below

    public static void WhenHealthBelow60PercentDoubleTheDamage(Player player)
    {
        Player.OnTakeDamage onTakeDamage = (damage) =>
        {
            if ((float)player.health / player.character.startHealth < 0.6f)
            {
                player.SetStat("damage", player.character.damage * 2);
            }
        };

        onTakeDamage += (damage) =>
        {
            if ((float)player.health / player.character.startHealth < 0.6f)
            {
                player.onTakeDamage -= onTakeDamage;
            }
        };

        player.onTakeDamage += onTakeDamage;
    }

    #endregion
}
