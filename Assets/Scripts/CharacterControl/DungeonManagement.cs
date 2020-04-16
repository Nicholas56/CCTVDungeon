using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonManagement : MonoBehaviour
{
    public GameObject placeHolder;
    public List<GameObject> dungeonObjects;

    GameObject selectedMonster;
    GameObject modifyMonster;

    Transform monsterHolder;

    public Animator menuBar;
    public Animator modifyBar;
    public Animator optionBar;
    public Camera playerCamera;
    bool isModify;
    bool isMoving;

    public TMP_Text essenceCount;
    public TMP_Text monsterChoice;
    List<string> choices = new List<string> { "Weakest", "Strongest", "Healer" };
    int choiceNum;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //When there is a monster selected, pressing the fire button will make a placeholder appear, then when released, the monster appears
        if (selectedMonster)
        {
            if (Input.GetButton("Fire1"))
            {
                placeHolder.GetComponent<MeshRenderer>().enabled = true;
            }
            else { placeHolder.GetComponent<MeshRenderer>().enabled = false; }
            if (Input.GetButtonUp("Fire1"))
            {
                GameObject placedMonster = Instantiate(selectedMonster, placeHolder.transform.position, Quaternion.identity, monsterHolder);

            }
        }
        //When the move monster option is picked
        if (isMoving)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (modifyMonster)
                {
                    modifyMonster.transform.SetParent(monsterHolder);
                }
            }
        }
        if (isModify)
        {
            optionBar.SetBool("Menu", false);
            //When the player clicks the mouse, whatever is in front will be 
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    if (objectHit.gameObject.GetComponent<MonsterScript>())
                    {
                        modifyMonster = objectHit.gameObject;
                        modifyBar.SetBool("Modify", true);
                    }
                }
            }
        }
        //When the space bar is pressed, the menu bar appears, then disappears when pressed again
        if (Input.GetKeyDown(KeyCode.Space))
        {
            selectedMonster = null;
            isModify = false;
            menuBar.SetBool("Options", !menuBar.GetBool("Options"));
            optionBar.SetBool("Menu", false);
        }
        essenceCount.text = GameManager.dungeonEssence.ToString("n2");
        monsterChoice.text = choices[choiceNum];
    }

    public MonsterScript GetMonster(GameObject monster)
    {
        return monster.GetComponent<MonsterScript>();
    }

    public void SetSelectedMonster(GameObject monster)
    {
        selectedMonster = monster;
        optionBar.SetBool("Menu", false);
    }

    public void Modify()
    {
        isModify = true;
    }

    public void MoveMonster()
    {
        if (modifyMonster)
        {
            modifyMonster.transform.SetParent(transform);
            isModify = false;
            modifyBar.SetBool("Modify", false);
        }
    }

    public void CycleChoice(bool reverse)
    {
        //Changes the choice being displayed as well as changing the choice in the the monster being modified
        if (reverse)
        {
            choiceNum--;
        }else { choiceNum++; }
        if (choiceNum > choices.Count) { choiceNum = 0; }
        if (choiceNum < 0) { choiceNum = choices.Count; }
        if (modifyMonster) { GetMonster(modifyMonster).SetChoice(choiceNum); }
    }

    public void RestoreMonster()
    {
        if (modifyMonster) { GetMonster(modifyMonster).Restore(); }
    }

    public void Options()
    {
        optionBar.SetBool("Menu", !optionBar.GetBool("Menu"));
    }
}
