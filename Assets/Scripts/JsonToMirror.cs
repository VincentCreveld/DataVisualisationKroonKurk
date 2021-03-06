﻿using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System;
using UnityEngine.UI;

public enum BCType { broadcasting, collective }

public class JsonToMirror : MonoBehaviour
{
	public TextAsset jsonString, jsonString2, jsonString3;
	private string str, str2, str3;
	private MirrorStorage jsonMirrorStorage;

	private List<Vector2> positions;

	public GameObject nodePrefab;

	public List<RefinedDataStorageClass> leader, story, listener, planner, odds;
	public Vector2 avgOdds, avgLeader, avgStory, avgPlanner, avgListener;
	private List<GameObject> oddsObjs, leaderObjs, storyObjs, listenerObjs, plannerObjs, allObjs, avgObjs;

	public List<RefinedDataStorageClass> refinedData;
	public Text totalText, storyText, leaderText, plannerText, listenerText, oddsText;
	public Text individualText, collectiveText, absorbingText, broadcastingText;

	private float storyPct, leaderPct, plannerPct, listenerPct, oddsPct;
	private float individualPct, collectivePct, absorbingPct, broadcastingPct;

	public Vector2 totalAvg;

	public HackyFOVCaster leaderFOV, storyFOV, listenerFOV, plannerFOV, oddsFOV;
	public GameObject pieChartParent;


	private BCType[] bcTypeOrder = new BCType[20]
	{
		BCType.broadcasting,	//q1
		BCType.collective,		//q2
		BCType.collective,		//q3
		BCType.collective,		//q4
		BCType.collective,		//q5
		BCType.collective,		//q6
		BCType.collective,		//q7
		BCType.collective,		//q8
		BCType.collective,		//q9
		BCType.collective,		//q10
		BCType.broadcasting,	//q11
		BCType.broadcasting,	//q12
		BCType.broadcasting,	//q13
		BCType.broadcasting,	//q14
		BCType.collective,		//q15
		BCType.broadcasting,	//q16
		BCType.broadcasting,	//q17
		BCType.broadcasting,	//q18
		BCType.broadcasting,	//q19
		BCType.broadcasting		//q20
	};

	private void Start ()
	{
		str = Encoding.UTF7.GetString(jsonString.bytes);
		str2 = Encoding.UTF7.GetString(jsonString2.bytes);
		str3 = Encoding.UTF7.GetString(jsonString3.bytes);
		SetupJson();
	}

	// Ik haat mezelf voor deze code.
	[ContextMenu("AAAAAAAAAAAAAAAA")]
	private void SetupJson()
	{
		jsonMirrorStorage = JsonConvert.DeserializeObject<MirrorStorage>(str);
		jsonMirrorStorage.SetupAxisVals();
		MirrorStorage ms = JsonConvert.DeserializeObject<MirrorStorage>(str2);
		ms.SetupAxisVals();
		jsonMirrorStorage.results.AddRange(ms.results);
		jsonMirrorStorage.axisVals.AddRange(ms.axisVals);
		ms = JsonConvert.DeserializeObject<MirrorStorage>(str3);
		ms.SetupAxisVals();
		jsonMirrorStorage.results.AddRange(ms.results);
		jsonMirrorStorage.axisVals.AddRange(ms.axisVals);

		GetPoints();

		SetupRefinedData();
		GetAverages();

		PlaceNodes();
		SetupUIText();

		GetTotalAvg();
		InstantiateAverages();
	}

	private void GetTotalAvg()
	{
		float x = 0, y = 0;

		foreach (var item in refinedData)
		{
			x += item.pointOnAxis.x;
			y += item.pointOnAxis.y;
		}

		x /= refinedData.Count;
		y /= refinedData.Count;

		totalAvg = new Vector2(x, y);
	}

