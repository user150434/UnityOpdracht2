using UnityEngine;

public class CollectibleItem : MonoBehaviour

{

    public static int collectedItems = 0; // Houdt het aantal verzamelde items bij

    // Deze functie wordt aangeroepen wanneer de speler met het object in contact komt

    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("Player")) // Controleer of de speler de trigger raakt

        {

            collectedItems++; // Verhoog het aantal verzamelde objecten

            Destroy(gameObject); // Verwijder het object uit de scène

        }

    }

}

