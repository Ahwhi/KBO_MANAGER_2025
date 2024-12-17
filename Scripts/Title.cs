using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject LoadPanel, LoadDatePanel1, LoadDatePanel2, LoadDatePanel3;
    public Button FirstNewButton, FirstLoadButton, LoadButton, BackButton, DeleteButton;
    public Material DefaultMaterial;
    private bool isLoad, SelectData1, SelectData2, SelectData3;
    private string fileName1, fileName2, fileName3;

    void Start()
    {
        SetPathGettingFile();
    }

    void Update()
    {
        if ((SelectData1 || SelectData2 || SelectData3) && isLoad)
        {
            SceneDirector.SetButton(LoadButton, DefaultMaterial, true);
            SceneDirector.SetButton(DeleteButton, DefaultMaterial, true);
        } else if (!SelectData1 && SelectData2 && !SelectData3 && isLoad)
        {
            SceneDirector.SetButton(LoadButton, DefaultMaterial, false);
            SceneDirector.SetButton(DeleteButton, DefaultMaterial, false);
        }
        else if (!isLoad)
        {
            SceneDirector.SetButton(LoadButton, DefaultMaterial, false);
            SceneDirector.SetButton(DeleteButton, DefaultMaterial, false);
        }
    }

    public void OnNewGame()
    {
        SceneManager.LoadScene("SelectTeam");
    }

    public void OnLoadGame()
    {
        LoadPanel.SetActive(true);
        isLoad = true;
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void OnLoadButton()
    {
        if (SelectData1)
        {
            SaveLoad.LoadData(fileName1);
        }
        if (SelectData2)
        {
            SaveLoad.LoadData(fileName2);
        }
        if (SelectData3)
        {
            SaveLoad.LoadData(fileName3);
        }
    }

    public void OnBackButton()
    {
        isLoad = false;
        SelectData1 = false;
        SelectData2 = false;
        SelectData3 = false;
        LoadDatePanel3.GetComponent<Image>().color = Color.white;
        LoadDatePanel1.GetComponent<Image>().color = Color.white;
        LoadDatePanel2.GetComponent<Image>().color = Color.white;
        LoadPanel.SetActive(false);
    }

    public void OnDeleteButton()
    {
        if (SelectData1)
        {
            DeleteFile(fileName1, LoadDatePanel1);
        }
        if (SelectData2)
        {
            DeleteFile(fileName2, LoadDatePanel2);
        }
        if (SelectData3)
        {
            DeleteFile(fileName3, LoadDatePanel3);
        }
    }

    void SetPathGettingFile()
    {
        fileName1 = Path.Combine(Application.persistentDataPath, "GameData1.txt");
        fileName2 = Path.Combine(Application.persistentDataPath, "GameData2.txt");
        fileName3 = Path.Combine(Application.persistentDataPath, "GameData3.txt");
        if (!File.Exists(fileName1) && !File.Exists(fileName2) && !File.Exists(fileName3))
        {
            GameDirector.currentFile = "GameData1.txt";
            SceneDirector.SetButton(FirstLoadButton, DefaultMaterial, false);
        }

        if (File.Exists(fileName1) && File.Exists(fileName2) && File.Exists(fileName3))
        {
            SceneDirector.SetButton(FirstNewButton, DefaultMaterial, false);
        }

        if (!File.Exists(fileName3))
        {
            GameDirector.currentFile = "GameData3.txt";
            LoadDatePanel3.SetActive(false);
        }
        else
        {
            LoadSome(fileName3, LoadDatePanel3);
        }

        if (!File.Exists(fileName2))
        {
            GameDirector.currentFile = "GameData2.txt";
            LoadDatePanel2.SetActive(false);
        }
        else
        {
            LoadSome(fileName2, LoadDatePanel2);
        }

        if (!File.Exists(fileName1))
        {
            GameDirector.currentFile = "GameData1.txt";
            LoadDatePanel1.SetActive(false);
        }
        else
        {
            LoadSome(fileName1, LoadDatePanel1);
        }
    }

    public void OnSelectPanel1()
    {
        LoadDatePanel1.GetComponent<Image>().color = Color.cyan;
        LoadDatePanel2.GetComponent<Image>().color = Color.white;
        LoadDatePanel3.GetComponent<Image>().color = Color.white;
        SelectData1 = true;
        SelectData2 = false;
        SelectData3 = false;
    }

    public void OnSelectPanel2()
    {
        LoadDatePanel2.GetComponent<Image>().color = Color.cyan;
        LoadDatePanel1.GetComponent<Image>().color = Color.white;
        LoadDatePanel3.GetComponent<Image>().color = Color.white;
        SelectData1 = false;
        SelectData2 = true;
        SelectData3 = false;
    }

    public void OnSelectPanel3()
    {
        LoadDatePanel3.GetComponent<Image>().color = Color.cyan;
        LoadDatePanel1.GetComponent<Image>().color = Color.white;
        LoadDatePanel2.GetComponent<Image>().color = Color.white;
        SelectData1 = false;
        SelectData2 = false;
        SelectData3 = true;
    }

    private void LoadSome(string LoadedData, GameObject DataPanel)
    {
        string filename = LoadedData;
        using (StreamReader reader = new StreamReader(filename))
        {
            string line;
            reader.ReadLine();
            line = reader.ReadLine();
            string[] dateParts = line.Split('-');
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text = "";
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += int.Parse(dateParts[0]);
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += "-";
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += int.Parse(dateParts[1]);
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += "-";
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += int.Parse(dateParts[2]);
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += "-";
            reader.ReadLine();
            line = reader.ReadLine();
            DataPanel.GetComponentInChildren<TextMeshProUGUI>().text += DataToString.TeamToString((TeamName)Enum.Parse(typeof(TeamName), line));
        }
    }

    private void DeleteFile(string fileName, GameObject dataPanel)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            dataPanel.GetComponentInChildren<TextMeshProUGUI>().text = "저장된 데이터가 없습니다.";
            dataPanel.SetActive(false);
            if (!File.Exists(fileName1) && !File.Exists(fileName2) && !File.Exists(fileName3))
            {
                SceneDirector.SetButton(FirstLoadButton, DefaultMaterial, false);
            }
            else
            {
                SceneDirector.SetButton(FirstNewButton, DefaultMaterial, true);
                SceneDirector.SetButton(FirstLoadButton, DefaultMaterial, true);
            }
            SelectData1 = SelectData2 = SelectData3 = false;
            SceneDirector.SetButton(LoadButton, DefaultMaterial, false);
            SceneDirector.SetButton(DeleteButton, DefaultMaterial, false);
            SetPathGettingFile();
        }
    }
}
