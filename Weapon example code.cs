public class Weapon : MonoBehaviour
{
	struct weaponStats
	{
		int range;
		int damage;
		int speed;
	}

	enum archetype { Sword, Axe, Spear, Bow };
	enum dmgStyle { Pitiful = -1, Basic = 0, Sharp = 1, Dreaded = 2.25, Godly = 4};
	enum rngStyle { Blinded, Basic, Long, Sighted, Reaching};
	enum spdStyle { Sluggish, Basic, Speedy, Agile, Hasty };
	
	weaponStats swordStats;
	swordStats.range = 3;
	swordStats.damage = 3;
	swordStats.speed = 3;
	
	weaponStats axeStats;
	axeStats.range = 3;
	axeStats.damage = 5;
	axeStats.speed = 2;
	
	weaponStats spearStats;
	spearStats.range = 5;
	spearStats.damage = 3;
	spearStats.speed = 2;
	
	weaponStats bowStats;
	bowStats.range = 7;
	bowStats.damage = 2;
	bowStats.speed = 3;
	
	int range;
	int damage;
	int speed;
	
	archetype weaponType;
	dmgStyle damageStyle;
	rngStyle rangeStyle;
	spdStyle speedStyle;
	
	int currentGameLevel;
	
	void Start()
	{
		weaponType = GetRandomEnum<archetype>();
		
		damageStyle = GetRandomEnum<dmgStyle>();
		rangeStyle = GetRandomEnum<rngStyle>();
		speedStyle = GetRandomEnum<spdStyle>();
	}
	
	void Update()
	{
		
	}
	
	void SetStats()
	{
		switch (weaponType)
		{
			case archetype.Sword:
				range = swordStats.range + rangeStyle;
				damage = swordStats.damage + damageStyle;
				speed = swordStats.speed + speedStyle;
				break;
				
			case archetype.Axe:
				range = axeStats.range + rangeStyle;
				damage = axeStats.damage + damageStyle;
				speed = axeStats.speed + speedStyle;
				break;
			
			case archetype.Spear
				range = spearStats.range + rangeStyle;
				damage = spearStats.damage + damageStyle;
				speed = spearStats.speed + speedStyle;
				break;
				
			case archetype.Bow
				range = bowStats.range + rangeStyle;
				damage = bowStats.damage + damageStyle;
				speed = bowStats.speed + speedStyle;
				break;
		}
	}
}

/*
public Point(int _x, int _y)
{
	x = _x;
	y = _y;
}

public int X
{
	get 
	{
		return x;
	}
	set
	{
		x = value;
	}
}
*/