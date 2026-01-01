using UnityEngine;
using System.Collections.Generic;
using System;
[System.Serializable]
[CreateAssetMenu(fileName = "Ownable", menuName = "Scriptable Objects/Ownable")]
public class Ownable : ScriptableObject
{

    public Sprite sprite;


    public List<Tag> tags = new List<Tag>() {new Tag("type", "claimable"), new Tag("rarity", "common"), new Tag("hideInInventory", "true")};

    public Ownable(string serialized)
    {
        int firstBar = serialized.IndexOf("|");
        int secondBar = serialized.IndexOf("|", firstBar+1);
        int thirdBar = serialized.IndexOf("|", secondBar+1);

        int fourthBar = serialized.IndexOf("|", thirdBar+1);
        if(firstBar == -1 || secondBar == -1)
        {
            Debug.LogWarning("Serious data corruption. Using default values");
            this.name = "NewOwnable";
            sprite = SpriteManager.instance.placeholder;
        }
        if(thirdBar == -1)
        {
            Debug.LogWarning("Data incomplete: Using default values");
            this.name = "NewOwnable";
            sprite = SpriteManager.instance.placeholder;
            
        }else if(fourthBar != -1)
        {
            Debug.LogWarning("Data possibly corrupted: Using default values");
            this.name = "NewOwnable";
            sprite = SpriteManager.instance.placeholder;
        }
        else
        {
            this.name = serialized.Substring(0, firstBar);
            string spriteName = serialized.Substring(firstBar+1, secondBar - firstBar - 1);
            sprite = SpriteManager.instance.GetSpriteByName(spriteName);
            
            // Debug.Log("Second Bar +1: " + (secondBar + 1));
            // Debug.Log("Third Bar: " + thirdBar);
            // Debug.Log("Serialized Length: " + serialized.Length);

            string combinedNames = serialized.Substring((secondBar + 1), thirdBar - secondBar - 1);
            string combinedValues = serialized.Substring(thirdBar + 1);

            List<string> tagNames = new List<string>();
            List<string> tagValues = new List<string>();


            while(combinedNames.IndexOf(",") != -1 && combinedValues.IndexOf(",") != -1){

                int nameIndex = combinedNames.IndexOf(",");
                int valueIndex = combinedValues.IndexOf(",");

                string nameSegment = combinedNames.Substring(0, nameIndex);
                string valueSegment = combinedValues.Substring(0, valueIndex);

                tagNames.Add(nameSegment);
                tagValues.Add(valueSegment);

                combinedNames = combinedNames.Substring(nameIndex + 1);
                combinedValues = combinedValues.Substring(valueIndex + 1);


            }
            
            tagNames.Add(combinedNames);
            tagValues.Add(combinedValues);


            List<Tag> initialize = new List<Tag>();

            for(int i=0; i<tagNames.Count; i++)
            {
                initialize.Add(new Tag(tagNames[i], tagValues[i]));
            }
            tags = initialize;

        }



    }

    public void LoadFromSerialized(string serialized)
    {
       int firstBar = serialized.IndexOf("|");
        int secondBar = serialized.IndexOf("|", firstBar+1);
        int thirdBar = serialized.IndexOf("|", secondBar+1);

        int fourthBar = serialized.IndexOf("|", thirdBar+1);
        if(firstBar == -1 || secondBar == -1)
        {
            Debug.LogWarning("Serious data corruption. Using default values");
            this.name = "NewOwnable";
            sprite = sprite = SpriteManager.instance.placeholder;
        }
        if(thirdBar == -1)
        {
            Debug.LogWarning("Data incomplete: Using default values");
            this.name = "NewOwnable";
            sprite = sprite = SpriteManager.instance.placeholder;
            
        }else if(fourthBar != -1)
        {
            Debug.LogWarning("Data possibly corrupted: Using default values");
            this.name = "NewOwnable";
            sprite = sprite = SpriteManager.instance.placeholder;
        }
        else
        {
            this.name = serialized.Substring(0, firstBar);
            string spriteName = serialized.Substring(firstBar+1, secondBar - firstBar - 1);
            sprite = SpriteManager.instance.GetSpriteByName(spriteName);
            
            // Debug.Log("Second Bar +1: " + (secondBar + 1));
            // Debug.Log("Third Bar: " + thirdBar);
            // Debug.Log("Serialized Length: " + serialized.Length);

            string combinedNames = serialized.Substring((secondBar + 1), thirdBar - secondBar - 1);
            string combinedValues = serialized.Substring(thirdBar + 1);

            List<string> tagNames = new List<string>();
            List<string> tagValues = new List<string>();


            while(combinedNames.IndexOf(",") != -1 && combinedValues.IndexOf(",") != -1){

                int nameIndex = combinedNames.IndexOf(",");
                int valueIndex = combinedValues.IndexOf(",");

                string nameSegment = combinedNames.Substring(0, nameIndex);
                string valueSegment = combinedValues.Substring(0, valueIndex);

                tagNames.Add(nameSegment);
                tagValues.Add(valueSegment);

                combinedNames = combinedNames.Substring(nameIndex + 1);
                combinedValues = combinedValues.Substring(valueIndex + 1);


            }
            
            tagNames.Add(combinedNames);
            tagValues.Add(combinedValues);


            List<Tag> initialize = new List<Tag>();

            for(int i=0; i<tagNames.Count; i++)
            {
                initialize.Add(new Tag(tagNames[i], tagValues[i]));
            }
            tags = initialize;

        }


    }
    public Ownable(string name, Sprite spr)
    {
        this.name = name;
        sprite = spr;
    }
    virtual public string Serialize()
    {

        if(sprite == null)
        {
            sprite = SpriteManager.instance.placeholder;
        }

        List<string> tagNames = new List<string>();
        List<string> tagValues = new List<string>();
        foreach(Tag tag in tags)
        {
            tagNames.Add(tag.name);
            tagValues.Add(tag.value);
        }


        string combinedNames = string.Join( ",", tagNames.ToArray() );

        string combinedValues = string.Join( ",", tagValues.ToArray() );



        // Debug.Log("Name: " + this.name);
        // Debug.Log("Sprite: " + sprite.name);
        // Debug.Log("Combined Tag Names: " + combinedNames);
        // Debug.Log("Combined Tag Values: " + combinedValues);
        string combined = 
        this.name + 
        "|" + sprite.name + 
        "|" + combinedNames + 
        "|" + combinedValues;
        //Debug.Log("Serialized Ownable: " + combined);
        return combined;

    }

    virtual public string FindTag(string label)
    {
        foreach(Tag t in tags)
        {
            if (t.name.Equals(label))
            {
                return t.value;
            }
        }
        return null;
    }

    virtual public void RemoveTag(string label)
    {
        foreach(Tag t in tags)
        {
            if (t.name.Equals(label))
            {
                tags.Remove(t);
                return;
            }
        }
    }

    virtual public void ModifyTagValue(string label, string value)
    {
        foreach(Tag t in tags)
        {
            if (t.name.Equals(label))
            {
                t.value = value;
                return;
            }
        }
    }

    virtual public void AddTag(string label, string value)
    {
        Tag t = new Tag(label, value);
        tags.Add(t);
    }

    public static Ownable FindOwnableByName(Ownable[] list, string name)
    {
        foreach (Ownable o in list)
        {
            if (o.name.Equals(name))
            {
                return o;
            }
        }
        return null;
    }



    

}
