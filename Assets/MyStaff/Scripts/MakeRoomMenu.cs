﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DaydreamElements.ClickMenu;


public class MakeRoomMenu : MonoBehaviour {

	public GameObject wall;
	public GameObject room;

	public GameObject topView;
	public GameObject sideView;
	public GameObject loadPanel;
	public GameObject savePanel;
	public GameObject keyboard;
	public GameObject newRoomPanel;

	public Camera mainCamera;
	public ClickMenuTree DefaultMenuTree;
	public ClickMenuTree SelectedWallMenuTree;



	private GameObject selectedWall;

	void Awake(){
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}

	void Start(){
		room.GetComponent<Room> ().Load ();
		gameObject.GetComponent<ClickMenuRoot> ().OnItemSelected += Switcher;
	}

	void Switcher(ClickMenuItem item){

		selectedWall = GameObject.FindGameObjectWithTag ("SelectedWall");

		if (item != null) {
			switch (item.id) {
			case 0: //Home
				room.GetComponent<Room>().Save();
				StartCoroutine (LoadAsyncScene ());
				break;
			case 1: //Add
				if (!room.GetComponent<Room> ().IsAnithingAtSpwanPlace ())
					Instantiate (wall, wall.GetComponent<WallScript> ().startingPosition, Quaternion.identity, room.transform);
				break;
			case 2: //File
				break;
			case 3: //View				
				break;
			case 4: //nothing , Delete volt, azert szedtem ki mert a new room vegul ugyan ezt csinalja, csak az jobban
				/*room.GetComponent<Room> ().DestroyWalls();*/
				break;
			case 5: //Load
				if (!savePanel.activeSelf) {
					SwitchOnOffPanel (loadPanel);
					if (loadPanel.activeSelf) {
						GameObject.Find ("LoadScrollView").GetComponent<LoadScrollViewScript> ().ListFolderNames ();
					}
				}
				break;
			case 6: //Save
				if (!loadPanel.activeSelf) {
					SwitchOnOffPanel (savePanel);
					SwitchOnOffPanel (keyboard);
				}
				break;
			case 7: //TopView				
				SwitchOnOffPanel (topView);
				break;
			case 8: //SideView				
				SwitchOnOffPanel (sideView);
				break;
			case 9: //------nothing (perspView volt itt)
				break;
			case 10: //NewRoom
				Instantiate (newRoomPanel, new Vector3 (0, 0, 0), Quaternion.identity);
				room.GetComponent<Room> ().CalculateRoomsCenter ();
				break;
			case 11: //Move				
				selectedWall.GetComponent<WallScript> ().SwitchMoveablePhysicsScript (true,false);
				break;
			case 12: //Edit wall
				//GameObject.FindGameObjectWithTag("SelectedWall").GetComponent<WallScript>().SwitchMoveablePhysicsScript(false);
				selectedWall.GetComponent<WallScript>().SwitchMoveablePhysicsScript(false,true);
				break;
			case 13: //Delete wall
				selectedWall.GetComponent<WallScript> ().Delete ();
				room.GetComponent<Room> ().CalculateRoomsCenter ();
				room.GetComponent<Room> ().Save ();
				break;
			default:
				break;			
			}
		}
	}

	IEnumerator LoadAsyncScene(){
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync ("Home");

		while(!asyncLoad.isDone){
			yield return null;
		}

	}

	public void ChangeToAlterMenu(bool toAlterMenu){
		if (toAlterMenu) {
			gameObject.GetComponent<ClickMenuRoot> ().menuTree = SelectedWallMenuTree;
		}
		else{
			gameObject.GetComponent<ClickMenuRoot> ().menuTree = DefaultMenuTree;
		}
	}

	private void SwitchOnOffPanel(GameObject panel){
		if(!panel.activeSelf){
			panel.SetActive (true);
		}
		else{
			panel.SetActive (false);
		}
	}
}