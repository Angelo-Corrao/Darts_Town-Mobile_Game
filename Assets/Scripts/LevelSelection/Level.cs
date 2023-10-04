using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Level: ScriptableObject
{
	public enum Location
	{
		grandmaHouse,
		office,
		pub,
		musicStore,
		arena
	}

	public enum DailyLocation 
	{
		grandmaHouseDaily,
		officeDaily,
		pubDaily,
		musicStoreDaily,
		arenaDaily
	}

	public enum LocationVictory 
	{
		grandmaHouseVictories,
		officeVictories,
		pubVictories,
		musicStoreVictories,
		arenaVictories
	}

	public string levelName;
	public int sceneId;
	public Location location;
	public DailyLocation dailyLocation;
	public LocationVictory victory;
	public int prize;
	public int entryFee;
	public string gameMode;
	public string gameModeDescription;
	public Sprite gameModeInfoTargetSprite;
	public List<int> numberOfGamesForUpgrade;
	public Sprite lockedLevel;
	public Sprite unlockedeLevel;

	public Level previuosLevel;
	public int couterToUnlock; //times you need to play the previous level to unlock

	public bool CheckUnlocked()
	{
		return PlayerPrefs.GetInt(previuosLevel.location.ToString()) >= couterToUnlock;
	}

	//returns the index of the map upgrade that has been unlocked
	public int UnlockMapUpgrade() 
	{
		for (int i = 0; i < numberOfGamesForUpgrade.Count; i++) 
		{
			if (!(PlayerPrefs.GetInt(victory.ToString()) >= numberOfGamesForUpgrade[i]))
				return i - 1;
				
		}

		return numberOfGamesForUpgrade.Count - 1;

	}

	public Sprite ChangeSprite() 
	{
		return CheckUnlocked() ? unlockedeLevel : lockedLevel;
	}

}
