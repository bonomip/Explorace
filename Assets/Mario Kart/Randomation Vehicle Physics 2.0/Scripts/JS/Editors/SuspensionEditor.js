﻿#if UNITY_EDITOR
@CustomEditor(typeof(Suspension))
@CanEditMultipleObjects

public class SuspensionEditor extends Editor
{
	var isPrefab : boolean = false;
	static var showButtons : boolean = true;

	public override function OnInspectorGUI()
	{
		var boldFoldout : GUIStyle = new GUIStyle(EditorStyles.foldout);
		boldFoldout.fontStyle = FontStyle.Bold;
		var targetScript : Suspension = target as Suspension;
		var allTargets : Suspension[] = new Suspension[targets.Length];
		isPrefab = PrefabUtility.GetPrefabType(targetScript) == PrefabType.Prefab;

		for (var i : int = 0; i < targets.Length; i++)
		{
			Undo.RecordObject(targets[i], "Suspension Change");
			allTargets[i] = targets[i] as Suspension;
		}

		if (!targetScript.wheel)
		{
			EditorGUILayout.HelpBox("Wheel must be assigned.", MessageType.Error);
		}

		DrawDefaultInspector();

		if (!isPrefab && targetScript.gameObject.activeInHierarchy)
		{
			showButtons = EditorGUILayout.Foldout(showButtons, "Quick Actions", boldFoldout);
			EditorGUI.indentLevel ++;
			if (showButtons)
			{
				if (GUILayout.Button("Get Wheel"))
				{
					for (var curTarget : Suspension in allTargets)
					{
						curTarget.wheel = curTarget.transform.GetComponentInChildren(Wheel);
					}
				}

				if (GUILayout.Button("Get Opposite Wheel"))
				{
					for (var curTarget : Suspension in allTargets)
					{
						var vp : VehicleParent = F.GetTopmostParentComponent(VehicleParent, curTarget.transform) as VehicleParent;
						var closestOne : Suspension = null;
						var closeDist : float = Mathf.Infinity;

						for (var curWheel : Wheel in vp.wheels)
						{
							var curDist : float = Vector2.Distance(new Vector2(curTarget.transform.localPosition.y, curTarget.transform.localPosition.z), new Vector2(curWheel.transform.parent.localPosition.y, curWheel.transform.parent.localPosition.z));
							if (Mathf.Sign(curTarget.transform.localPosition.x) != Mathf.Sign(curWheel.transform.parent.localPosition.x) && curDist < closeDist)
							{
								closeDist = curDist;
								closestOne = curWheel.transform.parent.GetComponent(Suspension);
							}
						}

						curTarget.oppositeWheel = closestOne;
					}
				}
			}
			EditorGUI.indentLevel --;
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty(targetScript);
		}
	}
}
#endif