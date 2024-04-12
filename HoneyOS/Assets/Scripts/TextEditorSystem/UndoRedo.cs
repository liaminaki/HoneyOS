using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UndoRedo
{
    public int myNum;
    public string myString;

    public UndoRedo(int num, string str){
        myNum = num;
        myString = str;
    }

    public byte[] GetBytes(){
        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(memoryStream, this);
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    public static UndoRedo Load(byte[] array){
        if (array != null){
            object obj = new BinaryFormatter().Deserialize(new MemoryStream(array));

            if(obj is UndoRedo)
                return obj as UndoRedo;
            else
                throw new ApplicationException("Unable to deserialize type" + obj.GetType());
        }
        else
            return null;
    }
}
