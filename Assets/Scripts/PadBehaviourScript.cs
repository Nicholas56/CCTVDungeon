using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Easterby - EAS12337350
//Special function relating to player interaction and the control room

public class PadBehaviourScript : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject monsterMenu;
    public GameObject healthMenu;

    Transform player;
    public Transform controlRoomPos;

    public void ReturnToControlRoom()
    {
        //Will find player transform and return the player to control room
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").transform; }
        player.position = controlRoomPos.position;
        GameManager.canTeleport = false;
    }

    public void MoveToLocation(Transform location)
    {
        //Using the screens, the player can move to the dungeon
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").transform; }
        player.position = location.position;
    }

    public void ShowMenu(GameObject menu)
    {
        //Opens the selected menu
        CloseMenu();
        menu.GetComponent<Animator>().SetBool("Menu", true);
    }

    void CloseMenu()
    {
        //Closes all menus
        optionMenu.GetComponent<Animator>().SetBool("Menu", false);
        monsterMenu.GetComponent<Animator>().SetBool("Menu", false);
        healthMenu.GetComponent<Animator>().SetBool("Menu", false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
