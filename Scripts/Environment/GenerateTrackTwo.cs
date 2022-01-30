using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrackTwo : MonoBehaviour
{
	public GameObject[] prefabSections;

	public Transform playerTransform;
	private float spawnZ = 0.0f;
	public float sectionScalerZ;
	private float sectionLength = 60.0f;
	private int amnSectionsOnScreen = 7;
	private float safeZone = 90.0f;
	private int lastSectionIndex;
	private int difficultyModifier = 1;
	private int maxDifficulty = 4;
	private float time = 0f;
	public float difficultyTime = 30;

	private Queue<GameObject> activeSections;

	void Start()
	{
		//sectionScalerZ = transform.localScale.z;
		sectionLength *= sectionScalerZ;
		activeSections = new Queue<GameObject>();
		SpawnSection(0);
		for (int i = 1; i < amnSectionsOnScreen; i++)
		{
			SpawnSection();
		}
	}

	void Update()
	{
		if (playerTransform.position.z - (safeZone * sectionScalerZ) > (spawnZ - amnSectionsOnScreen * sectionLength))
		{
			SpawnSection();
			DeleteSection();
		}
		time += Time.deltaTime;
		if (time % 60 >= difficultyTime)
		{
			IncreaseDifficulty();
			time = 0f;
		}
	}

	private void SpawnSection(int prefabIndex = -1)
	{
		GameObject newSection;
		if (prefabIndex != -1)
			newSection = Instantiate(prefabSections[prefabIndex]);
		else
			newSection = Instantiate(prefabSections[RandomPrefabIndex()]);
		newSection.transform.SetParent(transform);
		newSection.transform.position = new Vector3(transform.position.x,0,spawnZ);
		newSection.transform.localScale = new Vector3(1, 1, newSection.transform.localScale.z * sectionScalerZ);
		spawnZ += sectionLength;
		activeSections.Enqueue(newSection);
	}

	private void DeleteSection()
	{
		Destroy(activeSections.Dequeue());
	}

	private int RandomPrefabIndex()
	{
		if (prefabSections.Length <= 1)
			return 0;
		int randomIndex;
		do
		{
			randomIndex = Random.Range(1, (3 * difficultyModifier));
		} while (randomIndex == lastSectionIndex);
		lastSectionIndex = randomIndex;
		return randomIndex;
	}

	private void IncreaseDifficulty()
	{
		Debug.Log("Difficulty Increased");
		if (difficultyModifier == maxDifficulty)
			return;
		difficultyModifier += 1;
	}
}
