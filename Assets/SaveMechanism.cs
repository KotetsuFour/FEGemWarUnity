using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveMechanism
{
    private static string folderPath = "Saves/";
    public static void saveGame(int savefile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //      string path = Application.persistentDataPath + "/gamefile" + savefile + ".save";
        string path = folderPath + "gamefile" + savefile + ".save";
        FileStream stream = new FileStream(path, FileMode.Create);
        CampaignData.savefile = savefile;
        CampaignSaveData info = new CampaignSaveData();

        formatter.Serialize(stream, info);

        stream.Close();
    }

    public static void loadGame(int savefile, Unit unitToInstantiate)
    {
//        string path = Application.persistentDataPath + "/gamefile" + savefile + ".save";
        string path = folderPath + "gamefile" + savefile + ".save";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CampaignSaveData info = formatter.Deserialize(stream) as CampaignSaveData;
            stream.Close();

            info.unload(unitToInstantiate);
            CampaignData.savefile = savefile;
        }
        else
        {
            Debug.LogError("Could not find file at " + path);
        }
    }

    public static bool findFile(int file)
    {
//        string path = Application.persistentDataPath + "/gamefile" + file + ".save";
        string path = folderPath + "gamefile" + file + ".save";
        return File.Exists(path);
    }

    public static void deleteFile(int file)
    {
//        string path = Application.persistentDataPath + "/gamefile" + file + ".save";
        string path = folderPath + "gamefile" + file + ".save";
        if (File.Exists(path))
        {
            File.Delete(path);
            CampaignData.reset();
        }
    }
}
