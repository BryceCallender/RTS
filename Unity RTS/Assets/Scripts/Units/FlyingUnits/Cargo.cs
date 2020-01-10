using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : FlyingUnit
{
    public List<GameObject> loadedObjects;
    public int MAX_LOAD_SIZE = 8;

    private Vector3 unloadPosition;

    [SerializeField]
    private bool loadMode;
    [SerializeField]
    private bool unloadMode;

    protected override void Update()
    {
        base.Update();

        if(UnitIsSelected)
        {
            //Load 
            //if(Input.GetKeyDown(KeyCode.L))
            //{
            //    loadMode = true;
            //    unloadMode = false;
            //}

            //Unload
            if(Input.GetKeyDown(KeyCode.D))
            {
                unloadMode = true;
                loadMode = false;
            }

            if (unloadMode)
                UnloadMode();
        }
    }


    public bool Load(GameObject gameObject)
    {
        int unitLoadSize = gameObject.GetComponent<Unit>().loadSize;

        //If our current load cant take this unit then dont load it
        if (GetCurrentLoad() + unitLoadSize > MAX_LOAD_SIZE)
            return false;

        gameObject.SetActive(false); //Get rid of them in the space
        loadedObjects.Add(gameObject); //Add a reference to this unit into the Cargo ship list
        return true;
    }

    public void Unload(GameObject gameObject)
    {
        unloadPosition = transform.position;
        unloadPosition.y = 0;

        //Place the gameobject at the respective position
        gameObject.transform.position = unloadPosition;

        //Reveal the gameobject so that it has truly been unloaded
        gameObject.SetActive(true);

        loadedObjects.Remove(gameObject);
    }

    public void UnloadAll()
    {
        foreach(GameObject gameObject in loadedObjects)
        {
            unloadPosition = transform.position + Random.insideUnitSphere * Mathf.Sqrt(Random.Range(0.0f, 3.0f));
            unloadPosition.y = 0;

            //Place the gameobject at the respective position
            gameObject.transform.position = unloadPosition;

            //Reveal the gameobject so that it has truly been unloaded
            gameObject.SetActive(true);
        }

        loadedObjects.Clear();
    }

    public void UnloadMode()
    {
        UnloadAll();
    }

    private int GetCurrentLoad()
    {
        int loadSize = 0;
        foreach(GameObject gameObject in loadedObjects)
        {
            loadSize += gameObject.GetComponent<Unit>().loadSize;
        }

        return loadSize;
    }

    protected override void Fire()
    {
        //Do nothing this cannot fire!
    }
}
