using UnityEngine;
using System.Collections;
//物体抖动
public class ObjectShake : MonoBehaviour
{

	public Vector3 positionShake;//震动幅度
	public Vector3 angleShake;   //震动角度
	public float cycleTime = 0.2f;//震动周期
	public int cycleCount = 6;    //震动次数
	public bool fixShake = false; //为真时每次幅度相同，反之则递减
	public bool unscaleTime = false;//不考虑缩放时间
	public bool bothDir = true;//双向震动
	public float fCycleCount = 0;//设置此参数，以此震动次数为主
	public bool autoDisable = true;//自动disbale


	float currentTime;
	int curCycle;
	Vector3 curPositonShake;
	Vector3 curAngleShake;
	float curFovShake;
	Vector3 startPosition;
	Vector3 startAngles;
	Transform myTransform;

	void OnEnable()
	{
		currentTime = 0f;
		curCycle = 0;
		curPositonShake = positionShake;
		curAngleShake = angleShake;
		myTransform = transform;
		startPosition = myTransform.localPosition;
		startAngles = myTransform.localEulerAngles;
		if (fCycleCount > 0)
			cycleCount = Mathf.RoundToInt(fCycleCount);
	}

	void OnDisable()
	{
		myTransform.localPosition = startPosition;
		myTransform.localEulerAngles = startAngles;
	}

	// Update is called once per frame
	void Update()
	{

#if UNITY_EDITOR
		if (fCycleCount > 0)
			cycleCount = Mathf.RoundToInt(fCycleCount);
#endif

		if (curCycle >= cycleCount)
		{
			if (autoDisable)
				enabled = false;
			return;
		}

		float deltaTime = unscaleTime ? Time.unscaledDeltaTime : Time.deltaTime;
		currentTime += deltaTime;
		while (currentTime >= cycleTime)
		{
			currentTime -= cycleTime;
			curCycle++;
			if (curCycle >= cycleCount)
			{
				myTransform.localPosition = startPosition;
				myTransform.localEulerAngles = startAngles;
				return;
			}

			if (!fixShake)
			{
				if (positionShake != Vector3.zero)
					curPositonShake = (cycleCount - curCycle) * positionShake / cycleCount;
				if (angleShake != Vector3.zero)
					curAngleShake = (cycleCount - curCycle) * angleShake / cycleCount;
			}
		}

		if (curCycle < cycleCount)
		{
			float offsetScale = Mathf.Sin((bothDir ? 2 : 1) * Mathf.PI * currentTime / cycleTime);
			if (positionShake != Vector3.zero)
				myTransform.localPosition = startPosition + curPositonShake * offsetScale;
			if (angleShake != Vector3.zero)
				myTransform.localEulerAngles = startAngles + curAngleShake * offsetScale;
		}
	}
	//重置
	public void Restart()
	{
		if (enabled)
		{
			currentTime = 0f;
			curCycle = 0;
			curPositonShake = positionShake;
			curAngleShake = angleShake;
			myTransform.localPosition = startPosition;
			myTransform.localEulerAngles = startAngles;
			if (fCycleCount > 0)
				cycleCount = Mathf.RoundToInt(fCycleCount);
		}
		else
			enabled = true;
	}
}
