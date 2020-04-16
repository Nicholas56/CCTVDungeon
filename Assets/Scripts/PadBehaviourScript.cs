using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadBehaviourScript : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject monsterMenu;
    public GameObject healthMenu;

    public Transform player;
    public Transform controlRoomPos;

    public void ReturnToControlRoom()
    {
        player.position = controlRoomPos.position;
    }

    public void ShowMenu(GameObject menu)
    {
        menu.GetComponent<Animator>().SetBool("Menu", true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
