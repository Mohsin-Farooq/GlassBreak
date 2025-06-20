using UnityEngine;
using UnityEditor;
using System.Collections;

public class CarPaintEditor : ShaderGUI
{
	enum EFlakeType { None, Blend, Cutoff };
	EFlakeType m_FlakeType = EFlakeType.None;
	bool m_MultiLayerEmission = false;

	void CollectShaderFeatureState (MaterialEditor me)
	{
		m_MultiLayerEmission = false;

		Material mat = me.target as Material;
		string[] sks = mat.shaderKeywords;
		for (int i = 0; i < sks.Length; i++)
		{
			if (sks[i].CompareTo ("FLAKES_COLOR_BLEND") == 0)
				m_FlakeType = EFlakeType.Blend;
			else if (sks[i].CompareTo ("FLAKES_COLOR_CUTOFF") == 0)
				m_FlakeType = EFlakeType.Cutoff;

			if (sks[i].CompareTo ("MULTI_LAYER_EMISSION") == 0)
				m_MultiLayerEmission = true;
		}
	}
	void DrawShaderFeatures (MaterialEditor me)
	{
		EditorGUIUtility.fieldWidth = 64;
		EditorGUIUtility.labelWidth = 180;
		EditorGUILayout.BeginVertical("GroupBox");
		{
			m_FlakeType = (EFlakeType)EditorGUILayout.EnumPopup ("Flake Type", m_FlakeType);
			m_MultiLayerEmission = EditorGUILayout.Toggle ("Multi Layer Emission", m_MultiLayerEmission);
		}
		EditorGUILayout.EndVertical();
	}
	void ChangeShaderFeature (MaterialEditor me)
	{
		Material mat = me.target as Material;
		if (m_FlakeType == EFlakeType.None)
		{
			mat.DisableKeyword ("FLAKES_COLOR_BLEND");
			mat.DisableKeyword ("FLAKES_COLOR_CUTOFF");
		}
		else if (m_FlakeType == EFlakeType.Blend)
		{
			mat.EnableKeyword ("FLAKES_COLOR_BLEND");
			mat.DisableKeyword ("FLAKES_COLOR_CUTOFF");
		}
		else if (m_FlakeType == EFlakeType.Cutoff)
		{
			mat.DisableKeyword ("FLAKES_COLOR_BLEND");
			mat.EnableKeyword ("FLAKES_COLOR_CUTOFF");
		}

		if (m_MultiLayerEmission)
			mat.EnableKeyword ("MULTI_LAYER_EMISSION");
		else
			mat.DisableKeyword ("MULTI_LAYER_EMISSION");
	}
	public override void OnGUI (MaterialEditor me, MaterialProperty[] props)
	{
		CollectShaderFeatureState (me);
		EditorGUI.BeginChangeCheck ();
		DrawShaderFeatures (me);
		if (EditorGUI.EndChangeCheck ())
			ChangeShaderFeature (me);
		
		EditorGUIUtility.fieldWidth = 64;
		EditorGUIUtility.labelWidth = 180;
		for (int i = 0; i < props.Length; i++)
		{
			MaterialProperty prop = props[i];
			me.ShaderProperty (prop, prop.displayName);
		}
	}
}