	private void InstantiateAverages()
	{
		avgObjs = new List<GameObject>();

		GameObject go = Instantiate(nodePrefab, totalAvg, Quaternion.identity);
		go.GetComponent<MeshRenderer>().material.color = Color.gray;
		go.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
		avgObjs.Add(go);
		allObjs.Add(go);

		go = Instantiate(nodePrefab, avgOdds, Quaternion.identity);
		go.GetComponent<MeshRenderer>().material.color = Color.black;
		go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		avgObjs.Add(go);
		allObjs.Add(go);
		oddsObjs.Add(go);

		go = Instantiate(nodePrefab, avgStory, Quaternion.identity);
		go.GetComponent<MeshRenderer>().material.color = Color.green;
		go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		avgObjs.Add(go);
		allObjs.Add(go);
		storyObjs.Add(go);

		go = Instantiate(nodePrefab, avgLeader, Quaternion.identity);
		go.GetComponent<MeshRenderer>().material.color = Color.blue;
		go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		avgObjs.Add(go);
		allObjs.Add(go);
		leaderObjs.Add(go);

		go = Instantiate(nodePrefab, avgListener, Quaternion.identity);
		go.GetComponent<MeshRenderer>().material.color = Color.magenta;
		go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		avgObjs.Add(go);
		allObjs.Add(go);
		listenerObjs.Add(go);

		go = Instantiate(nodePrefab, avgPlanner, Quaternion.identity);
		go.GetComponent<MeshRenderer>().material.color = Color.yellow;
		go.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
		avgObjs.Add(go);
		allObjs.Add(go);
		plannerObjs.Add(go);
	}

