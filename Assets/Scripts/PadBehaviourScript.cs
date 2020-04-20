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
    public GameObject monster;

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

    public void CreateMonster()
    {
        if (GameManager.dungeonEssence > 100)
        {
            Vector3 pos1 = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            GameObject newMonster = Instantiate(monster);
            newMonster.transform.position = new Vector3(pos1.x+1, 1.6f, pos1.z+1);
            GameManager.dungeonEssence -= 100;
        }
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
