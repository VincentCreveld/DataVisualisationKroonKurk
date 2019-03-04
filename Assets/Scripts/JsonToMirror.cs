using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System;

public enum BCType { broadcasting, collective }

public class JsonToMirror : MonoBehaviour
{
	public TextAsset jsonString, jsonString2;
	private string str, str2;
	private MirrorStorage jsonMirrorStorage;

	private List<Vector2> positions;

	public List<RefinedDataStorageClass> leader, story, listener, planner, none;
	public Vector2 avgNone, avgLeader, avgStory, avgPlanner, avgListener;

	public List<RefinedDataStorageClass> refinedData;

	private BCType[] bcTypeOrder = new BCType[20]
	{
		BCType.broadcasting,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.collective,
		BCType.broadcasting,
		BCType.broadcasting,
		BCType.broadcasting,
		BCType.broadcasting,
		BCType.collective,
		BCType.broadcasting,
		BCType.broadcasting,
		BCType.broadcasting,
		BCType.broadcasting,
		BCType.broadcasting
	};

	// Use this for initialization
	private void Start ()
	{
		str = Encoding.UTF7.GetString(jsonString.bytes);
		str2 = Encoding.UTF7.GetString(jsonString2.bytes);
		SetupJson();
	}

	[ContextMenu("AAAAAAAAAAAAAAAA")]
	private void SetupJson()
	{
		jsonMirrorStorage = JsonConvert.DeserializeObject<MirrorStorage>(str);
		jsonMirrorStorage.SetupAxisVals();
		MirrorStorage ms = JsonConvert.DeserializeObject<MirrorStorage>(str2);
		ms.SetupAxisVals();
		jsonMirrorStorage.results.AddRange(ms.results);
		jsonMirrorStorage.axisVals.AddRange(ms.axisVals);
		GetPoints();

		SetupRefinedData();
		GetAverages();
	}

	private void GetAverages()
	{
		leader = new List<RefinedDataStorageClass>();
		story = new List<RefinedDataStorageClass>();
		listener = new List<RefinedDataStorageClass>();
		planner = new List<RefinedDataStorageClass>();
		none = new List<RefinedDataStorageClass>();

		foreach (var item in refinedData)
		{
			switch (item.personaType)
			{
				case PersonaType.none:
					none.Add(item);
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
		foreach (var item in none)
		{
			avgNone += item.pointOnAxis;
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

		avgNone = VectorAverage(none.Count, avgNone);
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
