    using System;
    using UnityEditor;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class AutoProjectStructure : EditorWindow
    {
        private string projectName;
        private string addFolderName;
        private List<string> projectFolders;
        private string[] hierarchyElements = {"= System","= Debug","= Management","= World","= Sounds","= GamePlay", "= UI"};
        
        private Vector2 scrollPos;
        private readonly string[] defaultList = 
        {
            // Art
            "Art/Materials",
            "Art/Models/Characters",
            "Art/Models/Environment",
            "Art/Models/Props",
            "Art/Textures/Characters",
            "Art/Textures/Environment",
            "Art/Textures/UI",
            "Art/Animations",
            "Art/Shaders",
        
            // Audio
            "Audio/Music",
            "Audio/SFX",
            "Audio/Voice",
        
            // Code
            "Code/Scripts/Core",
            "Code/Scripts/Gameplay",
            "Code/Scripts/UI",
            "Code/Scripts/Utils",
            "Code/ScriptableObjects",
        
            // Prefabs
            "Prefabs/UI",
            "Prefabs/Gameplay",
            "Prefabs/Environment",
            "Prefabs/Systems",
        
            // Scenes
            "Scenes/Levels",
            "Scenes/UI",
            "Scenes/Testing",
        
            // Settings
            "Settings/Input",
            "Settings/Graphics",
        
            // Resources
            "Resources",
        
            // Root folders
            "Plugins",
            "StreamingAssets"
        };

        [MenuItem("Tools/Auto Project Structure")]
        public static void ShowWindow()
        {
            var window = GetWindow<AutoProjectStructure>("Auto Project Structure");
            window.minSize = new Vector2(550, 700);
        }

        private void OnEnable()
        {
            projectFolders= new List<string>(defaultList);
        }

        void OnGUI()
        {
            projectName = EditorGUILayout.TextField("Project Name", projectName);
            GUILayout.Space(10);
            StructureView();
            DrawButtons();
        }

        void StructureView()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(550), GUILayout.Height(500));
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < projectFolders.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(projectFolders[i]);
                if (GUILayout.Button("Remove"))
                {
                    projectFolders.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        void DrawButtons()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            addFolderName = EditorGUILayout.TextField("Project Name", addFolderName);
            if (GUILayout.Button("Add Folder"))
            {
                if (EditorUtility.DisplayDialog("Confirm Creation",
                        $"This will create the folder under 'Assets/{projectName}/'\nAre you sure?",
                        "Create", "Cancel"))
                {
                    projectFolders.Add(addFolderName);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(projectName));
            if (GUILayout.Button("Generate Project Structure",GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Confirm Creation",
                        $"This will create the folder structure under 'Assets/{projectName}/'\nAre you sure?",
                        "Create", "Cancel"))
                {
                    CreateProjectStructure();
                }
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Reset Structure", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Confirm Creation",
                        $"This will Reset Change you do",
                        "Reset", "Cancel"))
                {
                    projectFolders = new List<string>(defaultList);
                }
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Create Hierarchy Layout", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Confirm Creation",
                        "This will create a hierarchy strucute gameObjects Are you sure?",
                        "Create", "Cancel"))
                {
                    CreateHierarchyStructure();
                }
            }
            
        }

        void CreateProjectStructure()
        {
            string basePath = Path.Combine("Assets", projectName);
            try
            {
                foreach (string folder in projectFolders)
                {
                    string fullPath = Path.Combine(basePath, folder);
                    Directory.CreateDirectory(fullPath);
                }
                
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success", 
                    "Project structure created successfully!", "OK");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error creating folder structure: {e.Message}");
                EditorUtility.DisplayDialog("Error", 
                    "Failed to create folder structure. Check console for details.", "OK");
            }
        }

        void CreateHierarchyStructure()
        {
                foreach (string s in hierarchyElements)
                {
                    GameObject hierarchyObject = new GameObject(s);
                    Undo.RegisterCreatedObjectUndo(hierarchyObject, s);
                }
                EditorUtility.DisplayDialog("Success", 
                    "Create Hierarchy Structure Success.", "OK");
        }
        
    }
