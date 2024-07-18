using UnityEngine;
using UnityEngine.UI;

/* An Item that can be consumed. So far that just means regaining health */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class Consumable : Item {

	public int healthGain;		

	//$"{AmountOfHealthCurrent}/{CalculationCurrentAmountOfHealthMAX()}";
	public override void Use()
	{
		if((playerController.instance.playerStats.CalculationCurrentAmountOfHealthMAX() - playerController.instance.playerStats.AmountOfHealthCurrent)< healthGain)
		{
            if (playerController.instance.playerStats.HealthImage != null)
            {
                playerController.instance.playerStats.HealthImage.GetComponent<Image>().fillAmount += 1.0f / playerController.instance.playerStats.CalculationCurrentAmountOfHealthMAX() * (playerController.instance.playerStats.CalculationCurrentAmountOfHealthMAX() - playerController.instance.playerStats.AmountOfHealthCurrent);
            }
            playerController.instance.playerStats.AmountOfHealthCurrent += playerController.instance.playerStats.CalculationCurrentAmountOfHealthMAX() - playerController.instance.playerStats.AmountOfHealthCurrent;

        }
		else
		{
            if (playerController.instance.playerStats.HealthImage != null)
            {
                playerController.instance.playerStats.HealthImage.GetComponent<Image>().fillAmount += 1.0f / playerController.instance.playerStats.CalculationCurrentAmountOfHealthMAX() * healthGain;
            }
            playerController.instance.playerStats.AmountOfHealthCurrent += healthGain;

        }

        playerController.instance.playerStats.TextHHp();
        playerController.instance.playerStats.TextMMp();
        

    }

}
