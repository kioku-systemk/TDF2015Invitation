using UnityEngine;
using System.Collections;

public class SpectrumToShader : MonoBehaviour
{
    public Reaktion.ReaktorLink m_spectrum1;
    public Reaktion.ReaktorLink m_spectrum2;
    public Reaktion.ReaktorLink m_spectrum3;
    public Reaktion.ReaktorLink m_spectrum4;
    public Vector4 m_spectrum;

    void Update()
    {
        m_spectrum = new Vector4(m_spectrum1.Output, m_spectrum2.Output, m_spectrum3.Output, m_spectrum4.Output);
        Shader.SetGlobalVector("_spectrum", m_spectrum);
    }
}
