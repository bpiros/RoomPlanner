﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadScrollViewScript : MonoBehaviour {

	public GameObject savedRoomPanel;
	public GameObject parentOfSavedRoomPanel;
	public bool isListUpdated = false;

	void Start(){
		ListFolderNames ();
	}

	public void ListFolderNames(){
		DeactivateWalls ();
		DeleteSavedRoomPanelObjects ();

		string[] folders = Directory.GetDirectories (Application.persistentDataPath);

		foreach (string folder in folders) {
			string folderName = folder.Substring (folder.LastIndexOf ("/")+1);
			if (!(folderName == "Unity" || folderName == "LastEditedRoom")) {
				GameObject newPanel = Instantiate (savedRoomPanel,parentOfSavedRoomPanel.transform);
				newPanel.GetComponentInChildren<Text> ().text = folderName;
			}
		}
	}

	private void DeleteSavedRoomPanelObjects(){
		int parentChildCount=parentOfSavedRoomPanel.transform.childCount;
		for (int i = 0; i < parentChildCount; i++) {
			Destroy (parentOfSavedRoomPanel.transform.GetChild (i).gameObject);
		}
	}

	private void DeactivateWalls(){
		GameObject room = GameObject.FindGameObjectWithTag ("Room");
		for (int i = 0; i < room.transform.childCount; i++) {
			Transform wall=room.transform.GetChild (i);
			WallScript wallScript = wall.gameObject.GetComponent<WallScript> ();
			if (wallScript.IsSelected ()) {
				wallScript.SetInactive ();
			}

		}
	}
}