	//Calculates and rounds percentages to two decimal place.
	private void SetupUIText()
	{
		storyPct = Mathf.Round(ExtensionFunctions.Map(storyObjs.Count, 0, allObjs.Count, 0, 100) * 100) / 100;
		leaderPct = Mathf.Round(ExtensionFunctions.Map(leaderObjs.Count, 0, allObjs.Count, 0, 100) * 100) / 100;
		plannerPct = Mathf.Round(ExtensionFunctions.Map(plannerObjs.Count, 0, allObjs.Count, 0, 100) * 100) / 100;
		listenerPct = Mathf.Round(ExtensionFunctions.Map(listenerObjs.Count, 0, allObjs.Count, 0, 100) * 100) / 100;
		oddsPct = Mathf.Round(ExtensionFunctions.Map(oddsObjs.Count, 0, allObjs.Count, 0, 100) * 100) / 100;

		broadcastingPct = storyPct + leaderPct;
		absorbingPct = plannerPct + listenerPct;

		collectivePct = leaderPct + listenerPct;
		individualPct = storyPct + plannerPct;

		totalText.text = allObjs.Count + " - 100%";
		storyText.text = storyObjs.Count + " - " + storyPct + "%";
		leaderText.text = leaderObjs.Count + " - " + leaderPct + "%";
		plannerText.text = plannerObjs.Count + " - " + plannerPct + "%";
		listenerText.text = listenerObjs.Count + " - " + listenerPct + "%";
		oddsText.text = oddsObjs.Count + " - " + oddsPct + "%";

		broadcastingText.text = (storyObjs.Count + leaderObjs.Count) + " - " + broadcastingPct + "%";
		absorbingText.text = (plannerObjs.Count + listenerObjs.Count) + " - " + absorbingPct + "%";
		collectiveText.text = (leaderObjs.Count + listenerObjs.Count) + " - " + collectivePct + "%";
		individualText.text = (storyObjs.Count + plannerObjs.Count) + " - " + individualPct + "%";

		float totalAngle = 0;

		storyFOV.viewAngle = ExtensionFunctions.Map(storyPct, 0, 100, 0, 360);
		storyFOV.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, totalAngle + storyFOV.viewAngle / 2, 0));
		storyFOV.DrawFieldOfView();
		totalAngle += storyFOV.viewAngle;

		leaderFOV.viewAngle = ExtensionFunctions.Map(leaderPct, 0, 100, 0, 360);
		leaderFOV.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, totalAngle + leaderFOV.viewAngle / 2, 0));
		leaderFOV.DrawFieldOfView();
		totalAngle += leaderFOV.viewAngle;

		listenerFOV.viewAngle = ExtensionFunctions.Map(listenerPct, 0, 100, 0, 360);
		listenerFOV.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, totalAngle + listenerFOV.viewAngle / 2, 0));
		listenerFOV.DrawFieldOfView();
		totalAngle += listenerFOV.viewAngle;

		plannerFOV.viewAngle = ExtensionFunctions.Map(plannerPct, 0, 100, 0, 360);
		plannerFOV.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, totalAngle + plannerFOV.viewAngle / 2, 0));
		plannerFOV.DrawFieldOfView();
		totalAngle += plannerFOV.viewAngle;

		float pct = storyPct + leaderPct + listenerPct + plannerPct;
		oddsFOV.viewAngle = ExtensionFunctions.Map((100 - pct), 0, 100, 0, 360);
		oddsFOV.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, totalAngle + oddsFOV.viewAngle / 2, 0));
		oddsFOV.DrawFieldOfView();

		pieChartParent.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
	}

	private void PlaceNodes()
	{
		leaderObjs = new List<GameObject>();
		storyObjs = new List<GameObject>();
		listenerObjs = new List<GameObject>();
		plannerObjs = new List<GameObject>();
		oddsObjs = new List<GameObject>();
		allObjs = new List<GameObject>();

		GameObject obj;

		foreach (var data in refinedData)
		{
			obj = Instantiate(nodePrefab, data.pointOnAxis, Quaternion.identity);
			//obj.SetActive(false);

			switch (data.personaType)
			{
				case PersonaType.none:
					oddsObjs.Add(obj);
					break;
				case PersonaType.storyteller:
					storyObjs.Add(obj);
					break;
				case PersonaType.leader:
					leaderObjs.Add(obj);
					break;
				case PersonaType.planner:
					plannerObjs.Add(obj);
					break;
				case PersonaType.listener:
					listenerObjs.Add(obj);
					break;
			}
			obj.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
			allObjs.Add(obj);
		}
	}

	private void GetAverages()
	{
		leader = new List<RefinedDataStorageClass>();
		story = new List<RefinedDataStorageClass>();
		listener = new List<RefinedDataStorageClass>();
		planner = new List<RefinedDataStorageClass>();
		odds = new List<RefinedDataStorageClass>();

		foreach (var item in refinedData)
		{
			switch (item.personaType)
			{
				case PersonaType.none:
					odds.Add(item);
					break;
				case PersonaType.storyteller:
					story.Add(item);
					break;
				case PersonaType.leader:
					leader.Add(item);
					break;
				case PersonaType.planner:
					planner.Add(item);
					break;
				case PersonaType.listener:
					listener.Add(item);
					break;
			}
		}
		foreach (var item in odds)
		{
			avgOdds += item.pointOnAxis;
		}
		foreach (var item in story)
		{
			avgStory += item.pointOnAxis;
		}
		foreach (var item in leader)
		{
			avgLeader += item.pointOnAxis;
		}
		foreach (var item in planner)
		{
			avgPlanner += item.pointOnAxis;
		}
		foreach (var item in listener)
		{
			avgListener += item.pointOnAxis;
		}

		avgOdds = VectorAverage(odds.Count, avgOdds);
		avgStory = VectorAverage(story.Count, avgStory);
		avgLeader = VectorAverage(leader.Count, avgLeader);
		avgPlanner = VectorAverage(planner.Count, avgPlanner);
		avgListener = VectorAverage(listener.Count, avgListener);
	}

	private void GetPoints()
	{
		positions = new List<Vector2>();
		for (int i = 0; i < jsonMirrorStorage.results.Count; i++)
		{
			positions.Add(GetPoint(jsonMirrorStorage.axisVals[i]));
		}
	}

	private Vector2 VectorAverage(int total, Vector2 target)
	{
		return new Vector2(target.x / total, target.y / total);
	}

	private Vector2 GetPoint(int[] placing)
	{
		Vector2 result = Vector2.zero;

		for (int i = 0; i < placing.Length; i++)
		{
			if (bcTypeOrder[i] == BCType.collective)
				result.x += ExtensionFunctions.Map(placing[i], 1, 9, -2, 2);
			else
				result.y += ExtensionFunctions.Map(placing[i], 1, 9, -2, 2);
		}

		return result;
	}

	private void SetupRefinedData()
	{
		refinedData = new List<RefinedDataStorageClass>();

		for (int i = 0; i < jsonMirrorStorage.results.Count; i++)
		{
			PersonaType t = GetPersonaType(positions[i]);
			int a = jsonMirrorStorage.results[i].o2;
			string g = jsonMirrorStorage.results[i].o3;
			bool grown = (jsonMirrorStorage.results[i].o6 == "Ja");
			Vector2 p = positions[i];

			refinedData.Add(new RefinedDataStorageClass(t, a, g, grown, p));
		}
	}

	private PersonaType GetPersonaType(Vector2 point)
	{
		PersonaType returnVal = PersonaType.none;

		if (point.x > 0 && point.y > 0)
			returnVal = PersonaType.leader;
		else if (point.x > 0 && point.y < 0)
			returnVal = PersonaType.listener;
		else if (point.x < 0 && point.y < 0)
			returnVal = PersonaType.planner;
		else if (point.x < 0 && point.y > 0)
			returnVal = PersonaType.storyteller;

		return returnVal;
	}

	public enum DisplayTypes { all, none, togglestoryteller, toggleleader, toggleplanner, togglelistener, onlystory, onlyleader, onlyplanner, onlylistener, onlyneutrals, toggleneutrals, toggleAvg, onlyAvg }

	// Coupled to UI
	// God help this code
	public void ToggleDisplay(int type)
	{
		switch ((DisplayTypes)type)
		{
			case DisplayTypes.all:
				foreach (var item in allObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.none:
				foreach (var item in allObjs)
					item.SetActive(false);
				break;
			case DisplayTypes.togglestoryteller:
				foreach (var item in storyObjs)
					item.SetActive(!item.activeSelf);
				break;
			case DisplayTypes.toggleleader:
				foreach (var item in leaderObjs)
					item.SetActive(!item.activeSelf);
				break;
			case DisplayTypes.toggleplanner:
				foreach (var item in plannerObjs)
					item.SetActive(!item.activeSelf);
				break;
			case DisplayTypes.togglelistener:
				foreach (var item in listenerObjs)
					item.SetActive(!item.activeSelf);
				break;
			case DisplayTypes.onlystory:
				foreach (var item in allObjs)
					item.SetActive(false);
				foreach (var item in storyObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.onlyleader:
				foreach (var item in allObjs)
					item.SetActive(false);
				foreach (var item in leaderObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.onlyplanner:
				foreach (var item in allObjs)
					item.SetActive(false);
				foreach (var item in plannerObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.onlylistener:
				foreach (var item in allObjs)
					item.SetActive(false);
				foreach (var item in listenerObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.onlyneutrals:
				foreach (var item in allObjs)
					item.SetActive(false);
				foreach (var item in oddsObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.toggleneutrals:
				foreach (var item in oddsObjs)
					item.SetActive(!item.activeSelf);
				break;
			case DisplayTypes.onlyAvg:
				foreach (var item in allObjs)
					item.SetActive(false);
				foreach (var item in avgObjs)
					item.SetActive(true);
				break;
			case DisplayTypes.toggleAvg:
				foreach (var item in avgObjs)
					item.SetActive(!item.activeSelf);
				break;
			default:
				Debug.Log("Wrong int used in ui coupling.");
				break;
		}
	}
}

public enum PersonaType { none, storyteller, leader, planner, listener}

[Serializable]
public class RefinedDataStorageClass
{
	public PersonaType personaType;
	public int age;
	public string gender;
	public bool grownUpInNL;
	public Vector2 pointOnAxis;

	public RefinedDataStorageClass(PersonaType t, int a, string g, bool grown, Vector2 val)
	{
		personaType = t;
		age = a;
		gender = g;
		grownUpInNL = grown;
		pointOnAxis = val;
	}
}
