using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialColorBlink : MonoBehaviour {

	public float red = 1.0f;
	public float green = 1.0f;
	public float blue = 1.0f;
	public float alpha = 1.0f;

	private MeshRenderer thisMeshRenderer;
	private List<Material> allMaterials = new List<Material>();

	// Use this for initialization
	void Start ()
	{
		thisMeshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (Material thisMaterial in thisMeshRenderer.materials)
		{
			thisMaterial.color = new Color(red, green, blue, alpha);
		}
	}
}
