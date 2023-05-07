//#define ENABLED
#if ENABLED
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Prototools.UnityPackageExport_3PLE {
    public class UnityPackageExport_3PLE : EditorWindow {
        static System.Text.StringBuilder sbPaths = new System.Text.StringBuilder();
        static List<string> paths;
        static string[] studentFolderPaths;
        static string[] studentFolderNames;
        bool exportTagsAndLayers = false, exportInputManager = false, exportProjSettings = false;

        static Dictionary<string, GUIStyle> STYLES = new Dictionary<string, GUIStyle>();
        static void SetGUIStyle(string s, GUIStyle gs) {
            if ( !STYLES.ContainsKey( s ) ) { // Add a new style
                STYLES.Add( s, gs );
            } else { // Or override the style
                STYLES[s] = gs;
            }
        }
        static void SetGUIStyles() {
            GUIStyle gs;
            RectOffset roPadding = new RectOffset( 4, 4, 4, 4 );

            // Normal
            gs = new GUIStyle();
            gs.fontStyle = FontStyle.Normal;
            gs.padding = roPadding;
            SetGUIStyle( "Normal", gs );

            // Bold
            gs = new GUIStyle();
            gs.fontStyle = FontStyle.Bold;
            gs.padding = roPadding;
            SetGUIStyle( "Bold", gs );

            // Box
            gs = new GUIStyle();
            gs.normal.background = Texture2D.whiteTexture;
            gs.padding = roPadding;
            gs.margin = roPadding;
            SetGUIStyle( "Box", gs );

            // Button
            gs = new GUIStyle();
            gs.fontStyle = FontStyle.Bold;
            gs.padding = new RectOffset( 16, 16, 4, 4 );
            SetGUIStyle( "Button", gs );

            // Wrap
            gs = new GUIStyle();
            gs.wordWrap = true;
            gs.padding = new RectOffset( 16, 16, 4, 4 );
            SetGUIStyle( "Wrap", gs );

        }


        [MenuItem( "3PLE/Export Student Folder to UnityPackage", false, 2 )]
        public static void OpenWindow() {
            paths = new List<string>();
            string studentFolderPath = "Assets/__Student Folders";
            studentFolderPaths = AssetDatabase.GetSubFolders( studentFolderPath );
            studentFolderNames = new string[studentFolderPath.Length];
            for ( int i = 0; i < studentFolderPaths.Length; i++ ) {
                string[] bits = studentFolderPaths[i].Split( '/' );
                studentFolderNames[i] = bits[bits.Length - 1];
            }

            // Set up GUIStyles and such
            SetGUIStyles();

            UnityPackageExport_3PLE window = GetWindow( typeof( UnityPackageExport_3PLE ) ) as UnityPackageExport_3PLE;
            window.titleContent = new GUIContent( "Export 3PLE Level UnityPackage" );
            window.initCountDown = 2;
            window.ShowUtility();
        }

        //bool altHeld = false;
        //void OnSceneGUI() {
        //    Event e = Event.current;
        //    switch ( e.type ) {
        //    case EventType.KeyDown:
        //        if ( Event.current.modifiers == EventModifiers.Alt ) altHeld = true;
        //        break;
        //    case EventType.KeyUp:
        //        if ( Event.current.modifiers == EventModifiers.Alt ) altHeld = false;
        //        break;
        //    }
        //}

        static string fileNameDefault = "[Your Name]";
        public string fileName;
        int studentSelection = 0;
        int phaseSelection = 0;
        string[] phases = { "Blockout", "Beta", "Final" };

        int initCountDown = 2;
        void OnGUI() {
            if (initCountDown > 0) {
                initCountDown--;
                if ( initCountDown == 0 ) {
                    PositionWindow();
                }
            }

            if ( !STYLES.ContainsKey( "Bold" ) ) SetGUIStyles();
            GUILayout.Label( "Choose your folder", STYLES["Bold"] );
            studentSelection = EditorGUILayout.Popup( studentSelection, studentFolderNames );
            GUILayout.Space( 10 );

            GUILayout.Label( "Choose project phase", STYLES["Bold"] );
            phaseSelection = EditorGUILayout.Popup( phaseSelection, phases );
            GUILayout.Space( 10 );

            GUILayout.BeginHorizontal( STYLES["Button"] );
            if ( GUILayout.Button( "Abort" ) ) {
                Close();
            }
            GUILayout.Space( 16 );
            if ( GUILayout.Button( "OK" ) ) {
                if ( fileName == fileNameDefault ) {
                    PopUp popUp = GetWindow( typeof( PopUp ) ) as PopUp;
                    popUp.text = $"You must select a filename other than\n\n\"{fileName}\"";
                    popUp.ShowPopup();
                } else {
                    Debug.Log( fileName );
                    ExportPackage();
                }
            }
            GUILayout.EndHorizontal();


            GUILayout.Space( 10 );
            GUILayout.Label( "Export Filename:", STYLES["Bold"] );
            GUILayout.Label( GenFileName(), STYLES["Wrap"] );


            //if (GUILayout.Button("Log Position")) {
            //    Debug.Log( position );
            //}
            //this.Repaint();
        }

        void PositionWindow() {
            //Debug.Log( Screen.currentResolution );
            //Debug.Log( position );
            float w = position.width;
            float h = position.height;
            position = new Rect( 100, 100, w, h );
            //position = new Rect( ( Screen.currentResolution.width - w ) / 2, ( Screen.currentResolution.height - h ) / 2, w, h );


            //Debug.Log( position );
        }

        string GenFileName() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append( "MI330_Grabbie_" );
            sb.Append( phases[phaseSelection] );
            sb.Append( "_" );
            sb.Append( studentFolderNames[studentSelection] );
            return $"{sb.ToString()}_{System.DateTime.Now.ToDateTimeStamp()}.unitypackage";
        }

        void ExportPackage() {
            if ( exportTagsAndLayers ) paths.Add( "ProjectSettings/TagManager.asset" );
            if ( exportInputManager ) paths.Add( "ProjectSettings/InputManager.asset" );
            if ( exportProjSettings ) paths.Add( "ProjectSettings/ProjectSettings.asset" );
            //string[] projectContent = new string[] { "Assets", "ProjectSettings/TagManager.asset", "ProjectSettings/InputManager.asset", "ProjectSettings/ProjectSettings.asset" };

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append( "MI330_Grappler_" );
            //sb.Append( phases[phaseSelection] );
            //sb.Append( "_" );
            //sb.Append( studentFolderNames[studentSelection] );
            string filePath = $"../{GenFileName()}";

            paths.Clear();
            paths.Add( studentFolderPaths[studentSelection] );
            AssetDatabase.ExportPackage( paths.ToArray(), filePath, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse );// | ExportPackageOptions.IncludeDependencies );

            Debug.Log( "UnityPackage exported to: " + filePath );

            PopUp popUp = GetWindow( typeof( PopUp ) ) as PopUp;
            popUp.text = $"UnityPackage exported to:\n\t{filePath}\n\n(it's right next to your Project folder)";
            popUp.ShowPopup();
            Close();
        }

    }

    public class PopUp : EditorWindow {
        public string text;
        int w = 400, h = 400;

        void Awake() {
            maxSize = new Vector2( w, h );
            minSize = maxSize / 2;
            //this.CenterOnMainWin();
            position = new Rect( ( Screen.currentResolution.width - ( w / 2 ) ) / 2, ( Screen.currentResolution.height - ( h / 2 ) ) / 2, w, h );
            //Rect mainWindowRect = EditorGUIUtility.GetMainWindowPosition();
            //Rect pos = position;
            //pos.x -= pos.width / 2;
            //pos.y -= pos.height / 2;
            //position = pos;
        }

        void OnGUI() {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.wordWrap = true;
            style.alignment = TextAnchor.UpperCenter;
            style.padding = new RectOffset( 4, 4, 4, 4 );
            GUIStyle gsButton = new GUIStyle( GUI.skin.button );
            //gsButton.alignment = TextAnchor.LowerCenter;
            gsButton.margin = new RectOffset( w / 4, w / 4, 4, 4 );
            //gsButton.fixedWidth = w / 4;

            GUILayout.Space( 10 );
            GUILayout.Label( text, style );
            GUILayout.Space( 10 );
            if ( GUILayout.Button( "OK", gsButton ) ) this.Close();
        }
    }

    public static class DateTimeExtension {
        static public string ToTimeStamp( this System.DateTime time ) {
            string str = $"{time.Hour:00}{time.Minute:00}";
            return str;
        }

        static public string ToDateStamp( this System.DateTime time ) {
            string str = $"{time.Year:0000}-{time.Month:00}-{time.Day:00}";
            return str;
        }

        static public string ToDateTimeStamp( this System.DateTime time ) {
            return $"{time.ToDateStamp()}_{time.ToTimeStamp()}";
        }
    }
}
#endif