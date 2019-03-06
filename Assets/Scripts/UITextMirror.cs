using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextMirror : MonoBehaviour {

	private Text text;
	[SerializeField]
	private Text textToCopy;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void Update()
	{
		text.text = textToCopy.text;
	}
}
