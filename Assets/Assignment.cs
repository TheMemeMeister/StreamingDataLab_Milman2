
/*
This RPG data streaming assignment was created by Fernando Restituto.
Pixel RPG characters created by Sean Browning.
*/

using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{
    const int PartyCharacterSaveDataSignifier = 0;
    const int EquipmentSaveDataSignifier = 1;
    static public void SavePartyButtonPressed()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + "Party.txt");
        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            Debug.Log("Writing Character Stats Here");
            sw.WriteLine(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health + "," + pc.mana
                     + "," + pc.strength + "," + pc.agility + "," + pc.wisdom); // 0 on .txt = new save 
            foreach (int equipID in pc.equipment)
            {
                sw.WriteLine(EquipmentSaveDataSignifier + "," + equipID);

            }
        }



        sw.Close();
        Debug.Log("Saved Pressed");
    }

    static public void LoadPartyButtonPressed()
    {

        GameContent.partyCharacters.Clear();
       
        StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + "Party.txt");
        string line;

        while ((line = sr.ReadLine()) != null)

        {

            Debug.Log(line);
            string[] csv = line.Split(','); //splitting the lines by the commans, hence csv
            Debug.Log(csv[0]); //double checking csv
            int saveDataSignifier = int.Parse(csv[0]); //First time using signifiers

            if (saveDataSignifier == PartyCharacterSaveDataSignifier) //it is new character
            {
                PartyCharacter pc = new PartyCharacter(int.Parse(csv[1]), int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6])); //changed parse to start at 1 
                GameContent.partyCharacters.AddLast(pc);
            }
            else if (saveDataSignifier == EquipmentSaveDataSignifier)
            {
                GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1])); //Use 1 for equipment 
            }

            GameContent.RefreshUI();

        }

    }

}
#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{
    static List<string> partylist = new List<string>();

    const int PartyCharacterSaveDataSignifier = 0;
    const int EquipmentSaveDataSignifier = 1;


    
    static public void GameStart()
    {
        DirectoryInfo partydirectory = new DirectoryInfo("PartySaves"); // makes a directory of the party saves folder 
        foreach (FileInfo checkfile in partydirectory.GetFiles()) //checks each file in the folder 
        {
            partylist.Add(checkfile.Name); // addingthe info from the files into my party list slots 
        }

        GameContent.RefreshUI();

    }

    static public List<string> GetListOfPartyNames()
    {

        //DirectoryInfo partydirectory = new DirectoryInfo("PartySaves"); // makes a directory of the party saves folder 
        //foreach (FileInfo checkfile in partydirectory.GetFiles()) //checks each file in the folder 
        //{
        //    partylist.Add(checkfile.Name); // addingthe info from the files into my party list slots 
        //}

        //return new List<string>();
        return partylist;
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        //string UserInput = GameObject.Find("PartyNameInputField").GetComponent<InputField>().text;
        
        GameContent.partyCharacters.Clear();
        // StreamReader sr = new StreamReader("PartySaves/MagesTheif.txt"); 
        Debug.Log("PartySaves/" + selectedName);
        StreamReader sr = new StreamReader("PartySaves/" + selectedName);
        //StreamReader sr2 = new StreamReader(GetListOfPartyNames().FindIndex(1).ToString + ".txt");

        string line;

        while ((line = sr.ReadLine()) != null)

        {

            Debug.Log(line);
            string[] csv = line.Split(','); //splitting the lines by the commans, hence csv
            Debug.Log(csv[0]); //double checking csv
            int saveDataSignifier = int.Parse(csv[0]); //First time using signifiers

            if (saveDataSignifier == PartyCharacterSaveDataSignifier) //it is new character
            {
                PartyCharacter pc = new PartyCharacter(int.Parse(csv[1]), int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6])); //changed parse to start at 1 
                GameContent.partyCharacters.AddLast(pc);
            }
            else if (saveDataSignifier == EquipmentSaveDataSignifier)
            {
                GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1])); //Use 1 for equipment 
            }

            GameContent.RefreshUI();
            
        }
        sr.Close();
    }

    static public void SavePartyButtonPressed()
    {
        string UserInput = GameObject.Find("PartyNameInputField").GetComponent<InputField>().text;
        StreamWriter sw = new StreamWriter("PartySaves/"+ UserInput +".txt");
        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            Debug.Log("Writing Character Stats Here");
            sw.WriteLine(PartyCharacterSaveDataSignifier + "," + pc.classID + "," + pc.health + "," + pc.mana
                     + "," + pc.strength + "," + pc.agility + "," + pc.wisdom); // 0 on .txt = new character 
            foreach (int equipID in pc.equipment) //for each loop causes the equipment to appear on new line. 
            {
                sw.WriteLine(EquipmentSaveDataSignifier + "," + equipID);

            }
        }

        partylist.Add(UserInput + ".txt");

        sw.Close();
        Debug.Log("Saved Pressed");
        GameContent.RefreshUI();
    }

    static public void NewPartyButtonPressed()
    {
        //Reroll or load a default save here? 
    }

    static public void DeletePartyButtonPressed(string selectedName)
    {
      
        //DirectoryInfo partydirectory = new DirectoryInfo("PartySaves/" + selectedName); // makes a directory of the party saves folder 
                                                                                        //foreach (FileInfo file in partydirectory.EnumerateFiles(selectedName))
                                                                                        //{
                                                                                        //    file.Delete();
                                                                                        //}
                                                                                        //foreach (DirectoryInfo dir in partydirectory.EnumerateDirectories(selectedName))
                                                                                        //{
                                                                                        //    dir.Delete(true);
                                                                                        //}

        File.Delete("PartySaves/" + selectedName);
        Debug.Log("Delete Pressed " + selectedName + " deleted");
        // partydirectory.Delete(selectedNam);



        partylist.Remove(selectedName);
        //Debug.Log("Delete Pressed, should be no text");
        GameContent.RefreshUI();
        
    }

}

#endregion


