using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : FindGM
{
    [Tooltip("��ⷶΧ")] public bool Sphere_Search = false;
    [Tooltip("��ǰĿ����")] public bool Line_Target = false;
    [Tooltip("�����Ϣ��Ӱ����")] public bool Line_Pheromenes_Source = false;
    [Tooltip("��Ϣ����")] public bool Line_Pheromenes = false;

    [Tooltip("�����Ƿ�����")] public bool NoDie_WorkerBee = false;
    [Tooltip("�����Ƿ�ἢ��")] public bool NoHunger_WorkerBee = false;

    [Tooltip("��Ϣ���Ƿ������")] public bool NoDie_Pheromones = false;


    //ʱ�����ϵͳ
     


    public void Sphere_Search_Switch()
    {
        if(Sphere_Search == true)
        {
            Sphere_Search = false;
        }else
        {
            Sphere_Search = true;
        }

    }
    public void Line_Target_Switch()
    {
        if (Line_Target == true)
        {
            Line_Target = false;
        }
        else
        {
            Line_Target = true;
        }

    }
    public void Line_Pheromenes_Source_Switch()
    {
        if (Line_Pheromenes_Source == true)
        {
            Line_Pheromenes_Source = false;
        }
        else
        {
            Line_Pheromenes_Source = true;
        }

    }
    public void Line_Pheromenes_Switch()
    {
        if (Line_Pheromenes == true)
        {
            Line_Pheromenes = false;
        }
        else
        {
            Line_Pheromenes = true;
        }

    }
    public void NoDie_WorkerBee_Switch()
    {
        if (NoDie_WorkerBee == true)
        {
            NoDie_WorkerBee = false;
        }
        else
        {
            NoDie_WorkerBee = true;
        }

    }

    public void NoHunger_WorkerBee_Switch()
    {
        if (NoHunger_WorkerBee == true)
        {
            NoHunger_WorkerBee = false;
        }
        else
        {
            NoHunger_WorkerBee = true;
        }

    }
    public void NoDie_Pheromones_Switch()
    {
        if (NoDie_Pheromones == true)
        {
            NoDie_Pheromones = false;
        }
        else
        {
            NoDie_Pheromones = true;
        }

    }

    public void Time_Controller(GameObject button)
    {
        //time 0 ʱ����ͣ 1 1���٣� 2 2���٣�3 4����
        GM.TimeSpeed++;
        if (GM.TimeSpeed > 8)
        {
            GM.TimeSpeed = 0;
        }
        //����int������ʱ��
        button.GetComponent<Text>().text = "Speed X" + GM.TimeSpeed;
    }

    public void InsObj_Null()
    {
        GM.InsOBJ = null;
    }
    public void InsObj_Food()
    {
        GM.InsOBJ = GM.Food_InsObj;
    }
    public void InsObj_Worker()
    {
        GM.InsOBJ = GM.Worker_InsObj;
    }
    public void Game_Replay()
    {
        SceneManager.LoadScene(0);
    }
    public void Game_Quit()
    {
        Application.Quit();
    }



}
