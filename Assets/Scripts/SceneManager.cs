using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public string m_scene_name_to_load = "sampleDemo";
    public float m_progress;

    void Start()
    {
    
    }
    
    void Update()
    {
        m_progress = Application.GetStreamProgressForLevel(m_scene_name_to_load);
        Debug.Log("loading... " + m_progress*100.0f + " %");
        if (m_progress == 1.0f)
        {
            Application.LoadLevel(m_scene_name_to_load);
        }
    }
}
