using UnityEngine;
using System.Collections;

[ExecuteAlways]  // Drawing outside playmode

public class Lines : MonoBehaviour
{

	[SerializeField] GameObject gaussObj;
	LineRenderer gauss;

	[SerializeField] GameObject fallAndRiseObj;
	LineRenderer fallAndRise;

	[SerializeField] GameObject constantObj;
	LineRenderer constant;

	[SerializeField] public int gameModeId = 0;
	[SerializeField] int pointCount = 30;  // Vertex count
	[SerializeField] float length = 10;      // Line length
	[SerializeField] float width = 0.15f;      // Line width
	[SerializeField] float startX = -9;			//X location of first point
	//[SerializeField] float startY = -6;         //Y location of first point
	[SerializeField] float amplitude = 1;		//Height
	[SerializeField] float radius = 1;
	[SerializeField] int place = 1;
	public float startY;
	[SerializeField] Color activeColor = Color.yellow;
	[SerializeField] Color inactiveColor = Color.gray;
	[SerializeField] float inactiveAlpha = 0.5f;
	Gradient activeGradient = new Gradient();
	Gradient inactiveGradient = new Gradient();

	bool crRunning = false;


	void Start()
    {
		gameModeId = 0;

		gauss = gaussObj.GetComponent<LineRenderer>();
		fallAndRise = fallAndRiseObj.GetComponent<LineRenderer>();
		constant = constantObj.GetComponent<LineRenderer>();

		activeGradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(activeColor, 0.0f), new GradientColorKey(activeColor, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
			);
		inactiveGradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(inactiveColor, 0.0f), new GradientColorKey(inactiveColor, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(inactiveAlpha, 0.0f), new GradientAlphaKey(inactiveAlpha, 1.0f) }
			);
		
	}

    void Update()
	{
		startY = -(amplitude / 2);

		if (gameModeId == 0)
        {
			StopCoroutine(RandomMode());
			crRunning = false;
			DrawConstant(false, place);
			DrawGauss(false, place);
			DrawFallAndRise(false, place);
		}
		else if (gameModeId == 1)
        {
			StopCoroutine(RandomMode());
			crRunning = false;
			DrawConstant(true, place);
			DrawGauss(false, place);
			DrawFallAndRise(false, place);
		}
		else if (gameModeId == 2)
        {
			StopCoroutine(RandomMode());
			crRunning = false;
			DrawConstant(false, place);
			DrawGauss(true, place);
			DrawFallAndRise(false, place);
		}
        else if (gameModeId == 3)
        {
			StopCoroutine(RandomMode());
			crRunning = false;
			DrawConstant(false, place);
			DrawGauss(false, place);
			DrawFallAndRise(true, place);
		}
		else
        {
			if (!crRunning)
				StartCoroutine(RandomMode());
		}
	}	
	void DrawConstant(bool active, int place)
	{
		//Constant
		constant.positionCount = 2;
		for (int i = 0; i < 2; i++)
		{
			float x = startX + i * length;
			float y = startY + amplitude / 2;
			Vector2 p = new(x, y);
			constant.SetPosition(i, p);
			if (active)
			{
				constant.colorGradient = activeGradient;
				constant.sortingOrder = place + 1;
			}
			else
			{
				constant.colorGradient = inactiveGradient;
				constant.sortingOrder = place;
			}
			constant.startWidth = width;
			constant.endWidth = width;
		}
	}
	void DrawGauss(bool active, int place)
	{
		//Gauss
		gauss.positionCount = pointCount;
		for (int i = 0; i < pointCount; i++)
		{
			float x = startX + i * length / pointCount;
			float y = startY + amplitude * Mathf.Exp(-(Mathf.Pow((x - startX - length / 2), 2) / (2 * Mathf.Pow(radius, 2))));
			Vector2 p = new(x, y);
			gauss.SetPosition(i, p);
			if (active)
			{
				gauss.colorGradient = activeGradient;
				gauss.sortingOrder = place + 1;
			}
			else
			{
				gauss.colorGradient = inactiveGradient;
				gauss.sortingOrder = place;
			}			
			gauss.startWidth = width;
			gauss.endWidth = width;
		}
	}
	void DrawFallAndRise(bool active, int place)
	{
		//FallAndRise
		fallAndRise.positionCount = pointCount;
		for (int i = 0; i < (pointCount / 2); i++)
		{
			float x = startX + i * length / pointCount;
			float y = startY + amplitude * Mathf.Exp(-(Mathf.Pow((x - startX), 2) / (2 * Mathf.Pow(radius, 2))));
			Vector2 p = new(x, y);
			fallAndRise.SetPosition(i, p);
		}
		for (int i = (pointCount / 2); i < pointCount; i++)
		{
			float x = startX + i * length / pointCount;
			float y = startY + amplitude * Mathf.Exp(-(Mathf.Pow(x - (length + startX), 2) / (2 * Mathf.Pow(radius, 2))));
			Vector2 p = new(x, y);
			fallAndRise.SetPosition(i, p);
		}
		if (active)
		{
			fallAndRise.colorGradient = activeGradient;
			fallAndRise.sortingOrder = place + 1;
		}
		else
        {
			fallAndRise.colorGradient = inactiveGradient;
			fallAndRise.sortingOrder = place;
		}			
		fallAndRise.startWidth = width;
		fallAndRise.endWidth = width;
	}

	IEnumerator RandomMode()
	{
		crRunning = true;
		const float flickTime = 0.15f;
		int i = 1;		
		while (gameModeId == 4)
		{
			if (i % 2 == 0)
            {
				DrawConstant(false, place);
				DrawGauss(true, place);
				DrawFallAndRise(false, place);
			}
			else if (i % 3 == 0)
            {
				DrawConstant(false, place);
				DrawGauss(false, place);
				DrawFallAndRise(true, place);
			}
            else
            {
				DrawConstant(true, place);
				DrawGauss(false, place);
				DrawFallAndRise(false, place);
			}
			i++;
			yield return new WaitForSeconds(flickTime);
		}
		crRunning = false;
	}

	public void enableLines(bool enabled)
    {
		LineRenderer[] lines = gameObject.GetComponentsInChildren<LineRenderer>();
		foreach (LineRenderer line in lines)
		{
			line.enabled = enabled;
		}
		return;
	}



}
